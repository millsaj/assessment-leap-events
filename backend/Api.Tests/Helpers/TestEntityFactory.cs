using System;
using Api.Domain.Entities;

namespace Api.Tests.Helpers;

public static class TestEntityFactory
{
    public static Event CreateEvent(Guid? id = null, string? name = null)
    {
        return new Event
        {
            Id = id ?? Guid.NewGuid(),
            Name = name ?? "Test Event",
            StartsOn = DateTime.UtcNow.AddDays(1),
            EndsOn = DateTime.UtcNow.AddDays(2),
            Location = "Test Location"
        };
    }

    public static TicketSale CreateTicketSale(Guid? id = null, Event? ev = null, int priceInCents = 1000)
    {
        return new TicketSale
        {
            Id = id ?? Guid.NewGuid(),
            Event = ev ?? CreateEvent(),
            UserId = 1,
            PurchaseDate = DateTime.UtcNow,
            PriceInCents = priceInCents
        };
    }
}
