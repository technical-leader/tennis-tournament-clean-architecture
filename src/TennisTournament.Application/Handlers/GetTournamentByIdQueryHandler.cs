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
    /// Manejador para la consulta GetTournamentByIdQuery.
    /// </summary>
    public class GetTournamentByIdQueryHandler : IRequestHandler<GetTournamentByIdQuery, TournamentDto?>
    {
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor con inyección de dependencias.
        /// </summary>
        /// <param name="tournamentRepository">Repositorio de torneos.</param>
        /// <param name="mapper">Mapper para conversión entre entidades y DTOs.</param>
        public GetTournamentByIdQueryHandler(ITournamentRepository tournamentRepository, IMapper mapper)
        {
            _tournamentRepository = tournamentRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Maneja la consulta para obtener un torneo por su identificador.
        /// </summary>
        /// <param name="request">Consulta con el identificador del torneo.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>DTO del torneo o null si no existe.</returns>
        public async Task<TournamentDto?> Handle(GetTournamentByIdQuery request, CancellationToken cancellationToken)
        {
            var tournament = await _tournamentRepository.GetByIdAsync(request.Id);
            return tournament != null ? _mapper.Map<TournamentDto>(tournament) : null;
        }
    }
}