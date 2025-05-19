using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TennisTournament.Domain.Entities;
using TennisTournament.Domain.Enums;

namespace TennisTournament.Infrastructure.Data
{
    /// <summary>
    /// Clase para inicializar la base de datos con datos de prueba.
    /// </summary>
    public class TournamentDbContextSeed
    {
        /// <summary>
        /// Inicializa la base de datos con datos de prueba.
        /// </summary>
        /// <param name="context">Contexto de base de datos.</param>
        /// <param name="logger">Logger para registrar información.</param>
        public static async Task SeedAsync(TournamentDbContext context, ILogger<TournamentDbContextSeed> logger)
        {
            try
            {
                // Verificar si ya existen datos
                if (!context.Players.Any())
                {
                    // Crear jugadores masculinos
                    var malePlayers = new List<MalePlayer>
                    {
                        new MalePlayer { Id = Guid.NewGuid(), Name = "Rafael Nadal", SkillLevel = 95, Strength = 90, Speed = 85 },
                        new MalePlayer { Id = Guid.NewGuid(), Name = "Novak Djokovic", SkillLevel = 98, Strength = 85, Speed = 95 },
                        new MalePlayer { Id = Guid.NewGuid(), Name = "Roger Federer", SkillLevel = 96, Strength = 80, Speed = 90 },
                        new MalePlayer { Id = Guid.NewGuid(), Name = "Andy Murray", SkillLevel = 90, Strength = 82, Speed = 88 }
                    };

                    // Crear jugadoras femeninas
                    var femalePlayers = new List<FemalePlayer>
                    {
                        new FemalePlayer { Id = Guid.NewGuid(), Name = "Serena Williams", SkillLevel = 97, ReactionTime = 92 },
                        new FemalePlayer { Id = Guid.NewGuid(), Name = "Naomi Osaka", SkillLevel = 93, ReactionTime = 90 },
                        new FemalePlayer { Id = Guid.NewGuid(), Name = "Ashleigh Barty", SkillLevel = 91, ReactionTime = 88 },
                        new FemalePlayer { Id = Guid.NewGuid(), Name = "Simona Halep", SkillLevel = 89, ReactionTime = 86 }
                    };

                    // Agregar jugadores a la base de datos
                    await context.MalePlayers.AddRangeAsync(malePlayers);
                    await context.FemalePlayers.AddRangeAsync(femalePlayers);
                    await context.SaveChangesAsync();

                    logger.LogInformation("Datos de jugadores sembrados correctamente.");
                }

                // Verificar si ya existen torneos
                if (!context.Tournaments.Any())
                {
                    // Crear torneos de ejemplo
                    var malePlayers = await context.MalePlayers.ToListAsync();
                    var femalePlayers = await context.FemalePlayers.ToListAsync();

                    if (malePlayers.Count >= 4)
                    {
                        var maleTournament = new Tournament
                        {
                            Id = Guid.NewGuid(),
                            Type = TournamentType.Male,
                            StartDate = DateTime.Now.AddDays(-7),
                            EndDate = DateTime.Now.AddDays(-6),
                            Status = TournamentStatus.Scheduled
                        };
                        
                        // Usar el método SetPlayers en lugar de asignar directamente a la propiedad
                        maleTournament.SetPlayers(malePlayers.Cast<Player>().ToList());

                        await context.Tournaments.AddAsync(maleTournament);
                    }

                    if (femalePlayers.Count >= 4)
                    {
                        var femaleTournament = new Tournament
                        {
                            Id = Guid.NewGuid(),
                            Type = TournamentType.Female,
                            StartDate = DateTime.Now.AddDays(-3),
                            EndDate = DateTime.Now.AddDays(-2),
                            Status = TournamentStatus.Scheduled
                        };
                        
                        // Usar el método SetPlayers en lugar de asignar directamente a la propiedad
                        femaleTournament.SetPlayers(femalePlayers.Cast<Player>().ToList());

                        await context.Tournaments.AddAsync(femaleTournament);
                    }

                    await context.SaveChangesAsync();
                    logger.LogInformation("Datos de torneos sembrados correctamente.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al sembrar datos en la base de datos.");
                throw;
            }
        }
    }
}