using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TennisTournament.Application.DTOs;
using TennisTournament.Application.Interfaces;
using TennisTournament.Domain.Entities;
using TennisTournament.Domain.Enums;
using TennisTournament.Domain.Interfaces;
using TennisTournament.Domain.Services;

namespace TennisTournament.Application.Services
{
    /// <summary>
    /// Implementación del servicio de gestión de torneos.
    /// </summary>
    public class TournamentService : ITournamentService
    {
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IResultRepository _resultRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly TournamentSimulationService _simulationService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor con inyección de dependencias.
        /// </summary>
        /// <param name="tournamentRepository">Repositorio de torneos.</param>
        /// <param name="playerRepository">Repositorio de jugadores.</param>
        /// <param name="resultRepository">Repositorio de resultados.</param>
        /// <param name="matchRepository">Repositorio de partidos.</param>
        /// <param name="simulationService">Servicio de simulación de torneos.</param>
        /// <param name="mapper">Mapper para conversión entre entidades y DTOs.</param>
        public TournamentService(
            ITournamentRepository tournamentRepository,
            IPlayerRepository playerRepository,
            IResultRepository resultRepository,
            IMatchRepository matchRepository,
            TournamentSimulationService simulationService,
            IMapper mapper)
        {
            _tournamentRepository = tournamentRepository ?? throw new ArgumentNullException(nameof(tournamentRepository));
            _playerRepository = playerRepository ?? throw new ArgumentNullException(nameof(playerRepository));
            _resultRepository = resultRepository ?? throw new ArgumentNullException(nameof(resultRepository));
            _matchRepository = matchRepository ?? throw new ArgumentNullException(nameof(matchRepository));
            _simulationService = simulationService ?? throw new ArgumentNullException(nameof(simulationService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <inheritdoc/>
        public async Task<TournamentDto?> GetTournamentByIdAsync(Guid id)
        {
            var tournament = await _tournamentRepository.GetByIdAsync(id);
            return tournament != null ? _mapper.Map<TournamentDto>(tournament) : null;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TournamentDto>> GetTournamentsByTypeAsync(TournamentType type)
        {
            var tournaments = await _tournamentRepository.GetByTypeAsync(type);
            return _mapper.Map<IEnumerable<TournamentDto>>(tournaments);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TournamentDto>> GetTournamentsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var tournaments = await _tournamentRepository.GetByDateRangeAsync(startDate, endDate);
            return _mapper.Map<IEnumerable<TournamentDto>>(tournaments);
        }

        /// <inheritdoc/>
        public async Task<TournamentDto> CreateTournamentAsync(TournamentType type, List<Guid> playerIds, DateTime startDate)
        {
            // Validar que el número de jugadores sea potencia de 2
            if ((playerIds.Count & (playerIds.Count - 1)) != 0)
                throw new ArgumentException("El número de jugadores debe ser una potencia de 2.");

            // Validar que todos los jugadores existan y sean del tipo correcto
            var players = new List<Player>();
            foreach (var playerId in playerIds)
            {
                var player = await _playerRepository.GetByIdAsync(playerId);
                if (player == null)
                    throw new ArgumentException($"El jugador con ID {playerId} no existe.");

                // Verificar que el tipo de jugador coincida con el tipo de torneo
                if ((type == TournamentType.Male && player is not MalePlayer) ||
                    (type == TournamentType.Female && player is not FemalePlayer))
                {
                    throw new ArgumentException($"El jugador con ID {playerId} no es compatible con el tipo de torneo {type}.");
                }

                players.Add(player);
            }

            // Crear el torneo
            var tournament = new Tournament
            {
                Type = type,
                StartDate = startDate,
                Status = TournamentStatus.Scheduled
            };
            
            // Usar el método SetPlayers en lugar de asignar directamente a la propiedad
            tournament.SetPlayers(players);

            // Guardar el torneo
            var createdTournament = await _tournamentRepository.AddAsync(tournament);
            return _mapper.Map<TournamentDto>(createdTournament);
        }

        /// <inheritdoc/>
        public async Task<ResultDto> SimulateTournamentAsync(Guid tournamentId)
        {
            // Obtener el torneo
            var tournament = await _tournamentRepository.GetByIdAsync(tournamentId);
            if (tournament == null)
                throw new ArgumentException($"El torneo con ID {tournamentId} no existe.");

            // Verificar que el torneo no haya sido simulado ya
            if (tournament.Status == TournamentStatus.Completed)
                throw new InvalidOperationException("El torneo ya ha sido simulado.");

            // Simular el torneo
            var result = _simulationService.SimulateTournament(tournament);

            // Guardar los partidos generados durante la simulación
            foreach (var match in tournament.Matches)
            {
                await _matchRepository.AddAsync(match);
            }

            // Actualizar el estado del torneo
            tournament.Status = TournamentStatus.Completed;
            tournament.EndDate = DateTime.UtcNow;
            await _tournamentRepository.UpdateAsync(tournament);

            // Guardar el resultado
            var savedResult = await _resultRepository.AddAsync(result);

            return _mapper.Map<ResultDto>(savedResult);
        }

        /// <inheritdoc/>
        public async Task<ResultDto?> GetTournamentResultAsync(Guid tournamentId)
        {
            var result = await _resultRepository.GetByTournamentIdAsync(tournamentId);
            return result != null ? _mapper.Map<ResultDto>(result) : null;
        }
    }
}