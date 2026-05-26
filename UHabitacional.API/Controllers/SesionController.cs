using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UHabitacional.API.Application.DTOs;
using UHabitacional.API.Application.Interfaces.Services;

namespace UHabitacional.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
public class SesionController : ControllerBase
{
    private readonly ISesionService _service;

    public SesionController(ISesionService service) => _service = service;

    /// <summary>
    /// Devuelve todas las sesiones activas del usuario autenticado.
    /// La sesión actual viene marcada con EsSesionActual = true.
    /// </summary>
    [HttpGet("activas")]
    [ProducesResponseType(typeof(IEnumerable<SesionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<SesionDto>>> GetActivas(CancellationToken ct) =>
        Ok(await _service.GetActivasDelUsuarioActualAsync(ct));

    /// <summary>
    /// Cierra la sesión actual (logout). El JWT seguirá pareciendo válido por
    /// criptografía hasta que expire, pero el middleware lo rechazará porque la
    /// sesión queda marcada como inactiva en BD.
    /// </summary>
    [HttpPost("logout")]
    [ProducesResponseType(typeof(LogoutResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LogoutResponseDto>> Logout(CancellationToken ct) =>
        Ok(await _service.CerrarSesionActualAsync(ct));

    /// <summary>
    /// Cierra todas las sesiones activas del usuario (incluyendo la actual).
    /// Útil cuando el usuario cree que sus credenciales fueron comprometidas.
    /// </summary>
    [HttpPost("logout-todas")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> LogoutTodas(CancellationToken ct)
    {
        var cerradas = await _service.CerrarTodasLasSesionesAsync(ct);
        return Ok(new { sesionesCerradas = cerradas, mensaje = $"Se cerraron {cerradas} sesión(es) activas." });
    }
}
