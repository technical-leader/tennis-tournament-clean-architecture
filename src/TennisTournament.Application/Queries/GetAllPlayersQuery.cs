using System.Collections.Generic;
using MediatR;
using TennisTournament.Application.DTOs;

namespace TennisTournament.Application.Queries
{
    /// <summary>
    /// Consulta para obtener todos los jugadores con filtro opcional por tipo.
    /// </summary>
    public class GetAllPlayersQuery : IRequest<IEnumerable<PlayerDto>>
    {
        /// <summary>
        /// Filtro opcional por tipo de jugador (Male o Female).
        /// </summary>
        public string? PlayerType { get; set; }
    }
}