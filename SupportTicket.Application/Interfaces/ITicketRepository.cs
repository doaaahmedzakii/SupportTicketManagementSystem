using SupportTicket.Domain.Entities;
using SupportTicket.Domain.Enums;
namespace SupportTicket.Application.Interfaces
{
    public interface ITicketRepository
    {
        Task<IEnumerable<Ticket>> GetAllAsync(
        TicketStatus? status,
        TicketPriority? priority);
        Task<Ticket?> GetByIdAsync(int id);
        Task<Ticket> CreateAsync(Ticket ticket);
        Task<Ticket> UpdateAsync(Ticket ticket);
        Task<bool> DeleteAsync(int id);
    }
}
