using Api.Domain.Entities;
using Api.Domain.Repositories;

namespace Api.Domain.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _repo;
    public EventService(IEventRepository repo)
    {
        _repo = repo;
    }
    public IEnumerable<Event> GetUpcomingEvents(
        int days,
        string? sortBy = null,
        string? sortOrder = null,
        int? page = null,
        int? pageSize = null
    ) => _repo.GetUpcomingEvents(days, sortBy, sortOrder, page, pageSize);
    public Event? GetById(Guid id) => _repo.GetById(id);
}
