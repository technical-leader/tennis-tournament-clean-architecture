using FluentValidation;
using TennisTournament.Application.DTOs;
using TennisTournament.Domain.Enums;

namespace TennisTournament.Application.Validators
{
    /// <summary>
    /// Validador para el DTO de jugadora femenina.
    /// </summary>
    public class FemalePlayerDtoValidator : AbstractValidator<FemalePlayerDto>
    {
        /// <summary>
        /// Constructor que configura las reglas de validación.
        /// </summary>
        public FemalePlayerDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("El identificador de la jugadora es obligatorio.");

            RuleFor(x => x.PlayerType)
                .Equal(PlayerType.Female).WithMessage("El tipo de jugador debe ser femenino.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre de la jugadora es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres.");

            RuleFor(x => x.SkillLevel)
                .InclusiveBetween(0, 100).WithMessage("El nivel de habilidad debe estar entre 0 y 100.");

            RuleFor(x => x.ReactionTime)
                .InclusiveBetween(0, 100).WithMessage("El tiempo de reacción debe estar entre 0 y 100.");
        }
    }
}