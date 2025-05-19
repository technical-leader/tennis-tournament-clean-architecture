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
    /// Manejador para la consulta de obtención de resultados por tipo de torneo.
    /// </summary>
    public class GetResultsByTournamentTypeQueryHandler : IRequestHandler<GetResultsByTournamentTypeQuery, IEnumerable<ResultDto>>
    {
        private readonly IResultRepository _resultRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor con inyección de dependencias.
        /// </summary>
        /// <param name="resultRepository">Repositorio de resultados.</param>
        /// <param name="mapper">Mapper para conversión entre entidades y DTOs.</param>
        public GetResultsByTournamentTypeQueryHandler(IResultRepository resultRepository, IMapper mapper)
        {
            _resultRepository = resultRepository ?? throw new ArgumentNullException(nameof(resultRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Maneja la consulta para obtener resultados por tipo de torneo.
        /// </summary>
        /// <param name="request">Consulta con el tipo de torneo.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Lista de DTOs de resultados del tipo de torneo especificado.</returns>
        public async Task<IEnumerable<ResultDto>> Handle(GetResultsByTournamentTypeQuery request, CancellationToken cancellationToken)
        {
            var results = await _resultRepository.GetByTournamentTypeAsync(request.Type);
            return _mapper.Map<IEnumerable<ResultDto>>(results);
        }
    }
}