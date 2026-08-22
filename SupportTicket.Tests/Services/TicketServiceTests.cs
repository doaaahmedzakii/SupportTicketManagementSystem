
using Moq;
using Microsoft.Extensions.Logging;
using AutoMapper;
using SupportTicket.Application.Interfaces;
using SupportTicket.Application.Services;
using SupportTicket.Application.DTOs;
using SupportTicket.Domain.Entities;
using SupportTicket.Domain.Enums;
using SupportTicket.Application.Exceptions;

namespace SupportTicket.Tests.Services
{
    public class TicketServiceTests
    {
        private readonly Mock<ITicketRepository> _ticketRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<TicketService>> _loggerMock;
        private readonly ITicketService _ticketService;

        public TicketServiceTests()
        {
            _ticketRepositoryMock = new Mock<ITicketRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<TicketService>>();
            _ticketService = new TicketService(
                _ticketRepositoryMock.Object
                , _mapperMock.Object
                , _loggerMock.Object);

        }

        [Fact]
        public async Task CreateTicketAsync_ValidTicket_ReturnsTicket()
        {
            var ticketDto = new CreateTicketDto
            {
                Title = "Ticket 1",
                Description = "Description For Ticket 1",
                CustomerEmail = "cus1@gmail.com"
            };
            var responseDto = new TicketResponseDto
            {
                Title = "Ticket 1",
                Description = "Description For Ticket 1",
                CustomerEmail = "cus1@gmail.com",
                Status = TicketStatus.Open
            };
            var ticket = new Ticket
            {
                CreatedAt = DateTime.UtcNow,
                Id = 1,
                Title = "Ticket 1",
                Description = "Description For Ticket 1",
                CustomerEmail = "cus1@gmail.com"
            };
            _mapperMock.Setup(x => x.Map<Ticket>(ticketDto))
                .Returns(ticket);
            _mapperMock.Setup(x => x.Map<TicketResponseDto>(ticket))
                .Returns(responseDto);
            _ticketRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Ticket>()))
                .ReturnsAsync(ticket);
            var result = await _ticketService.CreateTicketAsync(ticketDto);
            Assert.NotNull(result);
            Assert.Equal("Ticket 1", result.Title);
            Assert.Equal("cus1@gmail.com", result.CustomerEmail);
            Assert.Equal(TicketStatus.Open, result.Status);
            _ticketRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<Ticket>()), Times.Once);

        }


        [Fact]
        public async Task CreateTicketAsync_InvalidEmail_Rejected()
        {
            var ticketDto = new CreateTicketDto
            {
                Title = "Ticket 2",
                Description = "Description For Ticket 2",
                CustomerEmail = "cus1gmail.com",
                Priority = TicketPriority.Medium
            };
            
           await Assert.ThrowsAsync<ArgumentException>(
               () => _ticketService.CreateTicketAsync(ticketDto));

            _ticketRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<Ticket>()), Times.Never);
        }

        [Fact]
        public async Task CreateTicketAsync_TitleLessThan5_Rejected()
        {
            var ticketDto = new CreateTicketDto
            {
                Title = "Tick",
                Description = "Description For Ticket 2",
                CustomerEmail = "cus1@gmail.com",
                Priority = TicketPriority.Medium
            };

            await Assert.ThrowsAsync<ArgumentException>(
                () => _ticketService.CreateTicketAsync(ticketDto));

            _ticketRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<Ticket>()), Times.Never);
        }

        [Fact]
        public async Task UpdateTicketAsync_ClosedToOpen_Rejected()
        {
            var ticketId = 2;
            var existingTicket = new Ticket
            {
                Id = ticketId,
                Title = "Ticket 2",
                Description = "Description For Ticket 2",
                CustomerEmail = "cus2@gmail.com",
                Priority = TicketPriority.Medium,
                Status = TicketStatus.Closed,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                UpdatedAt = DateTime.UtcNow.AddHours(-2)
            };
            var updatedDto = new UpdateTicketDto
            {
                 status = TicketStatus.Open
            };
            _ticketRepositoryMock.Setup(x=>x.GetByIdAsync(ticketId))
                .ReturnsAsync(existingTicket);
            await Assert.ThrowsAsync<InvalidStatusTransitionException>(
                () => _ticketService.UpdateTicketAsync(ticketId, updatedDto));
            _ticketRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Ticket>()),
                Times.Never);
            Assert.Equal(TicketStatus.Closed , existingTicket.Status);
        }
        [Fact]
        public async Task UpdateTicketAsync_OpenToResolved_Rejected()
        {
            var ticketId = 2;
            var existingTicket = new Ticket
            {
                Id = ticketId,
                Title = "Ticket 2",
                Description = "Description For Ticket 2",
                CustomerEmail = "cus2@gmail.com",
                Priority = TicketPriority.Medium,
                Status = TicketStatus.Open,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                UpdatedAt = DateTime.UtcNow.AddHours(-2)
            };
            var updatedDto = new UpdateTicketDto
            {
                status = TicketStatus.Resolved
            };
            _ticketRepositoryMock.Setup(x => x.GetByIdAsync(ticketId))
                .ReturnsAsync(existingTicket);
            await Assert.ThrowsAsync<InvalidStatusTransitionException>(
                () => _ticketService.UpdateTicketAsync(ticketId, updatedDto));
            _ticketRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Ticket>()),
                Times.Never);
            Assert.Equal(TicketStatus.Open, existingTicket.Status);
        }
        [Fact]
        public async Task UpdateTicketAsync_InprogressToResolve_Succeeds()
        {
            var ticketId = 2;
            var existingTicket = new Ticket
            {
                Id = ticketId,
                Title = "Ticket 2",
                Description = "Description For Ticket 2",
                CustomerEmail = "cus2@gmail.com",
                Priority = TicketPriority.Medium,
                Status = TicketStatus.InProgress,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                UpdatedAt = DateTime.UtcNow.AddHours(-2)
            };
            var updatedDto = new UpdateTicketDto
            {
                status = TicketStatus.Resolved
            };
            _ticketRepositoryMock.Setup(x => x.GetByIdAsync(ticketId))
                .ReturnsAsync(existingTicket);
            _ticketRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Ticket>()))
                .ReturnsAsync(existingTicket);
            _mapperMock.Setup(x => x.Map(updatedDto, existingTicket))
                .Callback<UpdateTicketDto, Ticket>((dto, ticket) =>
                {
                    ticket.Status = dto.status;
                });

            await _ticketService.UpdateTicketAsync(ticketId, updatedDto);
            _ticketRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Ticket>()),
                Times.Once);
            Assert.Equal(TicketStatus.Resolved, existingTicket.Status);
        }
    }
}
