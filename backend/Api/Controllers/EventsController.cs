using Api.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/events")]
public class EventsController : ControllerBase
{
    private readonly IEventService _eventService;
    private readonly ITicketService _ticketService;
    public EventsController(IEventService eventService, ITicketService ticketService)
    {
        _eventService = eventService;
        _ticketService = ticketService;
    }

    [HttpGet]
    public IActionResult GetUpcoming(
        [FromQuery] int days = 30,
        [FromQuery] string? sortBy = null,
        [FromQuery] string? sortOrder = null,
        [FromQuery] int? page = null,
        [FromQuery] int? pageSize = null
    )
    {
        if (days != 30 && days != 60 && days != 180) return BadRequest();
        if (sortBy != null && sortBy != "name" && sortBy != "startDate") return BadRequest();
        if (sortOrder != null && sortOrder != "asc" && sortOrder != "desc") return BadRequest();
        if ((page.HasValue && page <= 0) || (pageSize.HasValue && pageSize <= 0)) return BadRequest();
        var events = _eventService.GetUpcomingEvents(days, sortBy, sortOrder, page, pageSize);
        return Ok(events);
    }

    [HttpGet("{id}/tickets")]
    public IActionResult GetTickets(Guid id)
    {
        var tickets = _ticketService.GetTicketsByEventId(id);
        return Ok(tickets);
    }
}
