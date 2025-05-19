using System;
using TennisTournament.Domain.Entities;

namespace TennisTournament.Domain.Services
{
    /// <summary>
    /// Implementación de la estrategia de simulación para partidos femeninos.
    /// </summary>
    public class FemaleMatchSimulation : IMatchSimulationStrategy
    {
        private readonly Random _random;

        /// <summary>
        /// Constructor que inicializa el generador de números aleatorios.
        /// </summary>
        public FemaleMatchSimulation()
        {
            _random = new Random();
        }

        /// <summary>
        /// Simula un partido femenino y determina la ganadora.
        /// Considera el nivel de habilidad, el tiempo de reacción y un factor de suerte.
        /// </summary>
        /// <param name="match">Partido a simular.</param>
        /// <returns>Jugadora ganadora del partido.</returns>
        public Player SimulateMatch(Match match)
        {
            if (match == null)
                throw new ArgumentNullException(nameof(match));

            if (match.Player1 == null || match.Player2 == null)
                throw new ArgumentException("Ambas jugadoras deben estar asignadas al partido.");

            if (!(match.Player1 is FemalePlayer) || !(match.Player2 is FemalePlayer))
                throw new ArgumentException("Ambas jugadoras deben ser de tipo femenino para esta estrategia de simulación.");

            // Generar factores de suerte aleatorios para cada jugadora (entre 0 y 20)
            double luckFactor1 = _random.NextDouble() * 20;
            double luckFactor2 = _random.NextDouble() * 20;

            // Determinar la ganadora usando el método de la clase Match
            return match.DetermineWinner(luckFactor1, luckFactor2);
        }
    }
}