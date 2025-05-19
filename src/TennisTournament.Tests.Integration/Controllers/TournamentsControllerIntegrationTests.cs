using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FluentAssertions;
using TennisTournament.Application.DTOs;
using TennisTournament.Domain.Enums;
using TennisTournament.Tests.Integration.Framework;
using Xunit;

namespace TennisTournament.Tests.Integration.Controllers;

public class TournamentsControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public TournamentsControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ShouldReturnOkResult_WithListOfTournaments()
    {
        // Act
        var response = await _client.GetAsync("/api/tournaments");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
        };

        var rawTournaments = await response.Content.ReadFromJsonAsync<List<Dictionary<string, object>>>(options);
        if (rawTournaments == null)
        {
            throw new InvalidOperationException("No se pudieron deserializar los torneos.");
        }

        var tournaments = rawTournaments.Select<Dictionary<string, object>, TournamentDto>(tournament =>
        {
            // Solo soporta eliminación directa
            return TryDeserialize<TournamentDto>(JsonSerializer.Serialize(tournament), options);
        }).ToList();

        tournaments.Should().NotBeNull();
        tournaments.Count.Should().BeGreaterThan(0);
    }

    private T TryDeserialize<T>(string json, JsonSerializerOptions options) where T : class
    {
        try
        {
            return JsonSerializer.Deserialize<T>(json, options) ?? throw new JsonException($"No se pudo deserializar {typeof(T).Name}");
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Error de JSON: {ex.Message}");
            throw;
        }
    }

    [Fact]
    public async Task GetById_WithExistingId_ShouldReturnOkResult_WithTournament()
    {
        // Arrange
        var tournamentId = "1e0ac609-c2f3-4eb2-8e0f-2a62c2828626"; // Replace with a valid ID from seeded data

        // Act
        var response = await _client.GetAsync($"/api/tournaments/{tournamentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
        };

        var rawTournament = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>(options);
        if (rawTournament == null)
        {
            throw new InvalidOperationException("No se pudo deserializar el torneo.");
        }

        var tournament = TryDeserialize<TournamentDto>(JsonSerializer.Serialize(rawTournament), options);

        tournament.Should().NotBeNull();
        tournament.Id.Should().Be(tournamentId);
    }

    [Fact]
    public async Task CreateTournament_WithValidData_ShouldReturnCreatedAtAction()
    {
        // Arrange
        var playerIds = new List<Guid>
        {
            Guid.Parse("87388471-dcdf-4e0c-a954-05ae3bcbc303"), // Rafael Nadal
            Guid.Parse("3cd6ef14-960e-4dbb-8c8c-2660657e4abf"), // Roger Federer
            Guid.Parse("ab29ea50-3f56-426d-84ff-652148813dc3"), // Novak Djokovic
            Guid.Parse("47374056-2e09-4fbd-b01c-61095f8f50cc")  // Andy Murray
        };
        var tournamentDto = new TournamentDto
        {
            Type = TournamentType.Male,
            PlayerIds = playerIds,
            StartDate = new DateTime(2025, 5, 7, 9, 54, 39, DateTimeKind.Utc)
        };

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/tournaments", tournamentDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var rawTournament = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>(options);
        rawTournament.Should().NotBeNull();

        var createdTournament = TryDeserialize<TournamentDto>(JsonSerializer.Serialize(rawTournament), options);

        createdTournament.Should().NotBeNull();
        createdTournament.Type.Should().Be(tournamentDto.Type);
        createdTournament.PlayerIds.Should().BeEquivalentTo(tournamentDto.PlayerIds);
        createdTournament.StartDate.Should().BeAfter(DateTime.UtcNow.AddMinutes(-5)); // Solo verificar que la fecha es reciente
        createdTournament.Status.Should().Be(TournamentStatus.Scheduled);
        createdTournament.Matches.Should().NotBeNull();
    }
}
