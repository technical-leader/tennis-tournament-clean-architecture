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
    /// Manejador para la consulta de obtención de un resultado por su identificador.
    /// </summary>
    public class GetResultByIdQueryHandler : IRequestHandler<GetResultByIdQuery, ResultDto>
    {
        private readonly IResultRepository _resultRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor con inyección de dependencias.
        /// </summary>
        /// <param name="resultRepository">Repositorio de resultados.</param>
        /// <param name="mapper">Mapper para conversión entre entidades y DTOs.</param>
        public GetResultByIdQueryHandler(IResultRepository resultRepository, IMapper mapper)
        {
            _resultRepository = resultRepository ?? throw new ArgumentNullException(nameof(resultRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Maneja la consulta para obtener un resultado por su identificador.
        /// </summary>
        /// <param name="request">Consulta con el identificador del resultado.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>DTO del resultado encontrado o null si no existe.</returns>
        public async Task<ResultDto> Handle(GetResultByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _resultRepository.GetByIdAsync(request.Id);
            return _mapper.Map<ResultDto>(result);
        }
    }
}