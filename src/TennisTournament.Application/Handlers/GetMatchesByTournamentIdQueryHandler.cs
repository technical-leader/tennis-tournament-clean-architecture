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
    /// Manejador para la consulta de obtención de partidos por torneo.
    /// </summary>
    public class GetMatchesByTournamentIdQueryHandler : IRequestHandler<GetMatchesByTournamentIdQuery, IEnumerable<MatchDto>>
    {
        private readonly IMatchRepository _matchRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor con inyección de dependencias.
        /// </summary>
        /// <param name="matchRepository">Repositorio de partidos.</param>
        /// <param name="mapper">Mapper para conversión entre entidades y DTOs.</param>
        public GetMatchesByTournamentIdQueryHandler(IMatchRepository matchRepository, IMapper mapper)
        {
            _matchRepository = matchRepository ?? throw new ArgumentNullException(nameof(matchRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Maneja la consulta para obtener partidos por torneo.
        /// </summary>
        /// <param name="request">Consulta con el identificador del torneo.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Lista de DTOs de partidos del torneo.</returns>
        public async Task<IEnumerable<MatchDto>> Handle(GetMatchesByTournamentIdQuery request, CancellationToken cancellationToken)
        {
            var matches = await _matchRepository.GetByTournamentIdAsync(request.TournamentId);
            return _mapper.Map<IEnumerable<MatchDto>>(matches);
        }
    }
}