using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TennisTournament.Application.Commands;
using TennisTournament.Application.DTOs;
using TennisTournament.Domain.Entities;
using TennisTournament.Domain.Enums;
using TennisTournament.Domain.Interfaces;

namespace TennisTournament.Application.Handlers
{
    /// <summary>
    /// Manejador para el comando CreatePlayerCommand.
    /// </summary>
    public class CreatePlayerCommandHandler : IRequestHandler<CreatePlayerCommand, PlayerDto>
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor con inyección de dependencias.
        /// </summary>
        /// <param name="playerRepository">Repositorio de jugadores.</param>
        /// <param name="mapper">Mapper para conversión entre entidades y DTOs.</param>
        public CreatePlayerCommandHandler(IPlayerRepository playerRepository, IMapper mapper)
        {
            _playerRepository = playerRepository ?? throw new ArgumentNullException(nameof(playerRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Maneja el comando para crear un nuevo jugador.
        /// </summary>
        /// <param name="request">Comando con los datos del jugador a crear.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>DTO del jugador creado.</returns>
        public async Task<PlayerDto> Handle(CreatePlayerCommand request, CancellationToken cancellationToken)
        {
            // Validar datos básicos
            if (string.IsNullOrEmpty(request.Name))
                throw new ArgumentException("El nombre del jugador es obligatorio.", nameof(request.Name));

            Player player;

            // Crear el jugador según el tipo
            if (request.PlayerType == PlayerType.Male)
            {
                if (!request.Strength.HasValue || !request.Speed.HasValue)
                    throw new ArgumentException("La fuerza y la velocidad son obligatorias para jugadores masculinos.");

                player = new MalePlayer
                {
                    Name = request.Name,
                    SkillLevel = request.SkillLevel,
                    Strength = request.Strength.Value,
                    Speed = request.Speed.Value
                };
            }
            else if (request.PlayerType == PlayerType.Female)
            {
                if (!request.ReactionTime.HasValue)
                    throw new ArgumentException("El tiempo de reacción es obligatorio para jugadoras femeninas.");

                player = new FemalePlayer
                {
                    Name = request.Name,
                    SkillLevel = request.SkillLevel,
                    ReactionTime = request.ReactionTime.Value
                };
            }
            else
            {
                throw new ArgumentException($"Tipo de jugador no válido: {request.PlayerType}. Debe ser Male o Female.");
            }

            // Guardar el jugador en la base de datos
            var createdPlayer = await _playerRepository.AddAsync(player);

            // Mapear el jugador creado al DTO correspondiente
            if (createdPlayer is MalePlayer malePlayer)
                return _mapper.Map<MalePlayerDto>(malePlayer);
            else if (createdPlayer is FemalePlayer femalePlayer)
                return _mapper.Map<FemalePlayerDto>(femalePlayer);

            // Caso genérico (no debería ocurrir con la implementación actual)
            return _mapper.Map<PlayerDto>(createdPlayer);
        }
    }
}