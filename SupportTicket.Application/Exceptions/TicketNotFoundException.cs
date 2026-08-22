

namespace SupportTicket.Application.Exceptions
{
    public class TicketNotFoundException :Exception
    {
        public TicketNotFoundException(string message)
        : base(message)
        {
        }
    }
}
