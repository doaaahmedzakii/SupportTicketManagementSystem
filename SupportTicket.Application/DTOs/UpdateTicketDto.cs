using SupportTicket.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportTicket.Application.DTOs
{
    public class UpdateTicketDto
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
        public TicketPriority priority { get; set; }
        public TicketStatus status { get; set; }
    }
}
