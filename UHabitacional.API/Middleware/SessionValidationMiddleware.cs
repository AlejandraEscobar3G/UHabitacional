using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text.Json;
using UHabitacional.API.Application.Interfaces.Services;

namespace UHabitacional.API.Middleware;

/// <summary>
/// Middleware que se ejecuta DESPUÉS de UseAuthentication() y valida que el JWT
/// del usuario corresponda a una sesión activa y no expirada registrada en BD.
///
/// Si el token está expirado o la sesión fue cerrada (logout), responde 401.
/// </summary>
public class SessionValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<SessionValidationMiddleware> _logger;

    /// <summary>
    /// Rutas que NO requieren validación de sesión (login, swagger, etc.).
    /// </summary>
    private static readonly string[] RutasIgnoradas = new[]
    {
        "/api/auth/login",
        "/swagger",
        "/health"
    };

    public SessionValidationMiddleware(RequestDelegate next, ILogger<SessionValidationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, ISesionService sesionService)
    {
        var path = context.Request.Path.Value ?? string.Empty;

        // Saltar validación para rutas públicas
        if (RutasIgnoradas.Any(r => path.StartsWith(r, StringComparison.OrdinalIgnoreCase)))
        {
            await _next(context);
            return;
        }

        // Si el usuario no está autenticado por JWT (anónimo), dejar que el pipeline
        // de autorización maneje el caso (devolverá 401 si la ruta lo requiere).
        if (context.User?.Identity?.IsAuthenticated != true)
        {
            await _next(context);
            return;
        }

        // Buscar el JTI del JWT actual
        var jti = context.User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value
                  ?? context.User.FindFirst("jti")?.Value;

        if (string.IsNullOrEmpty(jti))
        {
            await EscribirRespuestaNoAutorizadaAsync(context,
                "El token no contiene un identificador de sesión (jti).");
            return;
        }

        // Validar la sesión contra BD (activa, no expirada) y refrescar last activity
        var sesion = await sesionService.ValidarYRefrescarAsync(jti, context.RequestAborted);
        if (sesion == null)
        {
            _logger.LogWarning("Acceso denegado: sesión no válida o expirada para JTI {Jti}.", jti);
            await EscribirRespuestaNoAutorizadaAsync(context,
                "La sesión no está activa o el token ha expirado. Inicie sesión de nuevo.");
            return;
        }

        await _next(context);
    }

    private static async Task EscribirRespuestaNoAutorizadaAsync(HttpContext context, string mensaje)
    {
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        context.Response.ContentType = "application/problem+json";

        var payload = new
        {
            title = "Sesión no válida",
            status = (int)HttpStatusCode.Unauthorized,
            detail = mensaje
        };

        var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}
