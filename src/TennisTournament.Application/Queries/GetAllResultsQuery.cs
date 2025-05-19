using System.Collections.Generic;
using MediatR;
using TennisTournament.Application.DTOs;

namespace TennisTournament.Application.Queries
{
    /// <summary>
    /// Consulta para obtener todos los resultados de torneos.
    /// </summary>
    public class GetAllResultsQuery : IRequest<IEnumerable<ResultDto>>
    {
        // No se requieren parámetros para esta consulta
    }
}