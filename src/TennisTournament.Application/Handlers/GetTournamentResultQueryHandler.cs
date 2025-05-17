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
    /// Manejador para la consulta GetTournamentResultQuery.
    /// </summary>
    public class GetTournamentResultQueryHandler : IRequestHandler<GetTournamentResultQuery, ResultDto?>
    {
        private readonly IResultRepository _resultRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor con inyección de dependencias.
        /// </summary>
        /// <param name="resultRepository">Repositorio de resultados.</param>
        /// <param name="mapper">Mapper para conversión entre entidades y DTOs.</param>
        public GetTournamentResultQueryHandler(IResultRepository resultRepository, IMapper mapper)
        {
            _resultRepository = resultRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Maneja la consulta para obtener el resultado de un torneo.
        /// </summary>
        /// <param name="request">Consulta con el identificador del torneo.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>DTO del resultado del torneo o null si no existe.</returns>
        public async Task<ResultDto?> Handle(GetTournamentResultQuery request, CancellationToken cancellationToken)
        {
            var result = await _resultRepository.GetByTournamentIdAsync(request.TournamentId);
            return result != null ? _mapper.Map<ResultDto>(result) : null;
        }
    }
}