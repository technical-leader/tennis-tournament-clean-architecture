using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TennisTournament.Domain.Interfaces;
using TennisTournament.Domain.Services;
using TennisTournament.Infrastructure.Data;
using TennisTournament.Infrastructure.Data.Repositories;
using TennisTournament.Infrastructure.Logging;

namespace TennisTournament.Infrastructure
{
    /// <summary>
    /// Clase de extensión para registrar los servicios de infraestructura.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Registra los servicios de infraestructura en el contenedor de dependencias.
        /// </summary>
        /// <param name="services">Colección de servicios.</param>
        /// <param name="configuration">Configuración de la aplicación.</param>
        /// <returns>Colección de servicios actualizada.</returns>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Registrar repositorios
            services.AddScoped<IPlayerRepository, PlayerRepository>();
            services.AddScoped<ITournamentRepository, TournamentRepository>();
            services.AddScoped<IMatchRepository, MatchRepository>();
            services.AddScoped<IResultRepository, ResultRepository>();

            // Registrar servicios de dominio
            services.AddScoped<MatchSimulationStrategyFactory>();
            services.AddScoped<TournamentSimulationService>();

            // Registrar servicios de infraestructura
            services.AddScoped<ILoggingService, LoggingService>();

            // Registrar health checks
            services.AddHealthChecks()
                .AddDbContextCheck<TournamentDbContext>();

            return services;
        }
    }
}