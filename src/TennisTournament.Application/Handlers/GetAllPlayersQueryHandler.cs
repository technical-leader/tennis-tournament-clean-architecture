using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TennisTournament.Application.DTOs;
using TennisTournament.Application.Queries;
using TennisTournament.Domain.Entities;
using TennisTournament.Domain.Interfaces;

namespace TennisTournament.Application.Handlers
{
    /// <summary>
    /// Manejador para la consulta de obtención de todos los jugadores.
    /// </summary>
    public class GetAllPlayersQueryHandler : IRequestHandler<GetAllPlayersQuery, IEnumerable<PlayerDto>>
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor con inyección de dependencias.
        /// </summary>
        /// <param name="playerRepository">Repositorio de jugadores.</param>
        /// <param name="mapper">Mapper para conversión entre entidades y DTOs.</param>
        public GetAllPlayersQueryHandler(IPlayerRepository playerRepository, IMapper mapper)
        {
            _playerRepository = playerRepository ?? throw new ArgumentNullException(nameof(playerRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Maneja la consulta para obtener todos los jugadores.
        /// </summary>
        /// <param name="request">Consulta para obtener todos los jugadores.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Lista de DTOs de jugadores.</returns>
        public async Task<IEnumerable<PlayerDto>> Handle(GetAllPlayersQuery request, CancellationToken cancellationToken)
        {
            var players = await _playerRepository.GetAllAsync();
            var playerDtos = new List<PlayerDto>();

            foreach (var player in players)
            {
                if (player is MalePlayer malePlayer)
                {
                    playerDtos.Add(_mapper.Map<MalePlayerDto>(malePlayer));
                }
                else if (player is FemalePlayer femalePlayer)
                {
                    playerDtos.Add(_mapper.Map<FemalePlayerDto>(femalePlayer));
                }
            }

            return playerDtos;
        }
    }
}