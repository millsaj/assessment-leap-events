using System;
using System.Collections.Generic;
using System.Linq;
using Api.Domain.Entities;
using Api.Infrastructure.Repositories;
using FluentAssertions;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace Api.Tests.Unit.Infrastructure.Repositories;

[TestFixture]
public class TicketRepositoryTests : DbTestBase
{
    private TicketRepository _repo = null!;

    [SetUp]
    public void SetUpRepo()
    {
        _repo = new TicketRepository(Session);
    }

    [Test]
    public void GetTicketsByEventId_ReturnsTicketsForEvent()
    {
        // Arrange
        var ev = new Event { Id = Guid.NewGuid(), Name = "E", StartsOn = DateTime.UtcNow, EndsOn = DateTime.UtcNow.AddHours(1) };
        Session.Save(ev);
        var t1 = new TicketSale { Event = ev, PriceInCents = 1000, PurchaseDate = DateTime.UtcNow, UserId = 1 };
        var t2 = new TicketSale { Event = ev, PriceInCents = 2000, PurchaseDate = DateTime.UtcNow, UserId = 2 };
        Session.Save(t1);
        Session.Save(t2);
        Session.Flush();

        // Act
        var tickets = _repo.GetTicketsByEventId(ev.Id).ToList();

        // Assert
        tickets.Should().HaveCount(2);
        tickets.All(t => t.Event.Id == ev.Id).Should().BeTrue();
    }

    [Test]
    public void GetTicketsByEventId_ReturnsEmptyForEventWithNoTickets()
    {
        // Arrange
        var ev = new Event { Id = Guid.NewGuid(), Name = "NoTickets", StartsOn = DateTime.UtcNow, EndsOn = DateTime.UtcNow.AddHours(1) };
        Session.Save(ev);
        Session.Flush();

        // Act
        var tickets = _repo.GetTicketsByEventId(ev.Id).ToList();

        // Assert
        tickets.Should().BeEmpty();
    }

    [Test]
    public void GetTicketsByEventId_ReturnsEmptyForNonexistentEvent()
    {
        // Act
        var tickets = _repo.GetTicketsByEventId(Guid.NewGuid()).ToList();

        // Assert
        tickets.Should().BeEmpty();
    }

    [Test]
    public void GetTopEventsBySalesCount_ReturnsEventsSortedBySalesCount()
    {
        // Arrange
        var ev1 = new Event { Id = Guid.NewGuid(), Name = "A", StartsOn = DateTime.UtcNow, EndsOn = DateTime.UtcNow.AddHours(1) };
        var ev2 = new Event { Id = Guid.NewGuid(), Name = "B", StartsOn = DateTime.UtcNow, EndsOn = DateTime.UtcNow.AddHours(1) };
        Session.Save(ev1);
        Session.Save(ev2);
        for (int i = 0; i < 3; i++)
            Session.Save(new TicketSale { Event = ev1, PriceInCents = 1000, PurchaseDate = DateTime.UtcNow, UserId = i + 1 });
        for (int i = 0; i < 2; i++)
            Session.Save(new TicketSale { Event = ev2, PriceInCents = 2000, PurchaseDate = DateTime.UtcNow, UserId = i + 10 });
        Session.Flush();

        // Act
        var top = _repo.GetTopEventsBySalesCount(2).ToList();

        // Assert
        top.Should().HaveCount(2);
        top[0].Event.Name.Should().Be("A");
        top[0].Score.Should().Be(3);
        top[1].Event.Name.Should().Be("B");
        top[1].Score.Should().Be(2);
    }

    [Test]
    public void GetTopEventsBySalesCount_PaginatesResultsCorrectly()
    {
        // Arrange
        var events = new List<Event>();
        for (int i = 0; i < 10; i++)
        {
            var ev = new Event { Id = Guid.NewGuid(), Name = $"Event{i}", StartsOn = DateTime.UtcNow, EndsOn = DateTime.UtcNow.AddHours(1) };
            Session.Save(ev);
            events.Add(ev);
            for (int j = 0; j < i; j++)
                Session.Save(new TicketSale { Event = ev, PriceInCents = 1000, PurchaseDate = DateTime.UtcNow, UserId = j + 1 });
        }
        Session.Flush();

        // Act
        var top5 = _repo.GetTopEventsBySalesCount(5).ToList();

        // Assert
        top5.Should().HaveCount(5);
        top5[0].Score.Should().Be(9);
        top5[4].Score.Should().Be(5);
    }

    [Test]
    public void GetTopEventsByRevenue_ReturnsEventsSortedByRevenue()
    {
        // Arrange
        var ev1 = new Event { Id = Guid.NewGuid(), Name = "A", StartsOn = DateTime.UtcNow, EndsOn = DateTime.UtcNow.AddHours(1) };
        var ev2 = new Event { Id = Guid.NewGuid(), Name = "B", StartsOn = DateTime.UtcNow, EndsOn = DateTime.UtcNow.AddHours(1) };
        Session.Save(ev1);
        Session.Save(ev2);
        Session.Save(new TicketSale { Event = ev1, PriceInCents = 1000, PurchaseDate = DateTime.UtcNow, UserId = 1 });
        Session.Save(new TicketSale { Event = ev1, PriceInCents = 2000, PurchaseDate = DateTime.UtcNow, UserId = 2 });
        Session.Save(new TicketSale { Event = ev2, PriceInCents = 5000, PurchaseDate = DateTime.UtcNow, UserId = 3 });
        Session.Flush();

        // Act
        var top = _repo.GetTopEventsByRevenue(2).ToList();

        // Assert
        top.Should().HaveCount(2);
        top[0].Event.Name.Should().Be("B");
        top[0].Score.Should().Be(5000);
        top[1].Event.Name.Should().Be("A");
        top[1].Score.Should().Be(3000);
    }

    [Test]
    public void GetTopEventsByRevenue_PaginatesResultsCorrectly()
    {
        // Arrange
        var events = new List<Event>();
        for (int i = 0; i < 10; i++)
        {
            var ev = new Event { Id = Guid.NewGuid(), Name = $"Event{i}", StartsOn = DateTime.UtcNow, EndsOn = DateTime.UtcNow.AddHours(1) };
            Session.Save(ev);
            events.Add(ev);
            for (int j = 0; j < i; j++)
                Session.Save(new TicketSale { Event = ev, PriceInCents = (j + 1) * 100, PurchaseDate = DateTime.UtcNow, UserId = j + 1 });
        }
        Session.Flush();

        // Act
        var top5 = _repo.GetTopEventsByRevenue(5).ToList();

        // Assert
        top5.Should().HaveCount(5);
        top5[0].Score.Should().BeGreaterThan(top5[4].Score);
    }

    [Test]
    public void GetTopEventsBySalesCount_ReturnsEmptyWhenNoSales()
    {
        // Act
        var top = _repo.GetTopEventsBySalesCount(5).ToList();

        // Assert
        top.Should().BeEmpty();
    }

    [Test]
    public void GetTopEventsByRevenue_ReturnsEmptyWhenNoSales()
    {
        // Act
        var top = _repo.GetTopEventsByRevenue(5).ToList();

        // Assert
        top.Should().BeEmpty();
    }
}
