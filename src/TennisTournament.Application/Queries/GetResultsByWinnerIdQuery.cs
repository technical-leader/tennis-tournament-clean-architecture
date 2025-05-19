using System;
using System.Collections.Generic;
using MediatR;
using TennisTournament.Application.DTOs;

namespace TennisTournament.Application.Queries
{
    /// <summary>
    /// Consulta para obtener resultados por jugador ganador.
    /// </summary>
    public class GetResultsByWinnerIdQuery : IRequest<IEnumerable<ResultDto>>
    {
        /// <summary>
        /// Identificador del jugador ganador.
        /// </summary>
        public Guid PlayerId { get; }

        /// <summary>
        /// Constructor con el identificador del jugador ganador.
        /// </summary>
        /// <param name="playerId">Identificador del jugador ganador.</param>
        public GetResultsByWinnerIdQuery(Guid playerId)
        {
            PlayerId = playerId;
        }
    }
}