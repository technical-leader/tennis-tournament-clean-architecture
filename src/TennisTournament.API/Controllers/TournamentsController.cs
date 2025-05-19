using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TennisTournament.Application.Commands;
using TennisTournament.Application.DTOs;
using TennisTournament.Application.Queries;
using TennisTournament.Domain.Enums;

namespace TennisTournament.API.Controllers
{
    /// <summary>
    /// Controlador para la gestión de torneos de tenis.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TournamentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Constructor con inyección de dependencias.
        /// </summary>
        /// <param name="mediator">Mediador para el patrón CQRS.</param>
        public TournamentsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Obtiene todos los torneos.
        /// </summary>
        /// <param name="type">Tipo de torneo (opcional).</param>
        /// <param name="status">Estado del torneo (opcional).</param>
        /// <returns>Lista de torneos.</returns>
        /// <response code="200">Devuelve la lista de torneos.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TournamentDto>>> GetAll(
            [FromQuery] TournamentType? type = null,
            [FromQuery] TournamentStatus? status = null)
        {
            var query = new GetAllTournamentsQuery
            {
                Type = type,
                Status = status
            };

            var tournaments = await _mediator.Send(query);
            return Ok(tournaments);
        }

        /// <summary>
        /// Obtiene un torneo por su identificador.
        /// </summary>
        /// <param name="id">Identificador del torneo.</param>
        /// <returns>Torneo encontrado.</returns>
        /// <response code="200">Devuelve el torneo solicitado.</response>
        /// <response code="404">No se encontró el torneo.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TournamentDto>> GetById(Guid id)
        {
            try
            {
                var query = new GetTournamentByIdQuery(id);
                var tournament = await _mediator.Send(query);

                if (tournament == null)
                    return NotFound();

                return Ok(tournament);
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
        /// Obtiene torneos por rango de fechas.
        /// </summary>
        /// <param name="startDate">Fecha de inicio del rango.</param>
        /// <param name="endDate">Fecha de fin del rango.</param>
        /// <returns>Lista de torneos en el rango de fechas.</returns>
        /// <response code="200">Devuelve la lista de torneos.</response>
        [HttpGet("bydate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TournamentDto>>> GetByDateRange(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            try
            {
                var query = new GetTournamentsByDateQuery(startDate, endDate);
                var tournaments = await _mediator.Send(query);
                return Ok(tournaments);
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
        /// Crea un nuevo torneo.
        /// </summary>
        /// <param name="command">Datos del torneo a crear.</param>
        /// <returns>Torneo creado.</returns>
        /// <response code="201">Devuelve el torneo creado.</response>
        /// <response code="400">Datos de torneo inválidos.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TournamentDto>> Create([FromBody] CreateTournamentCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
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
        /// Simula un torneo completo.
        /// </summary>
        /// <param name="id">Identificador del torneo.</param>
        /// <returns>Resultado del torneo.</returns>
        /// <response code="200">Devuelve el resultado del torneo.</response>
        /// <response code="400">El torneo no puede ser simulado.</response>
        /// <response code="404">No se encontró el torneo.</response>
        [HttpPost("{id}/simulate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResultDto>> SimulateTournament(Guid id)
        {
            try
            {
                var command = new SimulateTournamentCommand(id);
                var result = await _mediator.Send(command);

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
        /// Obtiene el resultado de un torneo.
        /// </summary>
        /// <param name="id">Identificador del torneo.</param>
        /// <returns>Resultado del torneo.</returns>
        /// <response code="200">Devuelve el resultado del torneo.</response>
        /// <response code="404">No se encontró el resultado del torneo.</response>
        [HttpGet("{id}/result")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResultDto>> GetResult(Guid id)
        {
            var query = new GetTournamentResultQuery(id);
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
