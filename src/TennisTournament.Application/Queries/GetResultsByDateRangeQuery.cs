using System;
using System.Collections.Generic;
using MediatR;
using TennisTournament.Application.DTOs;

namespace TennisTournament.Application.Queries
{
    /// <summary>
    /// Consulta para obtener resultados por rango de fechas.
    /// </summary>
    public class GetResultsByDateRangeQuery : IRequest<IEnumerable<ResultDto>>
    {
        /// <summary>
        /// Fecha de inicio del rango.
        /// </summary>
        public DateTime StartDate { get; }

        /// <summary>
        /// Fecha de fin del rango.
        /// </summary>
        public DateTime EndDate { get; }

        /// <summary>
        /// Constructor con el rango de fechas.
        /// </summary>
        /// <param name="startDate">Fecha de inicio del rango.</param>
        /// <param name="endDate">Fecha de fin del rango.</param>
        public GetResultsByDateRangeQuery(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}