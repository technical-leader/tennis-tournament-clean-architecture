using System;
using MediatR;
using TennisTournament.Application.DTOs;

namespace TennisTournament.Application.Queries
{
    /// <summary>
    /// Consulta para obtener el resultado de un torneo específico.
    /// </summary>
    public class GetTournamentResultQuery : IRequest<ResultDto?>
    {
        /// <summary>
        /// Identificador del torneo.
        /// </summary>
        public Guid TournamentId { get; set; }

        /// <summary>
        /// Constructor con el identificador del torneo.
        /// </summary>
        /// <param name="tournamentId">Identificador del torneo.</param>
        public GetTournamentResultQuery(Guid tournamentId)
        {
            TournamentId = tournamentId;
        }
    }
}