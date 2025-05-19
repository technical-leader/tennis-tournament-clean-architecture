using System;
using System.Collections.Generic;
using MediatR;
using TennisTournament.Application.DTOs;

namespace TennisTournament.Application.Queries
{
    /// <summary>
    /// Consulta para obtener torneos por rango de fechas.
    /// </summary>
    public class GetTournamentsByDateQuery : IRequest<IEnumerable<TournamentDto>>
    {
        /// <summary>
        /// Fecha de inicio del rango.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Fecha de fin del rango.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Constructor con rango de fechas.
        /// </summary>
        /// <param name="startDate">Fecha de inicio del rango.</param>
        /// <param name="endDate">Fecha de fin del rango.</param>
        public GetTournamentsByDateQuery(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}