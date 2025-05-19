using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TennisTournament.Application.DTOs;
using TennisTournament.Application.Queries;
using TennisTournament.Domain.Enums;

namespace TennisTournament.API.Controllers
{
    /// <summary>
    /// Controlador para la gestión de resultados de torneos.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ResultsController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Constructor con inyección de dependencias.
        /// </summary>
        /// <param name="mediator">Mediador para el patrón CQRS.</param>
        public ResultsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Obtiene todos los resultados de torneos.
        /// </summary>
        /// <returns>Lista de resultados.</returns>
        /// <response code="200">Devuelve la lista de resultados.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ResultDto>>> GetAll()
        {
            var query = new GetAllResultsQuery();
            var results = await _mediator.Send(query);
            return Ok(results);
        }

        /// <summary>
        /// Obtiene un resultado por su identificador.
        /// </summary>
        /// <param name="id">Identificador del resultado.</param>
        /// <returns>Resultado encontrado.</returns>
        /// <response code="200">Devuelve el resultado solicitado.</response>
        /// <response code="404">No se encontró el resultado.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResultDto>> GetById(Guid id)
        {
            try
            {
                var query = new GetResultByIdQuery(id);
                var result = await _mediator.Send(query);

                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene resultados por tipo de torneo.
        /// </summary>
        /// <param name="type">Tipo de torneo.</param>
        /// <returns>Lista de resultados del tipo de torneo especificado.</returns>
        /// <response code="200">Devuelve la lista de resultados.</response>
        [HttpGet("bytype/{type}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ResultDto>>> GetByTournamentType(TournamentType type)
        {
            var query = new GetResultsByTournamentTypeQuery(type);
            var results = await _mediator.Send(query);
            return Ok(results);
        }

        /// <summary>
        /// Obtiene resultados por rango de fechas.
        /// </summary>
        /// <param name="startDate">Fecha de inicio del rango.</param>
        /// <param name="endDate">Fecha de fin del rango.</param>
        /// <returns>Lista de resultados en el rango de fechas.</returns>
        /// <response code="200">Devuelve la lista de resultados.</response>
        [HttpGet("bydate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ResultDto>>> GetByDateRange(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var query = new GetResultsByDateRangeQuery(startDate, endDate);
            var results = await _mediator.Send(query);
            return Ok(results);
        }

        /// <summary>
        /// Obtiene resultados por jugador ganador.
        /// </summary>
        /// <param name="playerId">Identificador del jugador ganador.</param>
        /// <returns>Lista de resultados donde el jugador especificado fue el ganador.</returns>
        /// <response code="200">Devuelve la lista de resultados.</response>
        [HttpGet("bywinner/{playerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ResultDto>>> GetByWinnerId(Guid playerId)
        {
            var query = new GetResultsByWinnerIdQuery(playerId);
            var results = await _mediator.Send(query);
            return Ok(results);
        }
    }
}
