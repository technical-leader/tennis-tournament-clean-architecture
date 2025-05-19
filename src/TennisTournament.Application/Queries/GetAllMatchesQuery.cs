using System.Collections.Generic;
using MediatR;
using TennisTournament.Application.DTOs;

namespace TennisTournament.Application.Queries
{
    /// <summary>
    /// Consulta para obtener todos los partidos.
    /// </summary>
    public class GetAllMatchesQuery : IRequest<IEnumerable<MatchDto>>
    {
        // No se requieren parámetros para esta consulta
    }
}