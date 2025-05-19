using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TennisTournament.Domain.Entities;
using TennisTournament.Domain.Enums;

namespace TennisTournament.Domain.Interfaces
{
    /// <summary>
    /// Interfaz para el repositorio de resultados de torneos.
    /// </summary>
    public interface IResultRepository
    {
        /// <summary>
        /// Obtiene todos los resultados.
        /// </summary>
        /// <returns>Lista de resultados.</returns>
        Task<IEnumerable<Result>> GetAllAsync();

        /// <summary>
        /// Obtiene un resultado por su identificador.
        /// </summary>
        /// <param name="id">Identificador del resultado.</param>
        /// <returns>Resultado encontrado o null si no existe.</returns>
        Task<Result?> GetByIdAsync(Guid id);

        /// <summary>
        /// Obtiene el resultado de un torneo específico.
        /// </summary>
        /// <param name="tournamentId">Identificador del torneo.</param>
        /// <returns>Resultado del torneo o null si no existe.</returns>
        Task<Result?> GetByTournamentIdAsync(Guid tournamentId);

        /// <summary>
        /// Obtiene resultados por tipo de torneo.
        /// </summary>
        /// <param name="type">Tipo de torneo.</param>
        /// <returns>Lista de resultados de torneos del tipo especificado.</returns>
        Task<IEnumerable<Result>> GetByTournamentTypeAsync(TournamentType type);

        /// <summary>
        /// Obtiene resultados por rango de fechas de finalización.
        /// </summary>
        /// <param name="startDate">Fecha de inicio del rango.</param>
        /// <param name="endDate">Fecha de fin del rango.</param>
        /// <returns>Lista de resultados dentro del rango de fechas.</returns>
        Task<IEnumerable<Result>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Obtiene resultados por jugador ganador.
        /// </summary>
        /// <param name="playerId">Identificador del jugador ganador.</param>
        /// <returns>Lista de resultados donde el jugador especificado fue el ganador.</returns>
        Task<IEnumerable<Result>> GetByWinnerIdAsync(Guid playerId);

        /// <summary>
        /// Añade un nuevo resultado.
        /// </summary>
        /// <param name="result">Resultado a añadir.</param>
        /// <returns>Resultado añadido.</returns>
        Task<Result> AddAsync(Result result);

        /// <summary>
        /// Actualiza un resultado existente.
        /// </summary>
        /// <param name="result">Resultado con los datos actualizados.</param>
        /// <returns>Resultado actualizado.</returns>
        Task<Result> UpdateAsync(Result result);

        /// <summary>
        /// Elimina un resultado por su identificador.
        /// </summary>
        /// <param name="id">Identificador del resultado a eliminar.</param>
        /// <returns>True si se eliminó correctamente, false en caso contrario.</returns>
        Task<bool> DeleteAsync(Guid id);
    }
}