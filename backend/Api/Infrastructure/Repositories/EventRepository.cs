using System.Collections.Generic;
using System.Linq;
using Api.Domain.Entities;
using Api.Domain.Repositories;
using NHibernate;

namespace Api.Infrastructure.Repositories;

public class EventRepository : IEventRepository
{
    private readonly NHibernate.ISession _session;

    public EventRepository(NHibernate.ISession session)
    {
        _session = session;
    }

    public IEnumerable<Event> GetUpcomingEvents(
        int days,
        string? sortBy = null,
        string? sortOrder = null,
        int? page = null,
        int? pageSize = null
    )
    {
        var min = DateTime.UtcNow;
        var max = min.AddDays(days);
        var query = _session.Query<Event>()
            .Where(e => e.StartsOn >= min && e.StartsOn <= max);

        // Sorting
        if (sortBy == "name")
            query = sortOrder == "desc" ? query.OrderByDescending(e => e.Name) : query.OrderBy(e => e.Name);
        else // default or "startDate"
            query = sortOrder == "desc" ? query.OrderByDescending(e => e.StartsOn) : query.OrderBy(e => e.StartsOn);

        // Pagination
        if (page.HasValue && pageSize.HasValue && page > 0 && pageSize > 0)
            query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);

        return query.ToList();
    }

    public Event? GetById(Guid id)
    {
        return _session.Get<Event>(id);
    }
}
