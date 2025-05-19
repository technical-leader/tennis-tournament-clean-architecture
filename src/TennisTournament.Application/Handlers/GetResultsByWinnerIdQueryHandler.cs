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
    /// Manejador para la consulta de obtención de resultados por jugador ganador.
    /// </summary>
    public class GetResultsByWinnerIdQueryHandler : IRequestHandler<GetResultsByWinnerIdQuery, IEnumerable<ResultDto>>
    {
        private readonly IResultRepository _resultRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor con inyección de dependencias.
        /// </summary>
        /// <param name="resultRepository">Repositorio de resultados.</param>
        /// <param name="mapper">Mapper para conversión entre entidades y DTOs.</param>
        public GetResultsByWinnerIdQueryHandler(IResultRepository resultRepository, IMapper mapper)
        {
            _resultRepository = resultRepository ?? throw new ArgumentNullException(nameof(resultRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Maneja la consulta para obtener resultados por jugador ganador.
        /// </summary>
        /// <param name="request">Consulta con el identificador del jugador ganador.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Lista de DTOs de resultados donde el jugador especificado fue el ganador.</returns>
        public async Task<IEnumerable<ResultDto>> Handle(GetResultsByWinnerIdQuery request, CancellationToken cancellationToken)
        {
            var results = await _resultRepository.GetByWinnerIdAsync(request.PlayerId);
            return _mapper.Map<IEnumerable<ResultDto>>(results);
        }
    }
}