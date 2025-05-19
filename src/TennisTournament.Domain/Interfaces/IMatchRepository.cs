using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TennisTournament.Domain.Entities;

namespace TennisTournament.Domain.Interfaces
{
    /// <summary>
    /// Interfaz para el repositorio de partidos.
    /// </summary>
    public interface IMatchRepository
    {
        /// <summary>
        /// Obtiene todos los partidos.
        /// </summary>
        /// <returns>Lista de partidos.</returns>
        Task<IEnumerable<Match>> GetAllAsync();

        /// <summary>
        /// Obtiene un partido por su identificador.
        /// </summary>
        /// <param name="id">Identificador del partido.</param>
        /// <returns>Partido encontrado o null si no existe.</returns>
        Task<Match?> GetByIdAsync(Guid id);

        /// <summary>
        /// Obtiene los partidos de un torneo específico.
        /// </summary>
        /// <param name="tournamentId">Identificador del torneo.</param>
        /// <returns>Lista de partidos del torneo.</returns>
        Task<IEnumerable<Match>> GetByTournamentIdAsync(Guid tournamentId);

        /// <summary>
        /// Obtiene los partidos de una ronda específica de un torneo.
        /// </summary>
        /// <param name="tournamentId">Identificador del torneo.</param>
        /// <param name="round">Número de ronda.</param>
        /// <returns>Lista de partidos de la ronda especificada.</returns>
        Task<IEnumerable<Match>> GetByTournamentAndRoundAsync(Guid tournamentId, int round);

        /// <summary>
        /// Obtiene los partidos en los que ha participado un jugador específico.
        /// </summary>
        /// <param name="playerId">Identificador del jugador.</param>
        /// <returns>Lista de partidos en los que ha participado el jugador.</returns>
        Task<IEnumerable<Match>> GetByPlayerIdAsync(Guid playerId);

        /// <summary>
        /// Añade un nuevo partido.
        /// </summary>
        /// <param name="match">Partido a añadir.</param>
        /// <returns>Partido añadido.</returns>
        Task<Match> AddAsync(Match match);

        /// <summary>
        /// Añade múltiples partidos.
        /// </summary>
        /// <param name="matches">Lista de partidos a añadir.</param>
        /// <returns>Lista de partidos añadidos.</returns>
        Task<IEnumerable<Match>> AddRangeAsync(IEnumerable<Match> matches);

        /// <summary>
        /// Actualiza un partido existente.
        /// </summary>
        /// <param name="match">Partido con los datos actualizados.</param>
        /// <returns>Partido actualizado.</returns>
        Task<Match> UpdateAsync(Match match);

        /// <summary>
        /// Elimina un partido por su identificador.
        /// </summary>
        /// <param name="id">Identificador del partido a eliminar.</param>
        /// <returns>True si se eliminó correctamente, false en caso contrario.</returns>
        Task<bool> DeleteAsync(Guid id);
    }
}