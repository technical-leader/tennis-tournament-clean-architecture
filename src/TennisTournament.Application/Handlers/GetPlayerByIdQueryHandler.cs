using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TennisTournament.Application.DTOs;
using TennisTournament.Application.Queries;
using TennisTournament.Domain.Entities;
using TennisTournament.Domain.Interfaces;

namespace TennisTournament.Application.Handlers
{
    /// <summary>
    /// Manejador para la consulta GetPlayerByIdQuery.
    /// </summary>
    public class GetPlayerByIdQueryHandler : IRequestHandler<GetPlayerByIdQuery, PlayerDto?>
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor con inyección de dependencias.
        /// </summary>
        /// <param name="playerRepository">Repositorio de jugadores.</param>
        /// <param name="mapper">Mapper para conversión entre entidades y DTOs.</param>
        public GetPlayerByIdQueryHandler(IPlayerRepository playerRepository, IMapper mapper)
        {
            _playerRepository = playerRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Maneja la consulta para obtener un jugador por su ID.
        /// </summary>
        /// <param name="request">Consulta con el ID del jugador a obtener.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>DTO del jugador o null si no existe.</returns>
        public async Task<PlayerDto?> Handle(GetPlayerByIdQuery request, CancellationToken cancellationToken)
        {
            var player = await _playerRepository.GetByIdAsync(request.Id);
            if (player == null)
                return null;

            // Mapear el jugador al DTO correspondiente según su tipo
            if (player is MalePlayer malePlayer)
                return _mapper.Map<MalePlayerDto>(malePlayer);
            else if (player is FemalePlayer femalePlayer)
                return _mapper.Map<FemalePlayerDto>(femalePlayer);

            // Caso genérico (no debería ocurrir con la implementación actual)
            return _mapper.Map<PlayerDto>(player);
        }
    }
}