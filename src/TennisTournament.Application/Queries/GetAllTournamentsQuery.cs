using System.Collections.Generic;
using MediatR;
using TennisTournament.Application.DTOs;
using TennisTournament.Domain.Enums;

namespace TennisTournament.Application.Queries
{
    /// <summary>
    /// Consulta para obtener todos los torneos con filtros opcionales.
    /// </summary>
    public class GetAllTournamentsQuery : IRequest<IEnumerable<TournamentShortDto>>
    {
        /// <summary>
        /// Filtro opcional por tipo de torneo.
        /// </summary>
        public TournamentType? Type { get; set; }

        /// <summary>
        /// Filtro opcional por estado del torneo.
        /// </summary>
        public TournamentStatus? Status { get; set; }
    }
}