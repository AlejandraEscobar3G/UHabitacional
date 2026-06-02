using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UHabitacional.API.Application.Interfaces.Services;

namespace UHabitacional.API.Infrastructure.Security;

/// <summary>
/// Implementación de ICurrentUserService basada en HttpContext.
/// </summary>
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

    public int? IdUsuario
    {
        get
        {
            // Buscar en los claims clásicos y los registrados de JWT
            var claim =
                User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                ?? User?.FindFirst("sub")?.Value
                ?? User?.FindFirst("nameid")?.Value;

            return int.TryParse(claim, out var id) ? id : null;
        }
    }

    public string? Email =>
        User?.FindFirst(ClaimTypes.Email)?.Value
        ?? User?.FindFirst(JwtRegisteredClaimNames.Email)?.Value
        ?? User?.FindFirst("email")?.Value;

    public string? Rol =>
        User?.FindFirst(ClaimTypes.Role)?.Value
        ?? User?.FindFirst("role")?.Value
        ?? User?.FindFirst("roles")?.Value;

    public string? Jti =>
        User?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value
        ?? User?.FindFirst("jti")?.Value;

    /// <summary>
    /// Verifica si el usuario tiene el rol indicado.
    /// Primero usa <see cref="ClaimsPrincipal.IsInRole"/> (que respeta el
    /// RoleClaimType de la identidad), luego compara contra los claims
    /// clásicos como fallback.
    /// </summary>
    public bool IsInRole(string rol)
    {
        if (!IsAuthenticated || User is null) return false;

        if (User.IsInRole(rol)) return true;

        // Fallback: comparar contra cualquier claim que represente el rol
        return string.Equals(Rol, rol, StringComparison.OrdinalIgnoreCase);
    }
}
