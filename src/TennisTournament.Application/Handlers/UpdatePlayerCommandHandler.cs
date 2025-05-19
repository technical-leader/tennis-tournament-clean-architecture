using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TennisTournament.Application.Commands;
using TennisTournament.Application.DTOs;
using TennisTournament.Domain.Entities;
using TennisTournament.Domain.Enums;
using TennisTournament.Domain.Interfaces;

namespace TennisTournament.Application.Handlers
{
    /// <summary>
    /// Manejador para el comando UpdatePlayerCommand.
    /// </summary>
    public class UpdatePlayerCommandHandler : IRequestHandler<UpdatePlayerCommand, PlayerDto>
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdatePlayerCommand> _validator;

        /// <summary>
        /// Constructor con inyección de dependencias.
        /// </summary>
        /// <param name="playerRepository">Repositorio de jugadores.</param>
        /// <param name="mapper">Mapper para conversión entre entidades y DTOs.</param>
        /// <param name="validator">Validador para el comando.</param>
        public UpdatePlayerCommandHandler(
            IPlayerRepository playerRepository,
            IMapper mapper,
            IValidator<UpdatePlayerCommand> validator)
        {
            _playerRepository = playerRepository ?? throw new ArgumentNullException(nameof(playerRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        /// <summary>
        /// Maneja la actualización de un jugador existente.
        /// </summary>
        /// <param name="request">Comando con los datos del jugador a actualizar.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>DTO del jugador actualizado.</returns>
        public async Task<PlayerDto> Handle(UpdatePlayerCommand request, CancellationToken cancellationToken)
        {
            // Validar el comando
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            // Obtener el jugador existente
            var existingPlayer = await _playerRepository.GetByIdAsync(request.Id);
            if (existingPlayer == null)
                throw new ArgumentException($"No se encontró un jugador con el ID {request.Id}.");

            // Actualizar propiedades comunes
            existingPlayer.Name = request.Name;
            existingPlayer.SkillLevel = request.SkillLevel;

            // Verificar que el tipo de jugador coincida con la entidad
            bool isCorrectType = (existingPlayer is MalePlayer && request.PlayerType == PlayerType.Male) ||
                                (existingPlayer is FemalePlayer && request.PlayerType == PlayerType.Female);

            if (!isCorrectType)
                throw new ArgumentException($"No se puede cambiar el tipo de jugador de {existingPlayer.PlayerType} a {request.PlayerType}.");

            // Actualizar propiedades específicas según el tipo
            if (existingPlayer is MalePlayer malePlayer)
            {
                if (!request.Strength.HasValue || !request.Speed.HasValue)
                    throw new ArgumentException("La fuerza y la velocidad son obligatorias para jugadores masculinos.");

                malePlayer.Strength = request.Strength.Value;
                malePlayer.Speed = request.Speed.Value;
            }
            else if (existingPlayer is FemalePlayer femalePlayer)
            {
                if (!request.ReactionTime.HasValue)
                    throw new ArgumentException("El tiempo de reacción es obligatorio para jugadoras femeninas.");

                femalePlayer.ReactionTime = request.ReactionTime.Value;
            }

            // Guardar los cambios
            var updatedPlayer = await _playerRepository.UpdateAsync(existingPlayer);

            // Mapear el jugador actualizado al DTO correspondiente
            if (updatedPlayer is MalePlayer updatedMalePlayer)
                return _mapper.Map<MalePlayerDto>(updatedMalePlayer);
            else if (updatedPlayer is FemalePlayer updatedFemalePlayer)
                return _mapper.Map<FemalePlayerDto>(updatedFemalePlayer);

            // Caso genérico (no debería ocurrir con la implementación actual)
            return _mapper.Map<PlayerDto>(updatedPlayer);
        }
    }
}