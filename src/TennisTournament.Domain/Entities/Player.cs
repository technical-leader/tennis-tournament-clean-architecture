using System;
using TennisTournament.Domain.Enums;

namespace TennisTournament.Domain.Entities
{
    /// <summary>
    /// Clase base abstracta para jugadores de tenis.
    /// </summary>
    public abstract class Player
    {
        /// <summary>
        /// Identificador único del jugador.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Nombre del jugador.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Nivel de habilidad del jugador (entre 0 y 100).
        /// </summary>
        public int SkillLevel { get; set; }

        /// <summary>
        /// Tipo de jugador (masculino o femenino).
        /// </summary>
        public abstract PlayerType PlayerType { get; protected set; }

        /// <summary>
        /// Constructor por defecto.
        /// </summary>
        protected Player()
        {
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// Constructor con parámetros.
        /// </summary>
        /// <param name="name">Nombre del jugador.</param>
        /// <param name="skillLevel">Nivel de habilidad del jugador.</param>
        protected Player(string name, int skillLevel)
        {
            Id = Guid.NewGuid();
            Name = name;
            SkillLevel = skillLevel;
        }

        /// <summary>
        /// Calcula la puntuación total del jugador para un partido.
        /// </summary>
        /// <param name="luckFactor">Factor de suerte (entre 0 y 20).</param>
        /// <returns>Puntuación total.</returns>
        public abstract double CalculateTotalScore(double luckFactor);
    }
}