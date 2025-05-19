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
    /// Manejador para la consulta de obtención de resultados por rango de fechas.
    /// </summary>
    public class GetResultsByDateRangeQueryHandler : IRequestHandler<GetResultsByDateRangeQuery, IEnumerable<ResultDto>>
    {
        private readonly IResultRepository _resultRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor con inyección de dependencias.
        /// </summary>
        /// <param name="resultRepository">Repositorio de resultados.</param>
        /// <param name="mapper">Mapper para conversión entre entidades y DTOs.</param>
        public GetResultsByDateRangeQueryHandler(IResultRepository resultRepository, IMapper mapper)
        {
            _resultRepository = resultRepository ?? throw new ArgumentNullException(nameof(resultRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Maneja la consulta para obtener resultados por rango de fechas.
        /// </summary>
        /// <param name="request">Consulta con el rango de fechas.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Lista de DTOs de resultados en el rango de fechas.</returns>
        public async Task<IEnumerable<ResultDto>> Handle(GetResultsByDateRangeQuery request, CancellationToken cancellationToken)
        {
            var results = await _resultRepository.GetByDateRangeAsync(request.StartDate, request.EndDate);
            return _mapper.Map<IEnumerable<ResultDto>>(results);
        }
    }
}