using AutoMapper;
using SupportTicket.Application.DTOs;
using SupportTicket.Domain.Entities;

namespace SupportTicket.Application.Mapping
{
    public class TicketProfile : Profile
    {
        public TicketProfile()
        {
            CreateMap<Ticket, TicketResponseDto>();
            CreateMap<CreateTicketDto, Ticket>();
            CreateMap<UpdateTicketDto, Ticket>();
        }
    }
}
