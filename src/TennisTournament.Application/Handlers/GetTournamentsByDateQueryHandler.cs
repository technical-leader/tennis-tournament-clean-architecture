using System;
using System.Collections.Generic;
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
    /// Manejador para la consulta de obtención de torneos por fecha.
    /// </summary>
    public class GetTournamentsByDateQueryHandler : IRequestHandler<GetTournamentsByDateQuery, IEnumerable<TournamentDto>>
    {
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor con inyección de dependencias.
        /// </summary>
        /// <param name="tournamentRepository">Repositorio de torneos.</param>
        /// <param name="mapper">Mapper para conversión entre entidades y DTOs.</param>
        public GetTournamentsByDateQueryHandler(ITournamentRepository tournamentRepository, IMapper mapper)
        {
            _tournamentRepository = tournamentRepository ?? throw new ArgumentNullException(nameof(tournamentRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Maneja la consulta para obtener torneos por fecha.
        /// </summary>
        /// <param name="request">Consulta con la fecha de los torneos a obtener.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Lista de DTOs de torneos en la fecha especificada.</returns>
        public async Task<IEnumerable<TournamentDto>> Handle(GetTournamentsByDateQuery request, CancellationToken cancellationToken)
        {
            var tournaments = await _tournamentRepository.GetByDateRangeAsync(
                request.StartDate, 
                request.EndDate);
                
            return _mapper.Map<IEnumerable<TournamentDto>>(tournaments);
        }
    }
}