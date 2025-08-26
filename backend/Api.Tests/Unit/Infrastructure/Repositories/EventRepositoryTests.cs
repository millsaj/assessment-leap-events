using System;
using System.Collections.Generic;
using System.IO;
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
public class EventRepositoryTests : DbTestBase
{
    private EventRepository _repo = null!;

    [SetUp]
    public void SetUpRepo()
    {
        _repo = new EventRepository(Session);
    }

    [Test]
    public void GetUpcomingEvents_FiltersByDaysCorrectly()
    {
        // Arrange
        var now = DateTime.UtcNow;
        var e1 = new Event { Id = Guid.NewGuid(), Name = "A", StartsOn = now.AddDays(1), EndsOn = now.AddDays(2) };
        var e2 = new Event { Id = Guid.NewGuid(), Name = "B", StartsOn = now.AddDays(10), EndsOn = now.AddDays(11) };
        var e3 = new Event { Id = Guid.NewGuid(), Name = "C", StartsOn = now.AddDays(40), EndsOn = now.AddDays(41) };
        Session.Save(e1);
        Session.Save(e2);
        Session.Save(e3);
        Session.Flush();

        // Act
        var result = _repo.GetUpcomingEvents(30).ToList();

        // Assert
        result.Should().Contain(x => x.Name == "A");
        result.Should().Contain(x => x.Name == "B");
        result.Should().NotContain(x => x.Name == "C");
    }

    [Test]
    public void GetUpcomingEvents_SortsByNameAscending()
    {
        // Arrange
        var now = DateTime.UtcNow;
        var e1 = new Event { Id = Guid.NewGuid(), Name = "Zeta", StartsOn = now.AddDays(1), EndsOn = now.AddDays(2) };
        var e2 = new Event { Id = Guid.NewGuid(), Name = "Alpha", StartsOn = now.AddDays(2), EndsOn = now.AddDays(3) };
        Session.Save(e1);
        Session.Save(e2);
        Session.Flush();

        // Act
        var result = _repo.GetUpcomingEvents(10, sortBy: "name", sortOrder: "asc").ToList();

        // Assert
        result[0].Name.Should().Be("Alpha");
        result[1].Name.Should().Be("Zeta");
    }

    [Test]
    public void GetUpcomingEvents_SortsByNameDescending()
    {
        // Arrange
        var now = DateTime.UtcNow;
        var e1 = new Event { Id = Guid.NewGuid(), Name = "Zeta", StartsOn = now.AddDays(1), EndsOn = now.AddDays(2) };
        var e2 = new Event { Id = Guid.NewGuid(), Name = "Alpha", StartsOn = now.AddDays(2), EndsOn = now.AddDays(3) };
        Session.Save(e1);
        Session.Save(e2);
        Session.Flush();

        // Act
        var result = _repo.GetUpcomingEvents(10, sortBy: "name", sortOrder: "desc").ToList();

        // Assert
        result[0].Name.Should().Be("Zeta");
        result[1].Name.Should().Be("Alpha");
    }

    [Test]
    public void GetUpcomingEvents_SortsByStartDateAscending()
    {
        // Arrange
        var now = DateTime.UtcNow;
        var e1 = new Event { Id = Guid.NewGuid(), Name = "A", StartsOn = now.AddDays(5), EndsOn = now.AddDays(6) };
        var e2 = new Event { Id = Guid.NewGuid(), Name = "B", StartsOn = now.AddDays(1), EndsOn = now.AddDays(2) };
        Session.Save(e1);
        Session.Save(e2);
        Session.Flush();

        // Act
        var result = _repo.GetUpcomingEvents(10, sortBy: "startDate", sortOrder: "asc").ToList();

        // Assert
        result[0].Name.Should().Be("B");
        result[1].Name.Should().Be("A");
    }

    [Test]
    public void GetUpcomingEvents_SortsByStartDateDescending()
    {
        // Arrange
        var now = DateTime.UtcNow;
        var e1 = new Event { Id = Guid.NewGuid(), Name = "A", StartsOn = now.AddDays(5), EndsOn = now.AddDays(6) };
        var e2 = new Event { Id = Guid.NewGuid(), Name = "B", StartsOn = now.AddDays(1), EndsOn = now.AddDays(2) };
        Session.Save(e1);
        Session.Save(e2);
        Session.Flush();

        // Act
        var result = _repo.GetUpcomingEvents(10, sortBy: "startDate", sortOrder: "desc").ToList();

        // Assert
        result[0].Name.Should().Be("A");
        result[1].Name.Should().Be("B");
    }

    [Test]
    public void GetUpcomingEvents_PaginatesResultsCorrectly()
    {
        // Arrange
        var now = DateTime.UtcNow;
        for (int i = 1; i < 15; i++)
        {
            var e = new Event { Id = Guid.NewGuid(), Name = $"Event{i}", StartsOn = now.AddDays(i), EndsOn = now.AddDays(i + 1) };
            Session.Save(e);
        }
        Session.Flush();

        // Act
        var page1 = _repo.GetUpcomingEvents(15, sortBy: "startDate", sortOrder: "asc", page: 1, pageSize: 3).ToList();
        var page2 = _repo.GetUpcomingEvents(15, sortBy: "startDate", sortOrder: "asc", page: 2, pageSize: 3).ToList();

        // Assert
        page1.Should().HaveCount(3);
        page2.Should().HaveCount(3);
        page1[0].Name.Should().Be("Event1");
        page2[0].Name.Should().Be("Event4");
    }

    [Test]
    public void GetUpcomingEvents_ReturnsEmptyWhenNoEventsInRange()
    {
        // Arrange
        var now = DateTime.UtcNow;
        var e = new Event { Id = Guid.NewGuid(), Name = "Old", StartsOn = now.AddDays(-10), EndsOn = now.AddDays(-9) };
        Session.Save(e);
        Session.Flush();

        // Act
        var result = _repo.GetUpcomingEvents(5).ToList();

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public void GetById_ReturnsCorrectEvent()
    {
        // Arrange
        var e = new Event { Id = Guid.NewGuid(), Name = "Test", StartsOn = DateTime.UtcNow, EndsOn = DateTime.UtcNow.AddHours(1) };
        var id = (Guid)Session.Save(e);
        Session.Flush();

        // Act
        var found = _repo.GetById(id);

        // Assert
        found.Should().NotBeNull();
        found!.Name.Should().Be("Test");
    }

    [Test]
    public void GetById_ReturnsNullWhenEventDoesNotExist()
    {
        // Act
        var found = _repo.GetById(Guid.NewGuid());

        // Assert
        found.Should().BeNull();
    }
}
