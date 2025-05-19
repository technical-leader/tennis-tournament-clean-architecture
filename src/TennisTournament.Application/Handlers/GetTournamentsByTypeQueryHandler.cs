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
    /// Manejador para la consulta de obtención de torneos por tipo.
    /// </summary>
    public class GetTournamentsByTypeQueryHandler : IRequestHandler<GetTournamentsByTypeQuery, IEnumerable<TournamentDto>>
    {
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor con inyección de dependencias.
        /// </summary>
        /// <param name="tournamentRepository">Repositorio de torneos.</param>
        /// <param name="mapper">Mapper para conversión entre entidades y DTOs.</param>
        public GetTournamentsByTypeQueryHandler(ITournamentRepository tournamentRepository, IMapper mapper)
        {
            _tournamentRepository = tournamentRepository ?? throw new ArgumentNullException(nameof(tournamentRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Maneja la consulta para obtener torneos por tipo.
        /// </summary>
        /// <param name="request">Consulta con el tipo de torneos a obtener.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Lista de DTOs de torneos del tipo especificado.</returns>
        public async Task<IEnumerable<TournamentDto>> Handle(GetTournamentsByTypeQuery request, CancellationToken cancellationToken)
        {
            var tournaments = await _tournamentRepository.GetByTypeAsync(request.Type);
            return _mapper.Map<IEnumerable<TournamentDto>>(tournaments);
        }
    }
}