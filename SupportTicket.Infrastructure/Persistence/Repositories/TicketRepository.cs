
using Microsoft.EntityFrameworkCore;
using SupportTicket.Application.Interfaces;
using SupportTicket.Domain.Entities;
using SupportTicket.Domain.Enums;

namespace SupportTicket.Infrastructure.Persistence.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly AppDbContext _context;
        public TicketRepository (AppDbContext context) { 
            _context = context;
        }
        public async Task<IEnumerable<Ticket>> GetAllAsync(
            TicketStatus? status,
            TicketPriority? priority)
        {
            IQueryable<Ticket> query = _context.Tickets;
            if(status.HasValue) 
                query = query.Where( x => x.Status == status.Value );
            if (priority.HasValue)
                query = query.Where( x => x.Priority == priority.Value);
            return await _context.Tickets.ToListAsync();
        }
        public async Task<Ticket> CreateAsync(Ticket ticket)
        {
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket is null)
            {
                return false;
            }
            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Ticket?> GetByIdAsync(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket is null)
            {
                return null;
            }
            return ticket;
        }

        public async Task<Ticket> UpdateAsync(Ticket ticket)
        {
            var updatedTicket = await _context.Tickets.FindAsync(ticket.Id);
            if (updatedTicket is null)
            {
                throw new Exception($"Ticket with ID {ticket.Id} not found.");
            }
            return updatedTicket;
        }
    }
}
