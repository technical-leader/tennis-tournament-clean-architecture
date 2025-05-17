using System;
using MediatR;
using TennisTournament.Application.DTOs;

namespace TennisTournament.Application.Queries
{
    /// <summary>
    /// Consulta para obtener un partido por su identificador.
    /// </summary>
    public class GetMatchByIdQuery : IRequest<MatchDto>
    {
        /// <summary>
        /// Identificador del partido.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Constructor con el identificador del partido.
        /// </summary>
        /// <param name="id">Identificador del partido.</param>
        public GetMatchByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}