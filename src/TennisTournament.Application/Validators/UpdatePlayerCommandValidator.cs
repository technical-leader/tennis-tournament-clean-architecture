using FluentValidation;
using TennisTournament.Application.Commands;
using TennisTournament.Domain.Enums;

namespace TennisTournament.Application.Validators
{
    /// <summary>
    /// Validador para el comando de actualización de jugadores.
    /// </summary>
    public class UpdatePlayerCommandValidator : AbstractValidator<UpdatePlayerCommand>
    {
        /// <summary>
        /// Constructor que configura las reglas de validación.
        /// </summary>
        public UpdatePlayerCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("El identificador del jugador es obligatorio.");

            RuleFor(x => x.PlayerType)
                .IsInEnum().WithMessage("El tipo de jugador debe ser un valor válido del enumerado PlayerType.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre del jugador es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres.");

            RuleFor(x => x.SkillLevel)
                .InclusiveBetween(0, 100).WithMessage("El nivel de habilidad debe estar entre 0 y 100.");

            // Validaciones condicionales para jugadores masculinos
            When(x => x.PlayerType == PlayerType.Male, () =>
            {
                RuleFor(x => x.Strength)
                    .NotNull().WithMessage("La fuerza es obligatoria para jugadores masculinos.")
                    .InclusiveBetween(0, 100).WithMessage("La fuerza debe estar entre 0 y 100.");

                RuleFor(x => x.Speed)
                    .NotNull().WithMessage("La velocidad es obligatoria para jugadores masculinos.")
                    .InclusiveBetween(0, 100).WithMessage("La velocidad debe estar entre 0 y 100.");
            });

            // Validaciones condicionales para jugadoras femeninas
            When(x => x.PlayerType == PlayerType.Female, () =>
            {
                RuleFor(x => x.ReactionTime)
                    .NotNull().WithMessage("El tiempo de reacción es obligatorio para jugadoras femeninas.")
                    .InclusiveBetween(0, 100).WithMessage("El tiempo de reacción debe estar entre 0 y 100.");
            });
        }
    }
}