using System;
using System.Collections.Generic;
using MediatR;
using TennisTournament.Application.DTOs;

namespace TennisTournament.Application.Queries
{
    /// <summary>
    /// Consulta para obtener partidos por jugador.
    /// </summary>
    public class GetMatchesByPlayerIdQuery : IRequest<IEnumerable<MatchDto>>
    {
        /// <summary>
        /// Identificador del jugador.
        /// </summary>
        public Guid PlayerId { get; }

        /// <summary>
        /// Constructor con el identificador del jugador.
        /// </summary>
        /// <param name="playerId">Identificador del jugador.</param>
        public GetMatchesByPlayerIdQuery(Guid playerId)
        {
            PlayerId = playerId;
        }
    }
}