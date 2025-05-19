using System;
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
    /// Manejador para la consulta de obtención de un partido por su identificador.
    /// </summary>
    public class GetMatchByIdQueryHandler : IRequestHandler<GetMatchByIdQuery, MatchDto>
    {
        private readonly IMatchRepository _matchRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor con inyección de dependencias.
        /// </summary>
        /// <param name="matchRepository">Repositorio de partidos.</param>
        /// <param name="mapper">Mapper para conversión entre entidades y DTOs.</param>
        public GetMatchByIdQueryHandler(IMatchRepository matchRepository, IMapper mapper)
        {
            _matchRepository = matchRepository ?? throw new ArgumentNullException(nameof(matchRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Maneja la consulta para obtener un partido por su identificador.
        /// </summary>
        /// <param name="request">Consulta con el identificador del partido.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>DTO del partido encontrado o null si no existe.</returns>
        public async Task<MatchDto> Handle(GetMatchByIdQuery request, CancellationToken cancellationToken)
        {
            var match = await _matchRepository.GetByIdAsync(request.Id);
            return _mapper.Map<MatchDto>(match);
        }
    }
}