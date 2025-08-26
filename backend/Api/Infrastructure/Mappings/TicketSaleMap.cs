using FluentNHibernate.Mapping;
using Api.Domain.Entities;

namespace Api.Infrastructure.Mappings;

public class TicketSaleMap : ClassMap<TicketSale>
{
    public TicketSaleMap()
    {
        Table("TicketSales");
        Id(x => x.Id).GeneratedBy.GuidComb();

        Map(x => x.UserId).Not.Nullable();
        Map(x => x.PurchaseDate).Not.Nullable();
        Map(x => x.PriceInCents).Not.Nullable();
        References(x => x.Event).Column("EventId").Not.Nullable();
    }
}
