using FluentNHibernate.Mapping;
using Api.Domain.Entities;

namespace Api.Infrastructure.Mappings;

public class EventMap : ClassMap<Event>
{
    public EventMap()
    {
        Table("Events");

        Id(x => x.Id).GeneratedBy.Assigned();

        Map(x => x.Name).Not.Nullable();
        Map(x => x.StartsOn).Not.Nullable();
        Map(x => x.EndsOn).Not.Nullable();
        Map(x => x.Location).Not.Nullable();
    }
}
