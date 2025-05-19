using FluentValidation;
using TennisTournament.Application.Commands;

namespace TennisTournament.Application.Validators
{
    /// <summary>
    /// Validador para el comando de simulación de torneos.
    /// </summary>
    public class SimulateTournamentCommandValidator : AbstractValidator<SimulateTournamentCommand>
    {
        /// <summary>
        /// Constructor que configura las reglas de validación.
        /// </summary>
        public SimulateTournamentCommandValidator()
        {
            RuleFor(x => x.TournamentId)
                .NotEmpty().WithMessage("El identificador del torneo es obligatorio.");
        }
    }
}