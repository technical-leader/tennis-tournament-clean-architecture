using System.ComponentModel.DataAnnotations;
using TennisTournament.Domain.Enums;

namespace TennisTournament.Application.DTOs
{
    /// <summary>
    /// DTO para transferencia de datos de jugadoras femeninas.
    /// </summary>
    public class FemalePlayerDto : PlayerDto, IValidatableObject
    {
        /// <summary>
        /// Tiempo de reacción de la jugadora (entre 0 y 100).
        /// </summary>
        /// <remarks>
        /// Corresponde a la propiedad ReactionTime en la entidad FemalePlayer.
        /// </remarks>
        [Required(ErrorMessage = "El tiempo de reacción es obligatorio para jugadoras femeninas")]
        [Range(0, 100, ErrorMessage = "El tiempo de reacción debe estar entre 0 y 100")]
        public int ReactionTime { get; set; }

        /// <summary>
        /// Constructor por defecto que inicializa el tipo de jugador como femenino.
        /// </summary>
        public FemalePlayerDto()
        {
            PlayerType = PlayerType.Female;
        }

        /// <summary>
        /// Validación adicional para asegurar que el tipo de jugador sea femenino.
        /// </summary>
        /// <param name="validationContext">Contexto de validación.</param>
        /// <returns>Errores de validación, si los hay.</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PlayerType != PlayerType.Female)
            {
                yield return new ValidationResult(
                    "El tipo de jugador debe ser femenino",
                    new[] { nameof(PlayerType) });
            }
        }
    }
}