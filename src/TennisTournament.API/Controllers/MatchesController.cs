using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TennisTournament.Application.DTOs;
using TennisTournament.Application.Queries;

namespace TennisTournament.API.Controllers
{
    /// <summary>
    /// Controlador para la gestión de partidos de tenis.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class MatchesController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Constructor con inyección de dependencias.
        /// </summary>
        /// <param name="mediator">Mediador para el patrón CQRS.</param>
        public MatchesController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Obtiene todos los partidos.
        /// </summary>
        /// <returns>Lista de partidos.</returns>
        /// <response code="200">Devuelve la lista de partidos.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<MatchDto>>> GetAll()
        {
            var query = new GetAllMatchesQuery();
            var matches = await _mediator.Send(query);
            return Ok(matches);
        }

        /// <summary>
        /// Obtiene un partido por su identificador.
        /// </summary>
        /// <param name="id">Identificador del partido.</param>
        /// <returns>Partido encontrado.</returns>
        /// <response code="200">Devuelve el partido solicitado.</response>
        /// <response code="404">No se encontró el partido.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MatchDto>> GetById(Guid id)
        {
            try
            {
                var query = new GetMatchByIdQuery(id);
                var match = await _mediator.Send(query);

                if (match == null)
                    return NotFound();

                return Ok(match);
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
        /// Obtiene partidos por torneo.
        /// </summary>
        /// <param name="tournamentId">Identificador del torneo.</param>
        /// <returns>Lista de partidos del torneo.</returns>
        /// <response code="200">Devuelve la lista de partidos.</response>
        [HttpGet("bytournament/{tournamentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<MatchDto>>> GetByTournamentId(Guid tournamentId)
        {
            var query = new GetMatchesByTournamentIdQuery(tournamentId);
            var matches = await _mediator.Send(query);
            return Ok(matches);
        }

        /// <summary>
        /// Obtiene partidos por jugador.
        /// </summary>
        /// <param name="playerId">Identificador del jugador.</param>
        /// <returns>Lista de partidos en los que participó el jugador.</returns>
        /// <response code="200">Devuelve la lista de partidos.</response>
        [HttpGet("byplayer/{playerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<MatchDto>>> GetByPlayerId(Guid playerId)
        {
            var query = new GetMatchesByPlayerIdQuery(playerId);
            var matches = await _mediator.Send(query);
            return Ok(matches);
        }
    }
}
