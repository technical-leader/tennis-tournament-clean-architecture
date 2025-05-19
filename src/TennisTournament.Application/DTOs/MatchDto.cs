using System;

namespace TennisTournament.Application.DTOs
{
    /// <summary>
    /// DTO para transferencia de datos de partidos.
    /// </summary>
    public class MatchDto
    {
        /// <summary>
        /// Identificador único del partido.
        /// </summary>
        /// <remarks>
        /// Corresponde a la propiedad Id en la entidad Match.
        /// </remarks>
        public Guid Id { get; set; }

        /// <summary>
        /// Identificador del primer jugador.
        /// </summary>
        /// <remarks>
        /// Corresponde a la propiedad Player1Id en la entidad Match.
        /// </remarks>
        public Guid Player1Id { get; set; }

        /// <summary>
        /// Identificador del segundo jugador.
        /// </summary>
        /// <remarks>
        /// Corresponde a la propiedad Player2Id en la entidad Match.
        /// </remarks>
        public Guid Player2Id { get; set; }

        /// <summary>
        /// Identificador del jugador ganador (puede ser nulo si el partido no se ha jugado).
        /// </summary>
        /// <remarks>
        /// Corresponde a la propiedad WinnerId en la entidad Match.
        /// </remarks>
        public Guid? WinnerId { get; set; }

        /// <summary>
        /// Nombre del primer jugador para mostrar.
        /// </summary>
        /// <remarks>
        /// Propiedad calculada basada en la propiedad Name del jugador referenciado por Player1Id.
        /// </remarks>
        public required string Player1Display { get; set; }

        /// <summary>
        /// Nombre del segundo jugador para mostrar.
        /// </summary>
        /// <remarks>
        /// Propiedad calculada basada en la propiedad Name del jugador referenciado por Player2Id.
        /// </remarks>
        public required string Player2Display { get; set; }

        /// <summary>
        /// Nombre del jugador ganador para mostrar.
        /// </summary>
        /// <remarks>
        /// Propiedad calculada basada en la propiedad Name del jugador referenciado por WinnerId.
        /// </remarks>
        public string? WinnerDisplay { get; set; }

        /// <summary>
        /// Ronda del torneo en la que se juega este partido.
        /// </summary>
        /// <remarks>
        /// Corresponde a la propiedad Round en la entidad Match.
        /// </remarks>
        public int Round { get; set; }

        /// <summary>
        /// Fecha y hora en la que se juega el partido.
        /// </summary>
        /// <remarks>
        /// Corresponde a la propiedad MatchDate en la entidad Match.
        /// </remarks>
        public DateTime MatchDate { get; set; }
    }
}