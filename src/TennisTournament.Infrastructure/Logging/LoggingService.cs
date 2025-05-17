using System;
using Microsoft.Extensions.Logging;

namespace TennisTournament.Infrastructure.Logging
{
    /// <summary>
    /// Servicio de logging para la aplicación.
    /// </summary>
    public class LoggingService : ILoggingService
    {
        private readonly ILogger<LoggingService> _logger;

        /// <summary>
        /// Constructor con inyección del logger.
        /// </summary>
        /// <param name="logger">Logger de Microsoft.Extensions.Logging.</param>
        public LoggingService(ILogger<LoggingService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Registra un mensaje de información.
        /// </summary>
        /// <param name="message">Mensaje a registrar.</param>
        public void LogInformation(string message)
        {
            _logger.LogInformation(message, Array.Empty<object>());
        }

        /// <summary>
        /// Registra un mensaje de advertencia.
        /// </summary>
        /// <param name="message">Mensaje a registrar.</param>
        public void LogWarning(string message)
        {
            _logger.LogWarning(message);
        }

        /// <summary>
        /// Registra un mensaje de error.
        /// </summary>
        /// <param name="message">Mensaje a registrar.</param>
        /// <param name="exception">Excepción asociada al error (opcional).</param>
        public void LogError(string message, Exception? exception = null)
        {
            if (exception != null)
                _logger.LogError(exception, message, Array.Empty<object>());
            else
                _logger.LogError(message);
        }
    }

    /// <summary>
    /// Interfaz para el servicio de logging.
    /// </summary>
    public interface ILoggingService
    {
        /// <summary>
        /// Registra un mensaje de información.
        /// </summary>
        /// <param name="message">Mensaje a registrar.</param>
        void LogInformation(string message);

        /// <summary>
        /// Registra un mensaje de advertencia.
        /// </summary>
        /// <param name="message">Mensaje a registrar.</param>
        void LogWarning(string message);

        /// <summary>
        /// Registra un mensaje de error.
        /// </summary>
        /// <param name="message">Mensaje a registrar.</param>
        /// <param name="exception">Excepción asociada al error (opcional).</param>
        void LogError(string message, Exception? exception = null);
    }
}