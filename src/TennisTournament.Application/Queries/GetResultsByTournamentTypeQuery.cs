using System.Collections.Generic;
using MediatR;
using TennisTournament.Application.DTOs;
using TennisTournament.Domain.Enums;

namespace TennisTournament.Application.Queries
{
    /// <summary>
    /// Consulta para obtener resultados por tipo de torneo.
    /// </summary>
    public class GetResultsByTournamentTypeQuery : IRequest<IEnumerable<ResultDto>>
    {
        /// <summary>
        /// Tipo de torneo.
        /// </summary>
        public TournamentType Type { get; }

        /// <summary>
        /// Constructor con el tipo de torneo.
        /// </summary>
        /// <param name="type">Tipo de torneo.</param>
        public GetResultsByTournamentTypeQuery(TournamentType type)
        {
            Type = type;
        }
    }
}