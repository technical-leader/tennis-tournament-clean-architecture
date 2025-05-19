using System;
using MediatR;
using TennisTournament.Application.DTOs;

namespace TennisTournament.Application.Commands
{
    /// <summary>
    /// Comando para simular un torneo existente.
    /// </summary>
    public class SimulateTournamentCommand : IRequest<ResultDto>
    {
        /// <summary>
        /// Identificador del torneo a simular.
        /// </summary>
        public Guid TournamentId { get; set; }

        /// <summary>
        /// Constructor con el identificador del torneo.
        /// </summary>
        /// <param name="tournamentId">Identificador del torneo a simular.</param>
        public SimulateTournamentCommand(Guid tournamentId)
        {
            TournamentId = tournamentId;
        }
    }
}