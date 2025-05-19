using System;
using MediatR;

namespace TennisTournament.Application.Commands
{
    /// <summary>
    /// Comando para eliminar un jugador existente.
    /// </summary>
    public class DeletePlayerCommand : IRequest<bool>
    {
        /// <summary>
        /// Identificador del jugador a eliminar.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Constructor con el identificador del jugador.
        /// </summary>
        /// <param name="id">Identificador del jugador a eliminar.</param>
        public DeletePlayerCommand(Guid id)
        {
            Id = id;
        }
    }
}