using System;
using MediatR;
using TennisTournament.Application.DTOs;
using TennisTournament.Domain.Enums;

namespace TennisTournament.Application.Commands
{
    /// <summary>
    /// Comando para actualizar un jugador existente.
    /// </summary>
    public class UpdatePlayerCommand : IRequest<PlayerDto>
    {
        /// <summary>
        /// Identificador del jugador a actualizar.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Tipo de jugador (Male o Female).
        /// </summary>
        public PlayerType PlayerType { get; set; }

        /// <summary>
        /// Nombre del jugador.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Nivel de habilidad del jugador (entre 0 y 100).
        /// </summary>
        public int SkillLevel { get; set; }

        /// <summary>
        /// Fuerza del jugador (solo para jugadores masculinos).
        /// </summary>
        public int? Strength { get; set; }

        /// <summary>
        /// Velocidad del jugador (solo para jugadores masculinos).
        /// </summary>
        public int? Speed { get; set; }

        /// <summary>
        /// Tiempo de reacción de la jugadora (solo para jugadoras femeninas).
        /// </summary>
        public int? ReactionTime { get; set; }
    }
}