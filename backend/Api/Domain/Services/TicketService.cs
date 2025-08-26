using Api.Domain.Entities;
using Api.Domain.Repositories;

namespace Api.Domain.Services;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _repo;
    public TicketService(ITicketRepository repo)
    {
        _repo = repo;
    }
    public IEnumerable<TicketSale> GetTicketsByEventId(Guid eventId) => _repo.GetTicketsByEventId(eventId);
    public IEnumerable<(Event Event, int Score)> GetTopEventsBySalesCount(int top) => _repo.GetTopEventsBySalesCount(top);
    public IEnumerable<(Event Event, int Score)> GetTopEventsByRevenue(int top) => _repo.GetTopEventsByRevenue(top);
}
