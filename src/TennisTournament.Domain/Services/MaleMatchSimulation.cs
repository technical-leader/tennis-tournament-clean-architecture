using System;
using TennisTournament.Domain.Entities;

namespace TennisTournament.Domain.Services
{
    /// <summary>
    /// Implementación de la estrategia de simulación para partidos masculinos.
    /// </summary>
    public class MaleMatchSimulation : IMatchSimulationStrategy
    {
        private readonly Random _random;

        /// <summary>
        /// Constructor que inicializa el generador de números aleatorios.
        /// </summary>
        public MaleMatchSimulation()
        {
            _random = new Random();
        }

        /// <summary>
        /// Simula un partido masculino y determina el ganador.
        /// Considera el nivel de habilidad, la fuerza, la velocidad y un factor de suerte.
        /// </summary>
        /// <param name="match">Partido a simular.</param>
        /// <returns>Jugador ganador del partido.</returns>
        public Player SimulateMatch(Match match)
        {
            if (match == null)
                throw new ArgumentNullException(nameof(match));

            if (match.Player1 == null || match.Player2 == null)
                throw new ArgumentException("Ambos jugadores deben estar asignados al partido.");

            if (!(match.Player1 is MalePlayer) || !(match.Player2 is MalePlayer))
                throw new ArgumentException("Ambos jugadores deben ser de tipo masculino para esta estrategia de simulación.");

            // Generar factores de suerte aleatorios para cada jugador (entre 0 y 20)
            double luckFactor1 = _random.NextDouble() * 20;
            double luckFactor2 = _random.NextDouble() * 20;

            // Determinar el ganador usando el método de la clase Match
            return match.DetermineWinner(luckFactor1, luckFactor2);
        }
    }
}