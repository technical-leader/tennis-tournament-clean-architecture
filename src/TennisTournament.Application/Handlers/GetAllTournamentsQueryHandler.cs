using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TennisTournament.Application.DTOs;
using TennisTournament.Application.Queries;
using TennisTournament.Domain.Interfaces;

namespace TennisTournament.Application.Handlers
{
    /// <summary>
    /// Manejador para la consulta de obtención de todos los torneos.
    /// </summary>
    public class GetAllTournamentsQueryHandler : IRequestHandler<GetAllTournamentsQuery, IEnumerable<TournamentShortDto>>
    {
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor con inyección de dependencias.
        /// </summary>
        /// <param name="tournamentRepository">Repositorio de torneos.</param>
        /// <param name="mapper">Mapper para conversión entre entidades y DTOs.</param>
        public GetAllTournamentsQueryHandler(ITournamentRepository tournamentRepository, IMapper mapper)
        {
            _tournamentRepository = tournamentRepository ?? throw new ArgumentNullException(nameof(tournamentRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Maneja la consulta para obtener todos los torneos con filtros opcionales.
        /// </summary>
        /// <param name="request">Consulta con filtros opcionales.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Lista de DTOs de torneos filtrados.</returns>
        public async Task<IEnumerable<TournamentShortDto>> Handle(GetAllTournamentsQuery request, CancellationToken cancellationToken)
        {
            var tournaments = await _tournamentRepository.GetAllAsync();

            // Aplicar filtros si están presentes
            if (request.Type.HasValue)
            {
                tournaments = tournaments.Where(t => t.Type == request.Type.Value);
            }

            if (request.Status.HasValue)
            {
                tournaments = tournaments.Where(t => t.Status == request.Status.Value);
            }

            return _mapper.Map<IEnumerable<TournamentShortDto>>(tournaments);
        }
    }
}