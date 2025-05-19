using System.Collections.Generic;
using MediatR;
using TennisTournament.Application.DTOs;
using TennisTournament.Domain.Enums;

namespace TennisTournament.Application.Queries
{
    /// <summary>
    /// Consulta para obtener torneos por tipo (masculino o femenino).
    /// </summary>
    public class GetTournamentsByTypeQuery : IRequest<IEnumerable<TournamentDto>>
    {
        /// <summary>
        /// Tipo de torneo a filtrar.
        /// </summary>
        public TournamentType Type { get; set; }

        /// <summary>
        /// Constructor con tipo de torneo.
        /// </summary>
        /// <param name="type">Tipo de torneo a filtrar.</param>
        public GetTournamentsByTypeQuery(TournamentType type)
        {
            Type = type;
        }
    }
}