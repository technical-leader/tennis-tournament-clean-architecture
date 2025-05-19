using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TennisTournament.Domain.Entities;

namespace TennisTournament.Domain.Interfaces
{
    /// <summary>
    /// Interfaz para el repositorio de jugadores.
    /// </summary>
    public interface IPlayerRepository
    {
        /// <summary>
        /// Obtiene todos los jugadores.
        /// </summary>
        /// <returns>Lista de jugadores.</returns>
        Task<IEnumerable<Player>> GetAllAsync();

        /// <summary>
        /// Obtiene un jugador por su identificador.
        /// </summary>
        /// <param name="id">Identificador del jugador.</param>
        /// <returns>Jugador encontrado o null si no existe.</returns>
        Task<Player?> GetByIdAsync(Guid id);

        /// <summary>
        /// Añade un nuevo jugador.
        /// </summary>
        /// <param name="player">Jugador a añadir.</param>
        /// <returns>Jugador añadido.</returns>
        Task<Player> AddAsync(Player player);

        /// <summary>
        /// Actualiza un jugador existente.
        /// </summary>
        /// <param name="player">Jugador con los datos actualizados.</param>
        /// <returns>Jugador actualizado.</returns>
        Task<Player> UpdateAsync(Player player);

        /// <summary>
        /// Elimina un jugador por su identificador.
        /// </summary>
        /// <param name="id">Identificador del jugador a eliminar.</param>
        /// <returns>True si se eliminó correctamente, false en caso contrario.</returns>
        Task<bool> DeleteAsync(Guid id);
    }
}