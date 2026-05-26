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
            var claim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(claim, out var id) ? id : null;
        }
    }

    public string? Email => User?.FindFirst(ClaimTypes.Email)?.Value;

    public string? Rol => User?.FindFirst(ClaimTypes.Role)?.Value;

    public string? Jti =>
        User?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value
        ?? User?.FindFirst("jti")?.Value;

    public bool IsInRole(string rol) =>
        IsAuthenticated && string.Equals(Rol, rol, StringComparison.OrdinalIgnoreCase);
}
