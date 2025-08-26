using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Api.Domain.Entities;
using Api.Domain.Repositories;
using NHibernate;

namespace Api.Infrastructure.Repositories;

public class TicketRepository : ITicketRepository
{
    private readonly NHibernate.ISession _session;

    public TicketRepository(NHibernate.ISession session)
    {
        _session = session;
    }

    public IEnumerable<TicketSale> GetTicketsByEventId(Guid eventId)
    {
        return _session.Query<TicketSale>()
            .Where(t => t.Event.Id == eventId)
            .ToList();
    }

    public IEnumerable<(Event Event, int Score)> GetTopEventsBySalesCount(int top)
    {
        var hql = @"
            select ts.Event, count(ts.Id) as SalesCount
            from TicketSale ts
            group by ts.Event
            order by SalesCount desc
        ";

        return GetTopEvents(hql, top);
    }

    public IEnumerable<(Event Event, int Score)> GetTopEventsByRevenue(int top)
    {
        var hql = @"
            select ts.Event, sum(ts.PriceInCents) as Revenue
            from TicketSale ts
            group by ts.Event
            order by Revenue desc
        ";

        return GetTopEvents(hql, top);
    }

    private IEnumerable<(Event Event, int Score)> GetTopEvents(string hql, int top)
    {
        var results = _session.CreateQuery(hql)
            .SetMaxResults(top)
            .List<object[]>();

        return results.Select(r => ((Event)r[0], Convert.ToInt32(r[1])));
    }
}
