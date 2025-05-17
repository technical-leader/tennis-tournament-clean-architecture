using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TennisTournament.Domain.Entities;
using TennisTournament.Domain.Enums;

namespace TennisTournament.Domain.Interfaces
{
    /// <summary>
    /// Interfaz para el repositorio de torneos.
    /// </summary>
    public interface ITournamentRepository
    {
        /// <summary>
        /// Obtiene todos los torneos.
        /// </summary>
        /// <returns>Lista de torneos.</returns>
        Task<IEnumerable<Tournament>> GetAllAsync();

        /// <summary>
        /// Obtiene un torneo por su identificador.
        /// </summary>
        /// <param name="id">Identificador del torneo.</param>
        /// <returns>Torneo encontrado o null si no existe.</returns>
        Task<Tournament?> GetByIdAsync(Guid id);

        /// <summary>
        /// Obtiene torneos por tipo (masculino o femenino).
        /// </summary>
        /// <param name="type">Tipo de torneo.</param>
        /// <returns>Lista de torneos del tipo especificado.</returns>
        Task<IEnumerable<Tournament>> GetByTypeAsync(TournamentType type);

        /// <summary>
        /// Obtiene torneos por rango de fechas.
        /// </summary>
        /// <param name="startDate">Fecha de inicio del rango.</param>
        /// <param name="endDate">Fecha de fin del rango.</param>
        /// <returns>Lista de torneos dentro del rango de fechas.</returns>
        Task<IEnumerable<Tournament>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Obtiene torneos por estado.
        /// </summary>
        /// <param name="status">Estado del torneo.</param>
        /// <returns>Lista de torneos con el estado especificado.</returns>
        Task<IEnumerable<Tournament>> GetByStatusAsync(TournamentStatus status);

        /// <summary>
        /// Añade un nuevo torneo.
        /// </summary>
        /// <param name="tournament">Torneo a añadir.</param>
        /// <returns>Torneo añadido.</returns>
        Task<Tournament> AddAsync(Tournament tournament);

        /// <summary>
        /// Actualiza un torneo existente.
        /// </summary>
        /// <param name="tournament">Torneo con los datos actualizados.</param>
        /// <returns>Torneo actualizado.</returns>
        Task<Tournament> UpdateAsync(Tournament tournament);

        /// <summary>
        /// Elimina un torneo por su identificador.
        /// </summary>
        /// <param name="id">Identificador del torneo a eliminar.</param>
        /// <returns>True si se eliminó correctamente, false en caso contrario.</returns>
        Task<bool> DeleteAsync(Guid id);
    }
}