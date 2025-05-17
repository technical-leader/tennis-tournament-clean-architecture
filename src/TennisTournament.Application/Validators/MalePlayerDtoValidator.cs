using FluentValidation;
using TennisTournament.Application.DTOs;
using TennisTournament.Domain.Enums;

namespace TennisTournament.Application.Validators
{
    /// <summary>
    /// Validador para el DTO de jugador masculino.
    /// </summary>
    public class MalePlayerDtoValidator : AbstractValidator<MalePlayerDto>
    {
        /// <summary>
        /// Constructor que configura las reglas de validación.
        /// </summary>
        public MalePlayerDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("El identificador del jugador es obligatorio.");

            RuleFor(x => x.PlayerType)
                .Equal(PlayerType.Male).WithMessage("El tipo de jugador debe ser masculino.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre del jugador es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres.");

            RuleFor(x => x.SkillLevel)
                .InclusiveBetween(0, 100).WithMessage("El nivel de habilidad debe estar entre 0 y 100.");

            RuleFor(x => x.Strength)
                .InclusiveBetween(0, 100).WithMessage("La fuerza debe estar entre 0 y 100.");

            RuleFor(x => x.Speed)
                .InclusiveBetween(0, 100).WithMessage("La velocidad debe estar entre 0 y 100.");
        }
    }
}