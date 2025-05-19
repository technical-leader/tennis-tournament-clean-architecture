using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using TennisTournament.Application.DTOs;
using TennisTournament.Application.Interfaces;
using TennisTournament.Domain.Entities;
using TennisTournament.Domain.Enums;
using TennisTournament.Domain.Interfaces;

namespace TennisTournament.Application.Services
{
    /// <summary>
    /// Implementación del servicio de gestión de jugadores.
    /// </summary>
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor con inyección de dependencias.
        /// </summary>
        /// <param name="playerRepository">Repositorio de jugadores.</param>
        /// <param name="mapper">Mapper para conversión entre entidades y DTOs.</param>
        public PlayerService(IPlayerRepository playerRepository, IMapper mapper)
        {
            _playerRepository = playerRepository ?? throw new ArgumentNullException(nameof(playerRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<PlayerDto>> GetAllPlayersAsync()
        {
            var players = await _playerRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PlayerDto>>(players);
        }

        /// <inheritdoc/>
        public async Task<PlayerDto?> GetPlayerByIdAsync(Guid id)
        {
            var player = await _playerRepository.GetByIdAsync(id);
            return player != null ? _mapper.Map<PlayerDto>(player) : null;
        }

        /// <inheritdoc/>
        public async Task<PlayerDto> CreatePlayerAsync(string playerType, string name, int skillLevel, int? strength, int? speed, int? reactionTime)
        {
            Player player;

            if (playerType.Equals("Male", StringComparison.OrdinalIgnoreCase))
            {
                if (!strength.HasValue || !speed.HasValue)
                    throw new ArgumentException("Los jugadores masculinos requieren valores de fuerza y velocidad.");

                player = new MalePlayer
                {
                    Name = name,
                    SkillLevel = skillLevel,
                    Strength = strength.Value,
                    Speed = speed.Value
                };
            }
            else if (playerType.Equals("Female", StringComparison.OrdinalIgnoreCase))
            {
                if (!reactionTime.HasValue)
                    throw new ArgumentException("Las jugadoras femeninas requieren un valor de tiempo de reacción.");

                player = new FemalePlayer
                {
                    Name = name,
                    SkillLevel = skillLevel,
                    ReactionTime = reactionTime.Value
                };
            }
            else
            {
                throw new ArgumentException($"Tipo de jugador no válido: {playerType}. Debe ser 'Male' o 'Female'.");
            }

            var createdPlayer = await _playerRepository.AddAsync(player);
            return _mapper.Map<PlayerDto>(createdPlayer);
        }

        /// <inheritdoc/>
        public async Task<PlayerDto> CreateMalePlayerAsync(MalePlayerDto playerDto)
        {
            var player = new MalePlayer
            {
                Name = playerDto.Name,
                SkillLevel = playerDto.SkillLevel,
                Strength = playerDto.Strength,
                Speed = playerDto.Speed
            };

            var createdPlayer = await _playerRepository.AddAsync(player);
            return _mapper.Map<MalePlayerDto>(createdPlayer);
        }

        /// <inheritdoc/>
        public async Task<PlayerDto> CreateFemalePlayerAsync(FemalePlayerDto playerDto)
        {
            var player = new FemalePlayer
            {
                Name = playerDto.Name,
                SkillLevel = playerDto.SkillLevel,
                ReactionTime = playerDto.ReactionTime
            };

            var createdPlayer = await _playerRepository.AddAsync(player);
            return _mapper.Map<FemalePlayerDto>(createdPlayer);
        }

        /// <inheritdoc/>
        public async Task<PlayerDto?> UpdatePlayerAsync(Guid id, string name, int skillLevel, int? strength, int? speed, int? reactionTime)
        {
            var existingPlayer = await _playerRepository.GetByIdAsync(id);
            if (existingPlayer == null)
                return null;

            // Actualizar propiedades básicas
            existingPlayer.Name = name;
            existingPlayer.SkillLevel = skillLevel;

            // Actualizar propiedades específicas según el tipo de jugador
            if (existingPlayer is MalePlayer malePlayer)
            {
                if (!strength.HasValue || !speed.HasValue)
                    throw new ArgumentException("Los jugadores masculinos requieren valores de fuerza y velocidad.");

                malePlayer.Strength = strength.Value;
                malePlayer.Speed = speed.Value;
            }
            else if (existingPlayer is FemalePlayer femalePlayer)
            {
                if (!reactionTime.HasValue)
                    throw new ArgumentException("Las jugadoras femeninas requieren un valor de tiempo de reacción.");

                femalePlayer.ReactionTime = reactionTime.Value;
            }

            var updatedPlayer = await _playerRepository.UpdateAsync(existingPlayer);
            return _mapper.Map<PlayerDto>(updatedPlayer);
        }

        /// <inheritdoc/>
        public async Task<PlayerDto?> UpdatePlayerAsync(Guid id, PlayerDto playerDto)
        {
            var existingPlayer = await _playerRepository.GetByIdAsync(id);
            if (existingPlayer == null)
                return null;

            // Actualizar propiedades básicas
            existingPlayer.Name = playerDto.Name;
            existingPlayer.SkillLevel = playerDto.SkillLevel;

            // Actualizar propiedades específicas según el tipo de jugador
            if (existingPlayer is MalePlayer malePlayer && playerDto is MalePlayerDto malePlayerDto)
            {
                malePlayer.Strength = malePlayerDto.Strength;
                malePlayer.Speed = malePlayerDto.Speed;
            }
            else if (existingPlayer is FemalePlayer femalePlayer && playerDto is FemalePlayerDto femalePlayerDto)
            {
                femalePlayer.ReactionTime = femalePlayerDto.ReactionTime;
            }

            var updatedPlayer = await _playerRepository.UpdateAsync(existingPlayer);
            return _mapper.Map<PlayerDto>(updatedPlayer);
        }

        /// <inheritdoc/>
        public async Task<bool> DeletePlayerAsync(Guid id)
        {
            return await _playerRepository.DeleteAsync(id);
        }
    }
}