namespace Api.Domain.Repositories;

using Api.Domain.Entities;

public interface IEventRepository
{
    IEnumerable<Event> GetUpcomingEvents(
        int days,
        string? sortBy = null,
        string? sortOrder = null,
        int? page = null,
        int? pageSize = null
    );
    Event? GetById(Guid id);
}
