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
using TennisTournament.Tests.Integration.Framework;
using Xunit;

namespace TennisTournament.Tests.Integration.Controllers;

public class PlayersControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
  private readonly HttpClient _client;

  public PlayersControllerIntegrationTests(CustomWebApplicationFactory factory)
  {
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task GetAll_ShouldReturnOkResult_WithListOfPlayers()
  {
    // Act
    var response = await _client.GetAsync("/api/players");

    // Log JSON response for debugging
    // var rawJson = await response.Content.ReadAsStringAsync();
    // Console.WriteLine("JSON Response: " + rawJson);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.OK);

    var options = new JsonSerializerOptions
    {
      PropertyNameCaseInsensitive = true, // Allow case-insensitive property names
      Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) } // Handle enums as camelCase
    };

    // Deserialize to a generic list of dictionaries
    var rawPlayers = await response.Content.ReadFromJsonAsync<List<Dictionary<string, object>>>(options);
    if (rawPlayers == null)
    {
      throw new InvalidOperationException("No se pudieron deserializar los jugadores.");
    }

    // Map to specific DTOs
    var players = rawPlayers.Select<Dictionary<string, object>, PlayerDto>(player =>
    {
      var playerType = player["playerType"].ToString();
      return playerType switch
      {
        "Male" => TryDeserialize<MalePlayerDto>(JsonSerializer.Serialize(player), options),
        "Female" => TryDeserialize<FemalePlayerDto>(JsonSerializer.Serialize(player), options),
        _ => throw new JsonException($"Tipo de jugador desconocido: {playerType}")
      };
    }).ToList();

    var malePlayers = players.OfType<MalePlayerDto>().ToList();
    var femalePlayers = players.OfType<FemalePlayerDto>().ToList();
    malePlayers.Should().NotBeNull();
    femalePlayers.Should().NotBeNull();
    (malePlayers.Count + femalePlayers.Count).Should().BeGreaterThan(0);
  }

  // Implemento TryDeserialize para validar que no sea nulo
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
  public async Task GetById_WithExistingId_ShouldReturnOkResult_WithPlayer()
  {
    // Arrange
    var playerId = "7b57cc6c-9324-45ae-bdea-045b96ad762d"; // Replace with a valid ID from seeded data

    // Act
    var response = await _client.GetAsync($"/api/players/{playerId}");

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.OK);

    var options = new JsonSerializerOptions
    {
      PropertyNameCaseInsensitive = true, // Allow case-insensitive property names
      Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) } // Handle enums as camelCase
    };

    // Deserialize to a generic dictionary
    var rawPlayer = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>(options);
    if (rawPlayer == null)
    {
      throw new InvalidOperationException("No se pudo deserializar el jugador.");
    }

    // Map to specific DTO
    var playerType = rawPlayer["playerType"].ToString();
    PlayerDto player = playerType switch
    {
      "Male" => TryDeserialize<MalePlayerDto>(JsonSerializer.Serialize(rawPlayer), options),
      "Female" => TryDeserialize<FemalePlayerDto>(JsonSerializer.Serialize(rawPlayer), options),
      _ => throw new JsonException($"Tipo de jugador desconocido: {playerType}")
    };

    player.Should().NotBeNull();
    player.Id.Should().Be(playerId);
  }

  [Fact]
  public async Task GetById_WithNonExistingId_ShouldReturnNotFound()
  {
    // Arrange
    var playerId = Guid.NewGuid().ToString(); // Genera un GUID cualquiera

    // Act
    var response = await _client.GetAsync($"/api/players/{playerId}");

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
  }

  [Fact]
  public async Task CreateMalePlayer_WithValidData_ShouldReturnCreatedAtAction()
  {
    // Arrange
    var playerDto = new MalePlayerDto
    {
      Name = "Don Cacho",
      SkillLevel = 20,
      Strength = 15,
      Speed = 10
    };

    var options = new JsonSerializerOptions
    {
      PropertyNameCaseInsensitive = true, // Permitir case-insensitive
      Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) } // Manejar enums correctamente
    };

    // Act
    var response = await _client.PostAsJsonAsync("/api/players/male", playerDto);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.Created);

    // Verificar si la respuesta tiene contenido antes de deserializar
    var rawJson = await response.Content.ReadAsStringAsync();
    rawJson.Should().NotBeNullOrEmpty();

    var rawPlayer = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>(options);
    rawPlayer.Should().NotBeNull();

    // Determinar el tipo de jugador y deserializar con seguridad
    var playerType = rawPlayer["playerType"].ToString();
    PlayerDto createdPlayer = playerType switch
    {
      "Male" => TryDeserialize<MalePlayerDto>(JsonSerializer.Serialize(rawPlayer), options),
      "Female" => TryDeserialize<FemalePlayerDto>(JsonSerializer.Serialize(rawPlayer), options),
      _ => throw new JsonException($"Tipo de jugador desconocido: {playerType}")
    };

    // Validaciones finales
    createdPlayer.Should().NotBeNull();
    createdPlayer.Name.Should().Be(playerDto.Name);
  }

  [Fact]
  public async Task Delete_WithExistingId_ShouldReturnNoContent()
  {
    // Arrange
    var playerId = "b1a98536-4933-4cf9-8192-78564554dfa0"; // Replace with a valid ID from seeded data

    // Act
    var response = await _client.DeleteAsync($"/api/players/{playerId}");

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.NoContent);
  }

  // Additional tests for Create, Update, and Delete endpoints can be added here
}
