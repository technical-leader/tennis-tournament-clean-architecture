using Microsoft.AspNetCore.Mvc;

namespace TennisTournament.API.Controllers;

/// <summary>
/// Controlador para verificar el estado de la aplicación.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    /// <summary>
    /// Verifica y devuelve el estado de la API.
    /// </summary>
    /// <returns>
    /// Un objeto JSON indicando el estado de la aplicación.
    /// Por ejemplo: { "status": "API en ejecución" }.
    /// </returns>
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { status = "API en ejecución" });
    }
}