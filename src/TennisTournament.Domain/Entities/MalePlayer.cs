using System;
using TennisTournament.Domain.Enums;

namespace TennisTournament.Domain.Entities
{
    /// <summary>
    /// Clase que representa a un jugador masculino de tenis.
    /// Extiende la clase base Player y añade atributos específicos para jugadores masculinos.
    /// </summary>
    public class MalePlayer : Player
    {
        /// <summary>
        /// Tipo de jugador (masculino).
        /// </summary>
        public override PlayerType PlayerType 
        { 
            get => PlayerType.Male;
            protected set { } // Necesario para EF Core
        }

        /// <summary>
        /// Nivel de fuerza del jugador (entre 0 y 100).
        /// </summary>
        private int _strength;
        public int Strength
        {
            get => _strength;
            set
            {
                if (value < 0 || value > 100)
                    throw new ArgumentOutOfRangeException(nameof(Strength), "El nivel de fuerza debe estar entre 0 y 100.");
                _strength = value;
            }
        }

        /// <summary>
        /// Velocidad de desplazamiento del jugador (entre 0 y 100).
        /// </summary>
        private int _speed;
        public int Speed
        {
            get => _speed;
            set
            {
                if (value < 0 || value > 100)
                    throw new ArgumentOutOfRangeException(nameof(Speed), "La velocidad debe estar entre 0 y 100.");
                _speed = value;
            }
        }

        /// <summary>
        /// Constructor por defecto.
        /// </summary>
        public MalePlayer() : base()
        {
            PlayerType = Enums.PlayerType.Male;
        }

        /// <summary>
        /// Constructor con parámetros.
        /// </summary>
        /// <param name="name">Nombre del jugador.</param>
        /// <param name="skillLevel">Nivel de habilidad del jugador.</param>
        /// <param name="strength">Nivel de fuerza del jugador.</param>
        /// <param name="speed">Velocidad de desplazamiento del jugador.</param>
        public MalePlayer(string name, int skillLevel, int strength, int speed) : base(name, skillLevel)
        {
            PlayerType = Enums.PlayerType.Male;
            Strength = strength;
            Speed = speed;
        }

        /// <summary>
        /// Implementación del método para calcular la puntuación total del jugador masculino.
        /// Considera el nivel de habilidad, la fuerza, la velocidad y el factor de suerte.
        /// </summary>
        /// <param name="luckFactor">Factor de suerte (entre 0 y 20).</param>
        /// <returns>Puntuación total calculada.</returns>
        public override double CalculateTotalScore(double luckFactor)
        {
            // Fórmula: (NivelHabilidad * 0.6) + ((Fuerza + Velocidad) / 2 * 0.3) + (FactorSuerte * 0.1)
            double skillComponent = SkillLevel * 0.6;
            double attributesComponent = ((Strength + Speed) / 2.0) * 0.3;
            double luckComponent = luckFactor * 0.1;

            return skillComponent + attributesComponent + luckComponent;
        }
    }
}