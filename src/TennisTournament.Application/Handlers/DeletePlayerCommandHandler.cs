using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using TennisTournament.Application.Commands;
using TennisTournament.Domain.Interfaces;

namespace TennisTournament.Application.Handlers
{
    /// <summary>
    /// Manejador para el comando de eliminación de jugadores.
    /// </summary>
    public class DeletePlayerCommandHandler : IRequestHandler<DeletePlayerCommand, bool>
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IValidator<DeletePlayerCommand> _validator;

        /// <summary>
        /// Constructor con inyección de dependencias.
        /// </summary>
        /// <param name="playerRepository">Repositorio de jugadores.</param>
        /// <param name="validator">Validador para el comando.</param>
        public DeletePlayerCommandHandler(
            IPlayerRepository playerRepository,
            IValidator<DeletePlayerCommand> validator)
        {
            _playerRepository = playerRepository ?? throw new ArgumentNullException(nameof(playerRepository));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        /// <summary>
        /// Maneja la eliminación de un jugador.
        /// </summary>
        /// <param name="request">Comando con el ID del jugador a eliminar.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>True si el jugador fue eliminado, False si no existía.</returns>
        public async Task<bool> Handle(DeletePlayerCommand request, CancellationToken cancellationToken)
        {
            // Validar el comando
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            // Obtener el jugador
            var player = await _playerRepository.GetByIdAsync(request.Id);
            if (player == null)
                return false;

            // Eliminar el jugador
            await _playerRepository.DeleteAsync(request.Id);
            return true;
        }
    }
}