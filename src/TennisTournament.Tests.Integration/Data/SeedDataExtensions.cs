// Métodos de extensión para inicialización de datos.
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TennisTournament.Infrastructure.Data;
using TennisTournament.Domain.Entities;
using TennisTournament.Domain.Enums;
using TennisTournament.Tests.Integration.Framework;

namespace TennisTournament.Tests.Integration.Data;

public static class SeedDataExtensions
{
  public static async Task<Guid> GetOrCreateTournamentIdAsync(CustomWebApplicationFactory factory)
  {
    using var scope = factory.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<TournamentDbContext>();

    // Leer configuración desde JSON
    var configPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Config", "TournamentTestConfig.json");
    if (File.Exists(configPath))
    {
      var json = await File.ReadAllTextAsync(configPath);
      var config = JsonSerializer.Deserialize<TournamentTestConfig>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
      if (config != null && config.EnableTournamentSeed)
      {
        var tournamentConfig = config.Tournaments?.FirstOrDefault(t => t.Enabled);
        if (tournamentConfig != null)
        {
          // Crear jugadores solo si no existen
          foreach (var p in tournamentConfig.Players)
          {
            var playerId = Guid.Parse(p.Id);
            if (!context.MalePlayers.Any(mp => mp.Id == playerId))
            {
              var player = new MalePlayer
              {
                Id = playerId,
                Name = p.Name,
                SkillLevel = p.SkillLevel,
                Strength = p.Strength,
                Speed = p.Speed
              };
              await context.MalePlayers.AddAsync(player);
            }
          }
          await context.SaveChangesAsync();

          // Obtener los jugadores creados para asociar al torneo
          var playerIds = tournamentConfig.Players.Select(p => Guid.Parse(p.Id)).ToList();
          var players = context.MalePlayers.Where(mp => playerIds.Contains(mp.Id)).Cast<Player>().ToList();

          // Crear torneo solo si no existe
          var tournamentId = Guid.Parse(tournamentConfig.Id);
          var existingTournament = context.Tournaments.FirstOrDefault(t => t.Id == tournamentId);
          if (existingTournament == null)
          {
            var newTournament = new Tournament
            {
              Id = tournamentId,
              Type = Enum.Parse<TournamentType>(tournamentConfig.Type),
              StartDate = DateTime.Parse(tournamentConfig.StartDate),
              EndDate = DateTime.Parse(tournamentConfig.EndDate),
              Status = Enum.Parse<TournamentStatus>(tournamentConfig.Status)
            };
            newTournament.SetPlayers(players);
            context.Tournaments.Add(newTournament);
            await context.SaveChangesAsync();
            return newTournament.Id;
          }
          else
          {
            return existingTournament.Id;
          }
        }
      }
    }
    // Si no hay torneo habilitado en el JSON, buscar uno existente en la base
    var tournament = context.Tournaments.FirstOrDefault();
    if (tournament != null)
      return tournament.Id;
    throw new InvalidOperationException("No hay torneos configurados ni existentes en la base de datos.");
  }

  // Clases auxiliares para deserialización
  private class TournamentTestConfig
  {
    public bool EnableTournamentSeed { get; set; }
    public TournamentConfig[]? Tournaments { get; set; }
  }
  private class TournamentConfig
  {
    public bool Enabled { get; set; }
    public string Id { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string StartDate { get; set; } = string.Empty;
    public string EndDate { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public PlayerConfig[] Players { get; set; } = Array.Empty<PlayerConfig>();
  }
  private class PlayerConfig
  {
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int SkillLevel { get; set; }
    public int Strength { get; set; }
    public int Speed { get; set; }
    public string PlayerType { get; set; } = string.Empty;
  }
}
