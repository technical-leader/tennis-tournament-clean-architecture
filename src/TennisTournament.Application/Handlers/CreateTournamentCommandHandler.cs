using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TennisTournament.Application.Commands;
using TennisTournament.Application.DTOs;
using TennisTournament.Domain.Entities;
using TennisTournament.Domain.Interfaces;

namespace TennisTournament.Application.Handlers
{
    /// <summary>
    /// Manejador para el comando de creación de torneos.
    /// </summary>
    public class CreateTournamentCommandHandler : IRequestHandler<CreateTournamentCommand, TournamentDto>
    {
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor con inyección de dependencias.
        /// </summary>
        /// <param name="tournamentRepository">Repositorio de torneos.</param>
        /// <param name="playerRepository">Repositorio de jugadores.</param>
        /// <param name="mapper">Mapper para conversión entre entidades y DTOs.</param>
        public CreateTournamentCommandHandler(
            ITournamentRepository tournamentRepository,
            IPlayerRepository playerRepository,
            IMapper mapper)
        {
            _tournamentRepository = tournamentRepository ?? throw new ArgumentNullException(nameof(tournamentRepository));
            _playerRepository = playerRepository ?? throw new ArgumentNullException(nameof(playerRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Maneja la creación de un nuevo torneo.
        /// </summary>
        /// <param name="request">Comando con los datos del torneo a crear.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>DTO del torneo creado.</returns>
        public async Task<TournamentDto> Handle(CreateTournamentCommand request, CancellationToken cancellationToken)
        {
            // Validar que hay jugadores
            if (request.PlayerIds == null || !request.PlayerIds.Any())
                throw new ArgumentException("La lista de jugadores no puede estar vacía.", nameof(request.PlayerIds));

            // Verificar que el número de jugadores sea potencia de 2
            int playerCount = request.PlayerIds.Count;
            if ((playerCount & (playerCount - 1)) != 0)
                throw new ArgumentException("El número de jugadores debe ser una potencia de 2.", nameof(request.PlayerIds));

            // Obtener los jugadores del repositorio
            var players = new List<Player>();
            foreach (var playerId in request.PlayerIds)
            {
                var player = await _playerRepository.GetByIdAsync(playerId);
                if (player == null)
                    throw new ArgumentException($"No se encontró el jugador con ID {playerId}.", nameof(request.PlayerIds));
                players.Add(player);
            }

            // Verificar que todos los jugadores sean del mismo tipo según el torneo
            bool allPlayersValid = request.Type == Domain.Enums.TournamentType.Male
                ? players.All(p => p is MalePlayer)
                : players.All(p => p is FemalePlayer);

            if (!allPlayersValid)
                throw new ArgumentException($"Todos los jugadores deben ser del tipo {request.Type}.", nameof(request.PlayerIds));

            // Crear el torneo
            var tournament = new Tournament(request.Type, players);

            // Guardar el torneo en el repositorio
            var createdTournament = await _tournamentRepository.AddAsync(tournament);

            // Mapear el torneo creado al DTO
            return _mapper.Map<TournamentDto>(createdTournament);
        }
    }
}