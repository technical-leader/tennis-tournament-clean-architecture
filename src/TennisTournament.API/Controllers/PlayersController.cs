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
    /// Controlador para la gestión de jugadores.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PlayersController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Constructor con inyección de dependencias.
        /// </summary>
        /// <param name="mediator">Mediador para el patrón CQRS.</param>
        public PlayersController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Obtiene todos los jugadores.
        /// </summary>
        /// <param name="playerType">Tipo de jugador (opcional).</param>
        /// <returns>Lista de jugadores.</returns>
        /// <response code="200">Devuelve la lista de jugadores.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PlayerDto>>> GetAll([FromQuery] string? playerType = null)
        {
            var query = new GetAllPlayersQuery
            {
                PlayerType = playerType
            };

            var players = await _mediator.Send(query);
            return Ok(players);
        }

        /// <summary>
        /// Obtiene un jugador por su identificador.
        /// </summary>
        /// <param name="id">Identificador del jugador.</param>
        /// <returns>Jugador encontrado.</returns>
        /// <response code="200">Devuelve el jugador solicitado.</response>
        /// <response code="404">No se encontró el jugador.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PlayerDto>> GetById(Guid id)
        {
            try
            {
                var query = new GetPlayerByIdQuery(id);
                var player = await _mediator.Send(query);

                if (player == null)
                    return NotFound();

                return Ok(player);
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
        /// Crea un nuevo jugador masculino.
        /// </summary>
        /// <param name="playerDto">Datos del jugador.</param>
        /// <returns>Jugador creado.</returns>
        /// <response code="201">Devuelve el jugador creado.</response>
        /// <response code="400">Datos de jugador inválidos.</response>
        [HttpPost("male")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PlayerDto>> CreateMalePlayer([FromBody] MalePlayerDto playerDto)
        {
            try
            {
                var command = new CreatePlayerCommand
                {
                    PlayerType = PlayerType.Male,
                    Name = playerDto.Name,
                    SkillLevel = playerDto.SkillLevel,
                    Strength = playerDto.Strength,
                    Speed = playerDto.Speed
                };

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
        /// Crea una nueva jugadora femenina.
        /// </summary>
        /// <param name="playerDto">Datos de la jugadora.</param>
        /// <returns>Jugadora creada.</returns>
        /// <response code="201">Devuelve la jugadora creada.</response>
        /// <response code="400">Datos de jugadora inválidos.</response>
        [HttpPost("female")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PlayerDto>> CreateFemalePlayer([FromBody] FemalePlayerDto playerDto)
        {
            try
            {
                var command = new CreatePlayerCommand
                {
                    PlayerType = PlayerType.Female,
                    Name = playerDto.Name,
                    SkillLevel = playerDto.SkillLevel,
                    ReactionTime = playerDto.ReactionTime
                };

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
        /// Actualiza un jugador masculino existente.
        /// </summary>
        /// <param name="id">Identificador del jugador.</param>
        /// <param name="playerDto">Datos actualizados del jugador masculino.</param>
        /// <returns>Jugador actualizado.</returns>
        /// <response code="200">Devuelve el jugador actualizado.</response>
        /// <response code="400">Datos de jugador inválidos.</response>
        /// <response code="404">No se encontró el jugador.</response>
        [HttpPut("male/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<PlayerDto>> UpdateMalePlayer(Guid id, [FromBody] MalePlayerDto playerDto)
        {
            try
            {
                // Validación explícita para asegurar que el ID coincida
                if (id != playerDto.Id)
                    return BadRequest("El ID de la ruta no coincide con el ID del jugador.");

                // Validación explícita para asegurar que el tipo de jugador sea masculino
                if (playerDto.PlayerType != PlayerType.Male)
                    return BadRequest("El tipo de jugador debe ser masculino para este endpoint.");

                // Validación de rangos de valores
                if (playerDto.SkillLevel < 0 || playerDto.SkillLevel > 100)
                    return BadRequest("El nivel de habilidad debe estar entre 0 y 100.");

                if (playerDto.Strength < 0 || playerDto.Strength > 100)
                    return BadRequest("La fuerza debe estar entre 0 y 100.");

                if (playerDto.Speed < 0 || playerDto.Speed > 100)
                    return BadRequest("La velocidad debe estar entre 0 y 100.");

                var command = new UpdatePlayerCommand
                {
                    Id = id,
                    PlayerType = PlayerType.Male,
                    Name = playerDto.Name,
                    SkillLevel = playerDto.SkillLevel,
                    Strength = playerDto.Strength,
                    Speed = playerDto.Speed
                };

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
        /// Actualiza una jugadora femenina existente.
        /// </summary>
        /// <param name="id">Identificador de la jugadora.</param>
        /// <param name="playerDto">Datos actualizados de la jugadora femenina.</param>
        /// <returns>Jugadora actualizada.</returns>
        /// <response code="200">Devuelve la jugadora actualizada.</response>
        /// <response code="400">Datos de jugadora inválidos.</response>
        /// <response code="404">No se encontró la jugadora.</response>
        [HttpPut("female/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<PlayerDto>> UpdateFemalePlayer(Guid id, [FromBody] FemalePlayerDto playerDto)
        {
            try
            {
                // Validación explícita para asegurar que el ID coincida
                if (id != playerDto.Id)
                    return BadRequest("El ID de la ruta no coincide con el ID de la jugadora.");

                // Validación explícita para asegurar que el tipo de jugador sea femenino
                if (playerDto.PlayerType != PlayerType.Female)
                    return BadRequest("El tipo de jugador debe ser femenino para este endpoint.");

                // Validación de rangos de valores
                if (playerDto.SkillLevel < 0 || playerDto.SkillLevel > 100)
                    return BadRequest("El nivel de habilidad debe estar entre 0 y 100.");

                if (playerDto.ReactionTime < 0 || playerDto.ReactionTime > 100)
                    return BadRequest("El tiempo de reacción debe estar entre 0 y 100.");

                var command = new UpdatePlayerCommand
                {
                    Id = id,
                    PlayerType = PlayerType.Female,
                    Name = playerDto.Name,
                    SkillLevel = playerDto.SkillLevel,
                    ReactionTime = playerDto.ReactionTime
                };

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
        /// Elimina un jugador.
        /// </summary>
        /// <param name="id">Identificador del jugador.</param>
        /// <returns>Sin contenido.</returns>
        /// <response code="204">Jugador eliminado correctamente.</response>
        /// <response code="404">No se encontró el jugador.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var command = new DeletePlayerCommand(id);
                var result = await _mediator.Send(command);

                if (!result)
                    return NotFound();

                return NoContent();
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
    }
}