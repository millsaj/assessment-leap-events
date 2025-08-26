namespace Api.Domain.Services;

using Api.Domain.Entities;

public interface ITicketService
{
    IEnumerable<TicketSale> GetTicketsByEventId(Guid eventId);
    IEnumerable<(Event Event, int Score)> GetTopEventsBySalesCount(int top);
    IEnumerable<(Event Event, int Score)> GetTopEventsByRevenue(int top);
}
