using System;
using System.Collections.Generic;
using System.Linq;
using TennisTournament.Domain.Entities;
using TennisTournament.Domain.Enums;

namespace TennisTournament.Domain.Services
{
    /// <summary>
    /// Servicio para simular un torneo completo.
    /// </summary>
    public class TournamentSimulationService
    {
        private readonly MatchSimulationStrategyFactory _strategyFactory;

        /// <summary>
        /// Constructor que inicializa la fábrica de estrategias de simulación.
        /// </summary>
        /// <param name="strategyFactory">Fábrica de estrategias de simulación.</param>
        public TournamentSimulationService(MatchSimulationStrategyFactory strategyFactory)
        {
            _strategyFactory = strategyFactory ?? throw new ArgumentNullException(nameof(strategyFactory));
        }

        /// <summary>
        /// Simula un torneo completo y devuelve el resultado.
        /// </summary>
        /// <param name="tournament">Torneo a simular.</param>
        /// <returns>Resultado del torneo con el ganador.</returns>
        public Result SimulateTournament(Tournament tournament)
        {
            if (tournament == null)
                throw new ArgumentNullException(nameof(tournament));

            if (tournament.Players == null || !tournament.Players.Any())
                throw new ArgumentException("El torneo debe tener jugadores asignados.");

            // Verificar que el número de jugadores sea potencia de 2
            int playerCount = tournament.Players.Count;
            if ((playerCount & (playerCount - 1)) != 0)
                throw new ArgumentException("El número de jugadores debe ser una potencia de 2.");

            // Generar emparejamientos iniciales
            var matches = tournament.GenerateInitialMatches();
            var currentRound = 1;
            var strategy = _strategyFactory.CreateStrategy(tournament.Type);
            var allMatches = new List<Match>(matches);

            // Simular todas las rondas hasta llegar al campeón
            while (matches.Count > 0)
            {
                var winners = new List<Player>();

                // Simular todos los partidos de la ronda actual
                foreach (var match in matches)
                {
                    var winner = strategy.SimulateMatch(match);
                    winners.Add(winner);
                }

                // Si solo queda un ganador, el torneo ha terminado
                if (winners.Count == 1)
                {
                    tournament.Status = TournamentStatus.Completed;
                    tournament.EndDate = DateTime.UtcNow;

                    // Crear el resultado
                    var result = new Result
                    {
                        Tournament = tournament,
                        TournamentId = tournament.Id,
                        Winner = winners[0],
                        WinnerId = winners[0].Id,
                        CompletionDate = DateTime.UtcNow
                    };

                    // Usar el método SetMatches en lugar de asignar directamente a la propiedad
                    result.SetMatches(allMatches);

                    // Validar que todos los partidos tengan ganador tras la simulación
                    foreach (var m in allMatches)
                    {
                        if (m.Winner == null || m.WinnerId == Guid.Empty)
                            throw new InvalidOperationException($"El partido con Id {m.Id} no tiene un ganador asignado tras la simulación.");
                    }

                    return result;
                }

                // Generar emparejamientos para la siguiente ronda
                currentRound++;
                matches = tournament.GenerateNextRoundMatches(winners, currentRound);
                allMatches.AddRange(matches);
            }

            throw new InvalidOperationException("Error al simular el torneo: no se pudo determinar un ganador.");
        }
    }
}