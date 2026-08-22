using Microsoft.EntityFrameworkCore;
using SupportTicket.Domain.Entities;

namespace SupportTicket.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Ticket> Tickets { get; set; }
    }
}
