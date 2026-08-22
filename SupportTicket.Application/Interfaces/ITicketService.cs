using SupportTicket.Application.DTOs;
using SupportTicket.Domain.Enums;
namespace SupportTicket.Application.Interfaces
{
    public interface ITicketService
    {
        Task<TicketResponseDto> CreateTicketAsync(CreateTicketDto ticket);
        Task<TicketResponseDto> GetTicketByIdAsync(int ticketId);
        Task<IEnumerable<TicketResponseDto>> GetAllTicketsAsync(
        TicketStatus? status,
        TicketPriority? priority);
        Task<TicketResponseDto> ChangeTicketStatusAsync(int ticketId, UpdateTicketStatusDto updateStatus);
        Task<TicketResponseDto> UpdateTicketAsync(int ticketId, UpdateTicketDto updateTicket);
        Task<bool> DeleteTicketAsync(int ticketId);

    }
}
