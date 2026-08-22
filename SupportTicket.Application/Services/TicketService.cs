using AutoMapper;
using Microsoft.Extensions.Logging;
using SupportTicket.Application.DTOs;
using SupportTicket.Application.Exceptions;
using SupportTicket.Application.Interfaces;
using SupportTicket.Domain.Entities;
using SupportTicket.Domain.Enums;
using System.ComponentModel.DataAnnotations;
namespace SupportTicket.Application.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<TicketService> _logger;
        public TicketService(ITicketRepository ticketRepository , IMapper mapper
            ,ILogger<TicketService> logger)
        {
            _ticketRepository = ticketRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<TicketResponseDto>> GetAllTicketsAsync(
            TicketStatus? status,
            TicketPriority? priority)
        {
            var tickets = await _ticketRepository.GetAllAsync(status,priority);
            return _mapper.Map<IEnumerable<TicketResponseDto>>(tickets);

        }

        public async Task<TicketResponseDto> ChangeTicketStatusAsync(int ticketId, UpdateTicketStatusDto updateStatus)
        {
            var ticket = await _ticketRepository.GetByIdAsync(ticketId);
            if (ticket == null)
            {
                throw new TicketNotFoundException($"Ticket with ID not found.");
            }
            ticket.Status = updateStatus.status;
            ticket.UpdatedAt = DateTime.UtcNow;
            await _ticketRepository.UpdateAsync(ticket);
            _logger.LogInformation("Status Changed");
            return _mapper.Map<TicketResponseDto>(ticket);
        }

        public async Task<TicketResponseDto> CreateTicketAsync(CreateTicketDto ticket)
        {
            if (string.IsNullOrWhiteSpace(ticket.CustomerEmail) || 
                !new EmailAddressAttribute().IsValid(ticket.CustomerEmail))
            {
                throw new ArgumentException("Invalid email");
            }
            if (string.IsNullOrWhiteSpace(ticket.Title) ||
                ticket.Title.Length < 5)
            {
                throw new ArgumentException("title must be at least 5 chars");
            }
            var newTicket = _mapper.Map<Ticket>(ticket);
            newTicket.Status = TicketStatus.Open;
            newTicket.CreatedAt = DateTime.UtcNow;
            newTicket.UpdatedAt = DateTime.UtcNow;
            await _ticketRepository.CreateAsync(newTicket);
            _logger.LogInformation("Ticket created");
            return _mapper.Map<TicketResponseDto>(newTicket);
        }

        public async Task<bool> DeleteTicketAsync(int ticketId)
        {
            var ticket = await _ticketRepository.GetByIdAsync(ticketId);

            if (ticket is null)
                throw new TicketNotFoundException($"Ticket with {ticketId} not found.");

            await _ticketRepository.DeleteAsync(ticket.Id);
            _logger.LogInformation("Ticket Deleted");
            return true;
        }

        public async Task<TicketResponseDto> GetTicketByIdAsync(int ticketId)
        {
            var ticket = await _ticketRepository.GetByIdAsync(ticketId);
            return _mapper.Map<TicketResponseDto> ( ticket );
        }

        public async Task<TicketResponseDto> UpdateTicketAsync(int ticketId, UpdateTicketDto updateTicket)
        {
            var ticket = await _ticketRepository.GetByIdAsync(ticketId);

            if (ticket is null )
                throw new TicketNotFoundException($"Ticket with ID not found.");

            if (ticket.Status != updateTicket.status)
            {
                if (!IsValidStatusTransition(ticket.Status, updateTicket.status))
                {
                    throw new InvalidStatusTransitionException($"Invalid transition");
                }
            }

            _mapper.Map(updateTicket, ticket);
            ticket.UpdatedAt = DateTime.UtcNow;
            await _ticketRepository.UpdateAsync(ticket);
            return _mapper.Map<TicketResponseDto>(ticket);
        }

        private bool IsValidStatusTransition(
            TicketStatus currentStatus, TicketStatus nextStatus)
        {
            return currentStatus switch
            {
                TicketStatus.Open => nextStatus == TicketStatus.InProgress,
                TicketStatus.InProgress => nextStatus == TicketStatus.Resolved,
                TicketStatus.Resolved => nextStatus == TicketStatus.Closed,
                TicketStatus.Closed => false,
                _ => false
            };
        }
    }
}
