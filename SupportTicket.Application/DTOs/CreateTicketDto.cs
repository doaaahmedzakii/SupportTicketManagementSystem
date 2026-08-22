using SupportTicket.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace SupportTicket.Application.DTOs
{
    public class CreateTicketDto
    {
        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string CustomerEmail { get; set; } = string.Empty;
        public TicketPriority Priority { get; set; }
    }
}
