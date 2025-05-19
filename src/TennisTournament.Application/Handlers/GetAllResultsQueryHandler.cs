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
    /// Manejador para la consulta de obtención de todos los resultados.
    /// </summary>
    public class GetAllResultsQueryHandler : IRequestHandler<GetAllResultsQuery, IEnumerable<ResultDto>>
    {
        private readonly IResultRepository _resultRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor con inyección de dependencias.
        /// </summary>
        /// <param name="resultRepository">Repositorio de resultados.</param>
        /// <param name="mapper">Mapper para conversión entre entidades y DTOs.</param>
        public GetAllResultsQueryHandler(IResultRepository resultRepository, IMapper mapper)
        {
            _resultRepository = resultRepository ?? throw new ArgumentNullException(nameof(resultRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Maneja la consulta para obtener todos los resultados.
        /// </summary>
        /// <param name="request">Consulta para obtener todos los resultados.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Lista de DTOs de resultados.</returns>
        public async Task<IEnumerable<ResultDto>> Handle(GetAllResultsQuery request, CancellationToken cancellationToken)
        {
            var results = await _resultRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ResultDto>>(results);
        }
    }
}