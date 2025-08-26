using Api.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/top-events")]
public class TopEventsController : ControllerBase
{
    private readonly ITicketService _ticketService;
    public TopEventsController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    [HttpGet]
    public IActionResult Get([FromQuery] string by = "count")
    {
        if (by != "count" && by != "revenue") return BadRequest();
        var result = by == "count"
            ? _ticketService.GetTopEventsBySalesCount(5)
            : _ticketService.GetTopEventsByRevenue(5);

        var dtoResult = result.Select(x => new DTOs.TopEventDto {
            Id = x.Event.Id,
            Name = x.Event.Name,
            Score = x.Score
        });

        return Ok(dtoResult);
    }
}
