using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TennisTournament.Application.DTOs;
using TennisTournament.Application.Mappings;
using TennisTournament.Application.Validators;
using TennisTournament.Domain.Interfaces;
using TennisTournament.Domain.Services;
using TennisTournament.Infrastructure.Data;
using TennisTournament.Infrastructure.Data.Repositories;
using TennisTournament.Application.Converters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.Converters.Add(new TennisTournament.Application.Converters.JsonPlayerDtoConverter());
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Tennis Tournament API",
        Version = "v1",
        Description = "API para la gestión de torneos de tenis"
    });

    // Set the comments path for the Swagger JSON and UI
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (System.IO.File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// Configurar FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreatePlayerCommandValidator>();
// Los validadores específicos para DTOs se registrarán automáticamente con AddValidatorsFromAssemblyContaining

// Configurar DbContext
builder.Services.AddDbContext<TournamentDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar repositorios
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<ITournamentRepository, TournamentRepository>();
builder.Services.AddScoped<IMatchRepository, MatchRepository>();
builder.Services.AddScoped<IResultRepository, ResultRepository>();

// Configurar servicios del dominio
builder.Services.AddScoped<MatchSimulationStrategyFactory>();
builder.Services.AddScoped<TournamentSimulationService>();

// Configurar AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Configurar MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(TennisTournament.Application.Commands.CreatePlayerCommand).Assembly);
});

// Configuración de CORS ajustada para producción
builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5000") // Ajustar según los orígenes permitidos
              .AllowAnyMethod()
              .AllowAnyHeader();

    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tennis Tournament API V1");
        c.RoutePrefix = string.Empty; // Para servir Swagger UI en la raíz de la aplicación

        // Inyectar código JavaScript para verificar el estado de la API
        string swaggerScript = @"
        window.addEventListener('load', function() {
            // Captura errores globalmente
            window.onerror = function(message, source, lineno, colno, error) {
                if (message && message.includes('Failed to fetch')) {
                    document.body.innerHTML = '<h2>Error: No se puede conectar a la API. Intente más tarde.</h2>';
                    return true;
                }
                return false;
            };
            
            // Verifica periódicamente el estado de la API
            setInterval(function() {
                fetch('/api/health')
                .then(response => { if (!response.ok) throw new Error('API en error'); })
                .catch(error => {
                    var message = 'Error: No se puede conectar a la API. Intente más tarde.';
                    if (error.message.includes('CORS')) {
                        message = 'Error: Configuración CORS incorrecta. Verifique permisos.';
                    }
                    document.body.innerHTML = '<h2>' + message + '</h2>';
                });
            }, 5000);
        });
        ";
        c.InjectJavascript($"data:text/javascript;charset=utf-8,{Uri.EscapeDataString(swaggerScript)}");
    });

    // Sembrar datos iniciales en la base de datos
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<TournamentDbContext>();
        var logger = services.GetRequiredService<ILogger<TournamentDbContextSeed>>();

        try
        {
            TournamentDbContextSeed.SeedAsync(context, logger).Wait();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al sembrar datos iniciales en la base de datos");
        }
    }
}

// Ejecutar migraciones automáticas al iniciar la app
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TournamentDbContext>();
    db.Database.Migrate(); // Aplica migraciones o crea la DB si no existe
}

app.UseCors("DefaultPolicy");

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
    // Lo declaro por si tengo pruebas de integración que dependen de Program
}