using System;
using TennisTournament.Domain.Enums;

namespace TennisTournament.Domain.Entities
{
    /// <summary>
    /// Clase que representa a una jugadora femenina de tenis.
    /// Extiende la clase base Player y añade atributos específicos para jugadoras femeninas.
    /// </summary>
    public class FemalePlayer : Player
    {
        /// <summary>
        /// Tipo de jugador (femenino).
        /// </summary>
        public override PlayerType PlayerType 
        { 
            get => PlayerType.Female;
            protected set { } // Necesario para EF Core
        }

        /// <summary>
        /// Tiempo de reacción de la jugadora (entre 0 y 100, donde valores más altos indican mejor tiempo de reacción).
        /// </summary>
        private int _reactionTime;
        public int ReactionTime
        {
            get => _reactionTime;
            set
            {
                if (value < 0 || value > 100)
                    throw new ArgumentOutOfRangeException(nameof(ReactionTime), "El tiempo de reacción debe estar entre 0 y 100.");
                _reactionTime = value;
            }
        }

        /// <summary>
        /// Constructor por defecto.
        /// </summary>
        public FemalePlayer() : base()
        {
            PlayerType = Enums.PlayerType.Female;
        }

        /// <summary>
        /// Constructor con parámetros.
        /// </summary>
        /// <param name="name">Nombre de la jugadora.</param>
        /// <param name="skillLevel">Nivel de habilidad de la jugadora.</param>
        /// <param name="reactionTime">Tiempo de reacción de la jugadora.</param>
        public FemalePlayer(string name, int skillLevel, int reactionTime) : base(name, skillLevel)
        {
            PlayerType = Enums.PlayerType.Female;
            ReactionTime = reactionTime;
        }

        /// <summary>
        /// Implementación del método para calcular la puntuación total de la jugadora femenina.
        /// Considera el nivel de habilidad, el tiempo de reacción y el factor de suerte.
        /// </summary>
        /// <param name="luckFactor">Factor de suerte (entre 0 y 20).</param>
        /// <returns>Puntuación total calculada.</returns>
        public override double CalculateTotalScore(double luckFactor)
        {
            // Fórmula: (NivelHabilidad * 0.6) + (TiempoReacción * 0.3) + (FactorSuerte * 0.1)
            double skillComponent = SkillLevel * 0.6;
            double reactionTimeComponent = ReactionTime * 0.3;
            double luckComponent = luckFactor * 0.1;

            return skillComponent + reactionTimeComponent + luckComponent;
        }
    }
}