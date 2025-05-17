using FluentValidation;
using TennisTournament.Application.Commands;

namespace TennisTournament.Application.Validators
{
    /// <summary>
    /// Validador para el comando de eliminación de jugadores.
    /// </summary>
    public class DeletePlayerCommandValidator : AbstractValidator<DeletePlayerCommand>
    {
        /// <summary>
        /// Constructor que configura las reglas de validación.
        /// </summary>
        public DeletePlayerCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("El identificador del jugador es obligatorio.");
        }
    }
}