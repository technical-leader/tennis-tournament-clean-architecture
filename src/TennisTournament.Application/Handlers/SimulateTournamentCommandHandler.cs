using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TennisTournament.Application.Commands;
using TennisTournament.Application.DTOs;
using TennisTournament.Domain.Entities;
using TennisTournament.Domain.Interfaces;
using TennisTournament.Domain.Services;

namespace TennisTournament.Application.Handlers
{
    /// <summary>
    /// Manejador para el comando de simulación de torneos.
    /// </summary>
    public class SimulateTournamentCommandHandler : IRequestHandler<SimulateTournamentCommand, ResultDto>
    {
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IResultRepository _resultRepository;
        private readonly TournamentSimulationService _simulationService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor con inyección de dependencias.
        /// </summary>
        /// <param name="tournamentRepository">Repositorio de torneos.</param>
        /// <param name="resultRepository">Repositorio de resultados.</param>
        /// <param name="simulationService">Servicio de simulación de torneos.</param>
        /// <param name="mapper">Mapper para conversión entre entidades y DTOs.</param>
        public SimulateTournamentCommandHandler(
            ITournamentRepository tournamentRepository,
            IResultRepository resultRepository,
            TournamentSimulationService simulationService,
            IMapper mapper)
        {
            _tournamentRepository = tournamentRepository ?? throw new ArgumentNullException(nameof(tournamentRepository));
            _resultRepository = resultRepository ?? throw new ArgumentNullException(nameof(resultRepository));
            _simulationService = simulationService ?? throw new ArgumentNullException(nameof(simulationService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Maneja la simulación de un torneo.
        /// </summary>
        /// <param name="request">Comando con el identificador del torneo a simular.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>DTO del resultado del torneo.</returns>
        public async Task<ResultDto> Handle(SimulateTournamentCommand request, CancellationToken cancellationToken)
        {
            // Obtener el torneo del repositorio
            var tournament = await _tournamentRepository.GetByIdAsync(request.TournamentId);
            if (tournament == null)
                throw new ArgumentException($"No se encontró el torneo con ID {request.TournamentId}.", nameof(request.TournamentId));

            // Verificar que el torneo no haya sido simulado ya
            if (tournament.Status == Domain.Enums.TournamentStatus.Completed)
                throw new InvalidOperationException($"El torneo con ID {request.TournamentId} ya ha sido completado.");

            // Simular el torneo
            Result result = _simulationService.SimulateTournament(tournament);

            // Actualizar el torneo en el repositorio
            await _tournamentRepository.UpdateAsync(tournament);

            // Guardar el resultado en el repositorio
            var savedResult = await _resultRepository.AddAsync(result);

            // Mapear el resultado al DTO
            return _mapper.Map<ResultDto>(savedResult);
        }
    }
}