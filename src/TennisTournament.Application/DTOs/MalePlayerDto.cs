using System.ComponentModel.DataAnnotations;
using TennisTournament.Domain.Enums;

namespace TennisTournament.Application.DTOs
{
    /// <summary>
    /// DTO para transferencia de datos de jugadores masculinos.
    /// </summary>
    public class MalePlayerDto : PlayerDto, IValidatableObject
    {
        /// <summary>
        /// Nivel de fuerza del jugador (entre 0 y 100).
        /// </summary>
        /// <remarks>
        /// Corresponde a la propiedad Strength en la entidad MalePlayer.
        /// </remarks>
        [Required(ErrorMessage = "La fuerza es obligatoria para jugadores masculinos")]
        [Range(0, 100, ErrorMessage = "La fuerza debe estar entre 0 y 100")]
        public int Strength { get; set; }

        /// <summary>
        /// Velocidad de desplazamiento del jugador (entre 0 y 100).
        /// </summary>
        /// <remarks>
        /// Corresponde a la propiedad Speed en la entidad MalePlayer.
        /// </remarks>
        [Required(ErrorMessage = "La velocidad es obligatoria para jugadores masculinos")]
        [Range(0, 100, ErrorMessage = "La velocidad debe estar entre 0 y 100")]
        public int Speed { get; set; }

        /// <summary>
        /// Constructor por defecto que inicializa el tipo de jugador como masculino.
        /// </summary>
        public MalePlayerDto()
        {
            PlayerType = PlayerType.Male;
        }

        /// <summary>
        /// Validación adicional para asegurar que el tipo de jugador sea masculino.
        /// </summary>
        /// <param name="validationContext">Contexto de validación.</param>
        /// <returns>Errores de validación, si los hay.</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PlayerType != PlayerType.Male)
            {
                yield return new ValidationResult(
                    "El tipo de jugador debe ser masculino",
                    new[] { nameof(PlayerType) });
            }
        }
    }
}