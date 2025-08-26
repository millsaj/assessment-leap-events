namespace Api.Domain.Repositories;

using Api.Domain.Entities;

public interface ITicketRepository
{
    IEnumerable<TicketSale> GetTicketsByEventId(Guid eventId);
    IEnumerable<(Event Event, int Score)> GetTopEventsBySalesCount(int top);
    IEnumerable<(Event Event, int Score)> GetTopEventsByRevenue(int top);
}
