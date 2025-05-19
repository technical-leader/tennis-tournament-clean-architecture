using System;
using System.ComponentModel.DataAnnotations;
using TennisTournament.Domain.Enums;

namespace TennisTournament.Application.DTOs
{
    /// <summary>
    /// DTO base para transferencia de datos de jugadores.
    /// </summary>
    public abstract class PlayerDto
    {
        /// <summary>
        /// Identificador único del jugador.
        /// </summary>
        /// <remarks>
        /// Corresponde a la propiedad Id en la entidad Player.
        /// </remarks>
        [Required(ErrorMessage = "El ID del jugador es obligatorio")]
        public Guid Id { get; set; }

        /// <summary>
        /// Nombre del jugador.
        /// </summary>
        /// <remarks>
        /// Corresponde a la propiedad Name en la entidad Player.
        /// </remarks>
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public required string Name { get; set; }

        /// <summary>
        /// Nivel de habilidad del jugador (entre 0 y 100).
        /// </summary>
        /// <remarks>
        /// Corresponde a la propiedad SkillLevel en la entidad Player.
        /// </remarks>
        [Required(ErrorMessage = "El nivel de habilidad es obligatorio")]
        [Range(0, 100, ErrorMessage = "El nivel de habilidad debe estar entre 0 y 100")]
        public int SkillLevel { get; set; }

        /// <summary>
        /// Tipo de jugador (Male o Female).
        /// </summary>
        /// <remarks>
        /// Corresponde a la propiedad PlayerType en la entidad Player.
        /// </remarks>
        [Required(ErrorMessage = "El tipo de jugador es obligatorio")]
        [EnumDataType(typeof(PlayerType), ErrorMessage = "El tipo de jugador debe ser un valor válido")]
        public PlayerType PlayerType { get; set; }
    }
}