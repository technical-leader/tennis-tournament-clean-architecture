using System;
using System.Collections.Generic;
using TennisTournament.Domain.Enums;

namespace TennisTournament.Application.DTOs
{
    /// <summary>
    /// DTO para transferencia de datos de resultados de torneos.
    /// </summary>
    public class ResultDto
    {
        /// <summary>
        /// Identificador único del resultado.
        /// </summary>
        /// <remarks>
        /// Corresponde a la propiedad Id en la entidad Result.
        /// </remarks>
        public Guid Id { get; set; }

        /// <summary>
        /// Identificador del torneo al que pertenece este resultado.
        /// </summary>
        /// <remarks>
        /// Corresponde a la propiedad TournamentId en la entidad Result.
        /// </remarks>
        public Guid TournamentId { get; set; }

        /// <summary>
        /// Tipo de torneo (Male o Female).
        /// </summary>
        /// <remarks>
        /// Corresponde a la propiedad Type de la entidad Tournament referenciada por TournamentId.
        /// </remarks>
        public TournamentType TournamentType { get; set; }

        /// <summary>
        /// Identificador del jugador ganador del torneo.
        /// </summary>
        /// <remarks>
        /// Corresponde a la propiedad WinnerId en la entidad Result.
        /// </remarks>
        public Guid WinnerId { get; set; }

        /// <summary>
        /// Nombre del jugador ganador del torneo para mostrar.
        /// </summary>
        /// <remarks>
        /// Propiedad calculada basada en la propiedad Name del jugador referenciado por WinnerId.
        /// </remarks>
        public required string WinnerDisplay { get; set; }

        /// <summary>
        /// Fecha de finalización del torneo.
        /// </summary>
        /// <remarks>
        /// Corresponde a la propiedad CompletionDate en la entidad Result.
        /// </remarks>
        public DateTime CompletionDate { get; set; }

        /// <summary>
        /// Lista de partidos jugados en el torneo.
        /// </summary>
        /// <remarks>
        /// Corresponde a la colección Matches en la entidad Result.
        /// </remarks>
        public List<MatchDto> Matches { get; set; } = new List<MatchDto>();
    }
}