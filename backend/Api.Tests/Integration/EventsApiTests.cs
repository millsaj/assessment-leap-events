using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;

namespace Api.Tests.Integration;

[TestFixture]
public class EventsApiTests
{
    private WebApplicationFactory<Api.Program> _factory = null!;
    private HttpClient _client = null!;

    [SetUp]
    public void SetUp()
    {
        // create factory and client using real DB
        // TODO: implement proper test DB setup, teardown and seeding
        _factory = new WebApplicationFactory<Api.Program>();
        _client = _factory.CreateClient();
    }

    [TearDown]
    public void TearDown()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [Test]
    public async Task GetUpcomingEvents_ReturnsEvents()
    {
        // Act
        var response = await _client.GetAsync("/api/events?days=30");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var events = await response.Content.ReadFromJsonAsync<object[]>();
        events.Should().NotBeNull();
        events.Should().NotBeEmpty();
    }

    [Test]
    public async Task GetTicketsForEvent_ReturnsTickets()
    {
        // Act
        var eventsResponse = await _client.GetAsync("/api/events?days=30");
        var events = await eventsResponse.Content.ReadFromJsonAsync<System.Text.Json.JsonElement[]>();
        events.Should().NotBeNull();
        events.Should().NotBeEmpty();
        var eventId = events.First().GetProperty("id").GetGuid();

        var ticketsResponse = await _client.GetAsync($"/api/events/{eventId}/tickets");

        // Assert
        ticketsResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var tickets = await ticketsResponse.Content.ReadFromJsonAsync<object[]>();
        tickets.Should().NotBeNull();
    }

    [Test]
    public async Task GetTopEventsByCount_ReturnsTopEvents()
    {
        // Act
        var response = await _client.GetAsync("/api/top-events?by=count");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var topEvents = await response.Content.ReadFromJsonAsync<object[]>();
        topEvents.Should().NotBeNull();
        topEvents.Should().NotBeEmpty();
    }

    [Test]
    public async Task GetTopEventsByRevenue_ReturnsTopEvents()
    {
        // Act
        var response = await _client.GetAsync("/api/top-events?by=revenue");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var topEvents = await response.Content.ReadFromJsonAsync<object[]>();
        topEvents.Should().NotBeNull();
        topEvents.Should().NotBeEmpty();
    }

    [Test]
    public async Task GetTicketsForEvent_ReturnsNotFound_WhenEventDoesNotExist()
    {
        // Act
        var response = await _client.GetAsync($"/api/events/{Guid.NewGuid()}/tickets");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var tickets = await response.Content.ReadFromJsonAsync<object[]>();
        tickets.Should().BeEmpty();
    }


    [Test]
    public async Task GetTopEventsBy_InvalidByParam_ReturnsBadRequestOrEmpty()
    {
        // Act
        var response = await _client.GetAsync("/api/top-events?by=invalid");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
