using System;

namespace TennisTournament.Infrastructure.Services
{
    /// <summary>
    /// Servicio para generar números aleatorios.
    /// </summary>
    public class RandomNumberService : IRandomNumberService
    {
        private readonly Random _random;

        /// <summary>
        /// Constructor que inicializa el generador de números aleatorios.
        /// </summary>
        public RandomNumberService()
        {
            _random = new Random();
        }

        /// <summary>
        /// Genera un número entero aleatorio entre los valores mínimo y máximo (inclusive).
        /// </summary>
        /// <param name="minValue">Valor mínimo (inclusive).</param>
        /// <param name="maxValue">Valor máximo (inclusive).</param>
        /// <returns>Número entero aleatorio.</returns>
        public int GenerateNumber(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue + 1);
        }

        /// <summary>
        /// Genera un número decimal aleatorio entre los valores mínimo y máximo.
        /// </summary>
        /// <param name="minValue">Valor mínimo (inclusive).</param>
        /// <param name="maxValue">Valor máximo (exclusive).</param>
        /// <returns>Número decimal aleatorio.</returns>
        public double GenerateDouble(double minValue, double maxValue)
        {
            return minValue + (_random.NextDouble() * (maxValue - minValue));
        }
    }

    /// <summary>
    /// Interfaz para el servicio de generación de números aleatorios.
    /// </summary>
    public interface IRandomNumberService
    {
        /// <summary>
        /// Genera un número entero aleatorio entre los valores mínimo y máximo (inclusive).
        /// </summary>
        /// <param name="minValue">Valor mínimo (inclusive).</param>
        /// <param name="maxValue">Valor máximo (inclusive).</param>
        /// <returns>Número entero aleatorio.</returns>
        int GenerateNumber(int minValue, int maxValue);

        /// <summary>
        /// Genera un número decimal aleatorio entre los valores mínimo y máximo.
        /// </summary>
        /// <param name="minValue">Valor mínimo (inclusive).</param>
        /// <param name="maxValue">Valor máximo (exclusive).</param>
        /// <returns>Número decimal aleatorio.</returns>
        double GenerateDouble(double minValue, double maxValue);
    }
}