using FluentValidation;
using System;
using System.Linq;
using TennisTournament.Application.Commands;

namespace TennisTournament.Application.Validators
{
    /// <summary>
    /// Validador para el comando de creación de torneos.
    /// </summary>
    public class CreateTournamentCommandValidator : AbstractValidator<CreateTournamentCommand>
    {
        /// <summary>
        /// Constructor que configura las reglas de validación.
        /// </summary>
        public CreateTournamentCommandValidator()
        {
            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("El tipo de torneo no es válido.");

            RuleFor(x => x.PlayerIds)
                .NotEmpty().WithMessage("Debe proporcionar al menos un jugador para el torneo.")
                .Must(playerIds => IsPowerOfTwo(playerIds.Count))
                .WithMessage("El número de jugadores debe ser una potencia de 2 (2, 4, 8, 16, etc.).");

            RuleFor(x => x.StartDate)
                .GreaterThanOrEqualTo(DateTime.Today)
                .WithMessage("La fecha de inicio debe ser igual o posterior a la fecha actual.");
        }

        /// <summary>
        /// Verifica si un número es potencia de 2.
        /// </summary>
        /// <param name="n">Número a verificar.</param>
        /// <returns>True si es potencia de 2, False en caso contrario.</returns>
        private bool IsPowerOfTwo(int n)
        {
            return n > 0 && (n & (n - 1)) == 0;
        }
    }
}