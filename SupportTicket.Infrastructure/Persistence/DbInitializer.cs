using Microsoft.EntityFrameworkCore;
using SupportTicket.Domain.Entities;
using SupportTicket.Domain.Enums;

namespace SupportTicket.Infrastructure.Persistence
{
    public class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.Migrate();
            SeedTickets(context);
        }
        private static void SeedTickets(AppDbContext context)
        {
            if (!context.Tickets.Any())
            {
                var tickets = new List<Ticket>
                {
                    new Ticket
                    {
                        Title = "Sample Ticket 1",
                        Description = "This is a sample ticket.",
                        CustomerEmail = "1@gmail.com",
                        Status = TicketStatus.Open,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        Priority = TicketPriority.Low
                    },
                    new Ticket
                    {
                        Title = "Sample Ticket 2",
                        Description = "This is a sample ticket.",
                        CustomerEmail = "2@gmail.com",
                        Status = TicketStatus.Closed,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        Priority = TicketPriority.High
                    },
                    new Ticket
                    {
                        Title = "Sample Ticket 3",
                        Description = "This is a sample ticket.",
                        CustomerEmail = "3@gmail.com",
                        Status = TicketStatus.InProgress,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        Priority = TicketPriority.Critical
                    },
                    new Ticket
                    {
                        Title = "Sample Ticket 4",
                        Description = "This is a sample ticket.",
                        CustomerEmail = "4@gmail.com",
                        Status = TicketStatus.Resolved,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        Priority = TicketPriority.Medium
                    }
                };
                context.Tickets.AddRange(tickets);
                context.SaveChanges();

            }

        }
        
    }
}
