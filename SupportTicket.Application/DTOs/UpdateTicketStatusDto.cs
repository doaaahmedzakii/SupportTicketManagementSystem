using SupportTicket.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportTicket.Application.DTOs
{
    public class UpdateTicketStatusDto
    {
        public TicketStatus status { get; set; }
    }
}
