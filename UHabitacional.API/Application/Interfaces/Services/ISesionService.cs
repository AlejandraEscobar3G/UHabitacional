using UHabitacional.API.Application.DTOs;
using UHabitacional.API.Domain.Entities;

namespace UHabitacional.API.Application.Interfaces.Services;

public interface ISesionService
{
    /// <summary>
    /// Crea y persiste una sesión asociada a un usuario al momento del login.
    /// </summary>
    Task<Sesion> CrearAsync(int idUsuario, string jti, DateTime fechaExpiracion,
        string? direccionIP, string? userAgent, CancellationToken ct = default);

    /// <summary>
    /// Devuelve la sesión asociada al JTI si está activa y no expirada.
    /// </summary>
    Task<Sesion?> ValidarYRefrescarAsync(string jti, CancellationToken ct = default);

    /// <summary>
    /// Devuelve todas las sesiones activas del usuario autenticado actual.
    /// </summary>
    Task<IEnumerable<SesionDto>> GetActivasDelUsuarioActualAsync(CancellationToken ct = default);

    /// <summary>
    /// Cierra la sesión actual del usuario autenticado.
    /// </summary>
    Task<LogoutResponseDto> CerrarSesionActualAsync(CancellationToken ct = default);

    /// <summary>
    /// Cierra todas las sesiones activas del usuario autenticado.
    /// </summary>
    Task<int> CerrarTodasLasSesionesAsync(CancellationToken ct = default);
}
