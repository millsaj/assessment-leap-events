namespace Api.Domain.Services;

using Api.Domain.Entities;

public interface IEventService
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
