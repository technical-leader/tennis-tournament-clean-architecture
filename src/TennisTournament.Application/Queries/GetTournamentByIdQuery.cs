using System;
using MediatR;
using TennisTournament.Application.DTOs;

namespace TennisTournament.Application.Queries
{
    /// <summary>
    /// Consulta para obtener un torneo por su identificador.
    /// </summary>
    public class GetTournamentByIdQuery : IRequest<TournamentDto?>
    {
        /// <summary>
        /// Identificador del torneo a consultar.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Constructor con el identificador del torneo.
        /// </summary>
        /// <param name="id">Identificador del torneo a consultar.</param>
        public GetTournamentByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}