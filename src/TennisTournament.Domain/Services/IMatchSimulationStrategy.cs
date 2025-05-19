using TennisTournament.Domain.Entities;

namespace TennisTournament.Domain.Services
{
    /// <summary>
    /// Interfaz que define la estrategia para simular un partido de tenis.
    /// </summary>
    public interface IMatchSimulationStrategy
    {
        /// <summary>
        /// Simula un partido de tenis y determina el ganador.
        /// </summary>
        /// <param name="match">Partido a simular.</param>
        /// <returns>Jugador ganador del partido.</returns>
        Player SimulateMatch(Match match);
    }
}