using System;
using System.Collections.Generic;
using MediatR;
using TennisTournament.Application.DTOs;

namespace TennisTournament.Application.Queries
{
    /// <summary>
    /// Consulta para obtener partidos por torneo.
    /// </summary>
    public class GetMatchesByTournamentIdQuery : IRequest<IEnumerable<MatchDto>>
    {
        /// <summary>
        /// Identificador del torneo.
        /// </summary>
        public Guid TournamentId { get; }

        /// <summary>
        /// Constructor con el identificador del torneo.
        /// </summary>
        /// <param name="tournamentId">Identificador del torneo.</param>
        public GetMatchesByTournamentIdQuery(Guid tournamentId)
        {
            TournamentId = tournamentId;
        }
    }
}