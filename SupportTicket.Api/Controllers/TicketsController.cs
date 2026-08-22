using Microsoft.AspNetCore.Mvc;
using SupportTicket.Application.DTOs;
using SupportTicket.Application.Interfaces;
using SupportTicket.Application.Services;
using SupportTicket.Domain.Enums;

namespace SupportTicket.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }
        [HttpPost]
        public async Task<IActionResult>  CreateTicket(CreateTicketDto dto)
        {
            var ticket = await _ticketService.CreateTicketAsync(dto);
            return CreatedAtAction(nameof(GetTicketById), new { id = ticket.Id }, ticket);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicketById (int id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);
            if (ticket is null) return NotFound($"The Ticket with this {id} not found");
            return Ok(ticket);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTickets(
            TicketStatus? status , TicketPriority? priority)
        {
            var tickets = await _ticketService.GetAllTicketsAsync(status,priority);
            return Ok(tickets);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTicket(UpdateTicketDto dto , int id)
        {
            var ticket = await _ticketService.UpdateTicketAsync(id ,dto);
            if (ticket is null) return NotFound("The ticket is not found");
            return Ok(ticket);
        }
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> ChangeStatus(int id , UpdateTicketStatusDto dto)
        {
            var ticket = await _ticketService.ChangeTicketStatusAsync(id, dto);
            if (ticket is null) return NotFound("This Ticket is not found!");
            return Ok(ticket);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
           await _ticketService.DeleteTicketAsync(id);
            return NoContent();
        }
    }
}
