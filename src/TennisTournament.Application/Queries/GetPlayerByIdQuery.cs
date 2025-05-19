using System;
using MediatR;
using TennisTournament.Application.DTOs;

namespace TennisTournament.Application.Queries
{
    /// <summary>
    /// Consulta para obtener un jugador por su identificador.
    /// </summary>
    public class GetPlayerByIdQuery : IRequest<PlayerDto?>
    {
        /// <summary>
        /// Identificador del jugador a consultar.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Constructor con el identificador del jugador.
        /// </summary>
        /// <param name="id">Identificador del jugador a consultar.</param>
        public GetPlayerByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}