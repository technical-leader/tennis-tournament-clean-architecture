using System;
using MediatR;
using TennisTournament.Application.DTOs;

namespace TennisTournament.Application.Queries
{
    /// <summary>
    /// Consulta para obtener un resultado por su identificador.
    /// </summary>
    public class GetResultByIdQuery : IRequest<ResultDto>
    {
        /// <summary>
        /// Identificador del resultado.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Constructor con el identificador del resultado.
        /// </summary>
        /// <param name="id">Identificador del resultado.</param>
        public GetResultByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}