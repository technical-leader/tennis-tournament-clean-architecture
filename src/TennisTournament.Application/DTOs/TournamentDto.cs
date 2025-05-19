using System;
using System.Collections.Generic;
using TennisTournament.Domain.Enums;

namespace TennisTournament.Application.DTOs
{
    /// <summary>
    /// DTO para transferencia de datos de torneos.
    /// </summary>
    public class TournamentDto
    {
        /// <summary>
        /// Identificador único del torneo.
        /// </summary>
        /// <remarks>
        /// Corresponde a la propiedad Id en la entidad Tournament.
        /// </remarks>
        public Guid Id { get; set; }

        /// <summary>
        /// Tipo de torneo (Masculino o Femenino).
        /// </summary>
        /// <remarks>
        /// Corresponde a la propiedad Type en la entidad Tournament.
        /// </remarks>
        public TournamentType Type { get; set; }

        /// <summary>
        /// Representación textual del tipo de torneo para mostrar.
        /// </summary>
        /// <remarks>
        /// Propiedad calculada basada en la propiedad Type.
        /// </remarks>
        public string TypeDisplay => Type.ToString();

        /// <summary>
        /// Lista de identificadores de jugadores participantes en el torneo.
        /// </summary>
        /// <remarks>
        /// Corresponde a los Ids de los jugadores en la colección Players de la entidad Tournament.
        /// </remarks>
        public List<Guid> PlayerIds { get; set; } = new List<Guid>();

        /// <summary>
        /// Lista de nombres de jugadores participantes en el torneo para mostrar.
        /// </summary>
        /// <remarks>
        /// Propiedad calculada basada en los nombres de los jugadores en la colección Players de la entidad Tournament.
        /// </remarks>
        public List<string> PlayerNames { get; set; } = new List<string>();

        /// <summary>
        /// Fecha de inicio del torneo.
        /// </summary>
        /// <remarks>
        /// Corresponde a la propiedad StartDate en la entidad Tournament.
        /// </remarks>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Fecha de finalización del torneo.
        /// </summary>
        /// <remarks>
        /// Corresponde a la propiedad EndDate en la entidad Tournament.
        /// </remarks>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Estado actual del torneo.
        /// </summary>
        /// <remarks>
        /// Corresponde a la propiedad Status en la entidad Tournament.
        /// </remarks>
        public TournamentStatus Status { get; set; }

        /// <summary>
        /// Representación textual del estado actual del torneo para mostrar.
        /// </summary>
        /// <remarks>
        /// Propiedad calculada basada en la propiedad Status.
        /// </remarks>
        public string StatusDisplay => Status.ToString();

        /// <summary>
        /// Lista de partidos del torneo.
        /// </summary>
        /// <remarks>
        /// Corresponde a la colección Matches en la entidad Tournament.
        /// </remarks>
        public List<MatchDto> Matches { get; set; } = new List<MatchDto>();
    }
}