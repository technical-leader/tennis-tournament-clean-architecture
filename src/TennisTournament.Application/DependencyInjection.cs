using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace TennisTournament.Application
{
    /// <summary>
    /// Clase de extensión para configurar la inyección de dependencias de la capa de aplicación.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Configura los servicios de la capa de aplicación.
        /// </summary>
        /// <param name="services">Colección de servicios.</param>
        /// <returns>Colección de servicios actualizada.</returns>
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Registrar AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Registrar MediatR (usando la nueva sintaxis de MediatR 12.x)
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                // Opcional: Registrar comportamientos de pipeline
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            });

            // Registrar FluentValidation
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Registrar servicios de aplicación
            // services.AddScoped<IPlayerService, PlayerService>();
            // services.AddScoped<ITournamentService, TournamentService>();

            return services;
        }
    }

    /// <summary>
    /// Comportamiento de validación para MediatR.
    /// </summary>
    /// <typeparam name="TRequest">Tipo de solicitud.</typeparam>
    /// <typeparam name="TResponse">Tipo de respuesta.</typeparam>
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        /// <summary>
        /// Constructor con inyección de validadores.
        /// </summary>
        /// <param name="validators">Validadores para el tipo de solicitud.</param>
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        /// <summary>
        /// Maneja la solicitud y realiza la validación.
        /// </summary>
        /// <param name="request">Solicitud a validar.</param>
        /// <param name="next">Siguiente delegado en el pipeline.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Respuesta del siguiente delegado.</returns>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var validationResults = await Task.WhenAll(
                    _validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                var failures = validationResults
                    .SelectMany(r => r.Errors)
                    .Where(f => f != null)
                    .ToList();

                if (failures.Count != 0)
                {
                    throw new ValidationException(failures);
                }
            }

            return await next();
        }
    }
}