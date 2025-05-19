using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TennisTournament.Application.DTOs;
using TennisTournament.Domain.Enums;

namespace TennisTournament.Application.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de gestión de torneos.
    /// </summary>
    public interface ITournamentService
    {
        /// <summary>
        /// Obtiene un torneo por su identificador.
        /// </summary>
        /// <param name="id">Identificador del torneo.</param>
        /// <returns>DTO del torneo o null si no existe.</returns>
        Task<TournamentDto?> GetTournamentByIdAsync(Guid id);

        /// <summary>
        /// Obtiene torneos por tipo (masculino/femenino).
        /// </summary>
        /// <param name="type">Tipo de torneo.</param>
        /// <returns>Lista de DTOs de torneos del tipo especificado.</returns>
        Task<IEnumerable<TournamentDto>> GetTournamentsByTypeAsync(TournamentType type);

        /// <summary>
        /// Obtiene torneos por rango de fechas.
        /// </summary>
        /// <param name="startDate">Fecha de inicio del rango.</param>
        /// <param name="endDate">Fecha de fin del rango.</param>
        /// <returns>Lista de DTOs de torneos en el rango de fechas.</returns>
        Task<IEnumerable<TournamentDto>> GetTournamentsByDateRangeAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Crea un nuevo torneo.
        /// </summary>
        /// <param name="type">Tipo de torneo.</param>
        /// <param name="playerIds">Lista de identificadores de jugadores.</param>
        /// <param name="startDate">Fecha de inicio del torneo.</param>
        /// <returns>DTO del torneo creado.</returns>
        Task<TournamentDto> CreateTournamentAsync(TournamentType type, List<Guid> playerIds, DateTime startDate);

        /// <summary>
        /// Simula un torneo completo.
        /// </summary>
        /// <param name="tournamentId">Identificador del torneo a simular.</param>
        /// <returns>DTO con el resultado del torneo.</returns>
        Task<ResultDto> SimulateTournamentAsync(Guid tournamentId);

        /// <summary>
        /// Obtiene el resultado de un torneo.
        /// </summary>
        /// <param name="tournamentId">Identificador del torneo.</param>
        /// <returns>DTO con el resultado del torneo o null si no existe.</returns>
        Task<ResultDto?> GetTournamentResultAsync(Guid tournamentId);
    }
}