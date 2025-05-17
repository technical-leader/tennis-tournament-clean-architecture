// Prueba de integración para la simulación de torneo
using System;
using System.IO;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using FluentAssertions;
using TennisTournament.Application.DTOs;
using TennisTournament.Tests.Integration.Data;
using TennisTournament.Tests.Integration.Framework;
using Xunit;

namespace TennisTournament.Tests.Integration.Features;

public class SimulateTournamentTests : IClassFixture<CustomWebApplicationFactory>
{
  private readonly HttpClient _client;
  private readonly CustomWebApplicationFactory _factory;
  private readonly string _logPath = "Logs/SimulateTournamentTest.log";

  public SimulateTournamentTests(CustomWebApplicationFactory factory)
  {
    _factory = factory;
    _client = factory.CreateClient();

    // --- Lógica de ruta adaptada para entornos Docker/local ---

    // Definir un directorio base predecible dentro del contenedor (ej: /app/logs)
    // O usar una ruta temporal (ej: /tmp/logs)
    // Para compatibilidad local, puedes usar el directorio de trabajo actual o una ruta relativa simple.

    string logDirectory;
    // Detectar si estamos en un entorno Docker (esto es una simplificación, podría requerir una variable de entorno)
    bool isDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true"; // Ejemplo de detección

    if (isDocker)
    {
      // Ruta dentro del contenedor Docker
      logDirectory = Path.Combine("/app", "Logs"); // O Path.Combine("/tmp", "Logs");
    }
    else
    {
      // Lógica original para entorno local (dotnet test)
      string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
      string projectRoot = Path.GetFullPath(Path.Combine(baseDirectory, @"..\..\..\"));
      logDirectory = Path.Combine(projectRoot, "Logs");
    }

    // Asegurarse de que el directorio 'Logs' exista.
    Directory.CreateDirectory(logDirectory);

    _logPath = Path.Combine(logDirectory, "SimulateTournamentTest.log");

    // --- Fin de la lógica de ruta adaptada ---
  }

  [Fact]
  public async Task SimulateTournament_ShouldLogEventsAndReturnResult()
  {
    // Arrange: inicializar datos de prueba desde Data/SeedDataExtensions
    var tournamentId = await SeedDataExtensions.GetOrCreateTournamentIdAsync(_factory);
    var logLines = new List<string>();
    logLines.Add($"\n[{DateTime.Now}] Bajo 'Tests.Integration'\n[{DateTime.Now}] Iniciando prueba de simulación de torneo para ID: {tournamentId}");

    // Act: realizar la solicitud de simulación
    var response = await _client.PostAsync($"/api/tournaments/{tournamentId}/simulate", null);
    logLines.Add($"[{DateTime.Now}] Solicitud POST /api/tournaments/{tournamentId}/simulate enviada. Status: {response.StatusCode}");

    // Assert: verificar resultado y registrar eventos
    response.StatusCode.Should().Be(HttpStatusCode.OK);
    var options = new System.Text.Json.JsonSerializerOptions
    {
      PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
      PropertyNameCaseInsensitive = true
    };
    options.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    var result = await response.Content.ReadFromJsonAsync<ResultDto>(options);
    result.Should().NotBeNull();
    logLines.Add($"[{DateTime.Now}] Resultado recibido: Ganador = {result?.WinnerDisplay}");

    // Guardar log
    Directory.CreateDirectory("Logs");
    await File.AppendAllLinesAsync(_logPath, logLines);
  }
}
