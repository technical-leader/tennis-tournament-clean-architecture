using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using TennisTournament.API;
using TennisTournament.Infrastructure.Data;
using TennisTournament.Infrastructure;

namespace TennisTournament.Tests.Integration.Framework;

/// <summary>
/// Fábrica personalizada para configurar el entorno de pruebas de integración.
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
  protected override void ConfigureWebHost(IWebHostBuilder builder)
  {
    builder.ConfigureServices(services =>
    {
      var sp = services.BuildServiceProvider();

      using (var scope = sp.CreateScope())
      {
        var scopedServices = scope.ServiceProvider;
        var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory>>();

        try
        {
          var context = scopedServices.GetRequiredService<TournamentDbContext>();

          // Asegurarse de que la base de datos esté creada
          context.Database.EnsureCreated();

          // Inicializar datos de prueba
          var loggerFactory = scopedServices.GetRequiredService<ILoggerFactory>();
          var seedLogger = loggerFactory.CreateLogger<TournamentDbContextSeed>();
          TournamentDbContextSeed.SeedAsync(context, seedLogger).Wait();
        }
        catch (Exception ex)
        {
          logger.LogError(ex, "Ocurrió un error al configurar el entorno de pruebas.");
        }
      }
    });
  }
}

