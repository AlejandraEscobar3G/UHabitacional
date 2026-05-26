using Microsoft.AspNetCore.Mvc;
using UHabitacional.API.Application.DTOs;
using UHabitacional.API.Application.Interfaces.Services;

namespace UHabitacional.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService) => _authService = authService;

    /// <summary>
    /// Autentica al usuario con correo y contraseña, crea una sesión en BD
    /// y devuelve un JWT con el JTI asociado.
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<LoginResponseDto>> Login(
        [FromBody] LoginDto dto,
        CancellationToken ct)
    {
        var direccionIP = HttpContext.Connection.RemoteIpAddress?.ToString();
        var userAgent = Request.Headers.UserAgent.ToString();

        var result = await _authService.LoginAsync(dto, direccionIP, userAgent, ct);
        return Ok(result);
    }
}
