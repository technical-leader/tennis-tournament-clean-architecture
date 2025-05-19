using System;
using TennisTournament.Domain.Entities;
using TennisTournament.Domain.Enums;

namespace TennisTournament.Domain.Services
{
    /// <summary>
    /// Fábrica para crear estrategias de simulación de partidos según el tipo de torneo.
    /// </summary>
    public class MatchSimulationStrategyFactory
    {
        /// <summary>
        /// Crea una estrategia de simulación de partidos según el tipo de torneo.
        /// </summary>
        /// <param name="tournamentType">Tipo de torneo.</param>
        /// <returns>Estrategia de simulación apropiada para el tipo de torneo.</returns>
        public IMatchSimulationStrategy CreateStrategy(TournamentType tournamentType)
        {
            return tournamentType switch
            {
                TournamentType.Male => new MaleMatchSimulation(),
                TournamentType.Female => new FemaleMatchSimulation(),
                _ => throw new ArgumentException($"Tipo de torneo no soportado: {tournamentType}", nameof(tournamentType))
            };
        }

        /// <summary>
        /// Crea una estrategia de simulación de partidos según el tipo de los jugadores.
        /// </summary>
        /// <param name="player">Un jugador del partido.</param>
        /// <returns>Estrategia de simulación apropiada para el tipo de jugador.</returns>
        public IMatchSimulationStrategy CreateStrategy(Player player)
        {
            return player switch
            {
                MalePlayer => new MaleMatchSimulation(),
                FemalePlayer => new FemaleMatchSimulation(),
                _ => throw new ArgumentException($"Tipo de jugador no soportado: {player.GetType().Name}", nameof(player))
            };
        }

        /// <summary>
        /// Crea una estrategia de simulación de partidos según el partido.
        /// </summary>
        /// <param name="match">Partido a simular.</param>
        /// <returns>Estrategia de simulación apropiada para el partido.</returns>
        public IMatchSimulationStrategy CreateStrategy(Match match)
        {
            if (match == null)
                throw new ArgumentNullException(nameof(match));

            if (match.Player1 == null || match.Player2 == null)
                throw new ArgumentException("El partido debe tener dos jugadores asignados.");

            // Verificar que ambos jugadores sean del mismo tipo
            if (match.Player1.GetType() != match.Player2.GetType())
                throw new ArgumentException("Ambos jugadores deben ser del mismo tipo (masculino o femenino).");

            return CreateStrategy(match.Player1);
        }
    }
}