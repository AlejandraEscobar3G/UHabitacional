namespace UHabitacional.API.Application.Interfaces.Services;

/// <summary>
/// Abstracción para obtener información del usuario autenticado actual.
/// </summary>
public interface ICurrentUserService
{
    int? IdUsuario { get; }
    string? Email { get; }
    string? Rol { get; }
    /// <summary>
    /// JWT ID (claim 'jti') del token recibido en la request actual.
    /// </summary>
    string? Jti { get; }
    bool IsAuthenticated { get; }
    bool IsInRole(string rol);
}
