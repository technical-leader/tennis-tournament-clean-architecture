using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TennisTournament.Application.DTOs;

namespace TennisTournament.Application.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de gestión de jugadores.
    /// </summary>
    public interface IPlayerService
    {
        /// <summary>
        /// Obtiene todos los jugadores.
        /// </summary>
        /// <returns>Lista de DTOs de jugadores.</returns>
        Task<IEnumerable<PlayerDto>> GetAllPlayersAsync();

        /// <summary>
        /// Obtiene un jugador por su identificador.
        /// </summary>
        /// <param name="id">Identificador del jugador.</param>
        /// <returns>DTO del jugador o null si no existe.</returns>
        Task<PlayerDto?> GetPlayerByIdAsync(Guid id);

        /// <summary>
        /// Crea un nuevo jugador.
        /// </summary>
        /// <param name="playerType">Tipo de jugador (Male/Female).</param>
        /// <param name="name">Nombre del jugador.</param>
        /// <param name="skillLevel">Nivel de habilidad.</param>
        /// <param name="strength">Fuerza (solo para jugadores masculinos).</param>
        /// <param name="speed">Velocidad (solo para jugadores masculinos).</param>
        /// <param name="reactionTime">Tiempo de reacción (solo para jugadoras femeninas).</param>
        /// <returns>DTO del jugador creado.</returns>
        Task<PlayerDto> CreatePlayerAsync(string playerType, string name, int skillLevel, int? strength, int? speed, int? reactionTime);

        /// <summary>
        /// Crea un nuevo jugador masculino.
        /// </summary>
        /// <param name="playerDto">DTO con los datos del jugador.</param>
        /// <returns>DTO del jugador creado.</returns>
        Task<PlayerDto> CreateMalePlayerAsync(MalePlayerDto playerDto);

        /// <summary>
        /// Crea una nueva jugadora femenina.
        /// </summary>
        /// <param name="playerDto">DTO con los datos de la jugadora.</param>
        /// <returns>DTO de la jugadora creada.</returns>
        Task<PlayerDto> CreateFemalePlayerAsync(FemalePlayerDto playerDto);

        /// <summary>
        /// Actualiza un jugador existente.
        /// </summary>
        /// <param name="id">Identificador del jugador.</param>
        /// <param name="name">Nombre del jugador.</param>
        /// <param name="skillLevel">Nivel de habilidad.</param>
        /// <param name="strength">Fuerza (solo para jugadores masculinos).</param>
        /// <param name="speed">Velocidad (solo para jugadores masculinos).</param>
        /// <param name="reactionTime">Tiempo de reacción (solo para jugadoras femeninas).</param>
        /// <returns>DTO del jugador actualizado o null si no existe.</returns>
        Task<PlayerDto?> UpdatePlayerAsync(Guid id, string name, int skillLevel, int? strength, int? speed, int? reactionTime);

        /// <summary>
        /// Actualiza un jugador existente.
        /// </summary>
        /// <param name="id">Identificador del jugador.</param>
        /// <param name="playerDto">DTO con los datos actualizados.</param>
        /// <returns>DTO del jugador actualizado o null si no existe.</returns>
        Task<PlayerDto?> UpdatePlayerAsync(Guid id, PlayerDto playerDto);

        /// <summary>
        /// Elimina un jugador.
        /// </summary>
        /// <param name="id">Identificador del jugador a eliminar.</param>
        /// <returns>True si el jugador fue eliminado, False si no existía.</returns>
        Task<bool> DeletePlayerAsync(Guid id);
    }
}
