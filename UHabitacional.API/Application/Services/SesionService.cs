using UHabitacional.API.Application.DTOs;
using UHabitacional.API.Application.Interfaces.Repositories;
using UHabitacional.API.Application.Interfaces.Services;
using UHabitacional.API.Domain.Entities;
using UHabitacional.API.Domain.Exceptions;

namespace UHabitacional.API.Application.Services;

public class SesionService : ISesionService
{
    private readonly ISesionRepository _repo;
    private readonly ICurrentUserService _currentUser;

    public SesionService(ISesionRepository repo, ICurrentUserService currentUser)
    {
        _repo = repo;
        _currentUser = currentUser;
    }

    public async Task<Sesion> CrearAsync(int idUsuario, string jti, DateTime fechaExpiracion,
        string? direccionIP, string? userAgent, CancellationToken ct = default)
    {
        var entity = new Sesion
        {
            IdUsuario = idUsuario,
            Jti = jti,
            FechaInicio = DateTime.UtcNow,
            FechaExpiracion = fechaExpiracion,
            FechaUltimaActividad = DateTime.UtcNow,
            DireccionIP = direccionIP,
            UserAgent = userAgent,
            Activa = true
        };

        return await _repo.AddAsync(entity, ct);
    }

    public async Task<Sesion?> ValidarYRefrescarAsync(string jti, CancellationToken ct = default)
    {
        var sesion = await _repo.GetByJtiAsync(jti, ct);
        if (sesion == null) return null;

        // Si la sesión ya fue cerrada
        if (!sesion.Activa || sesion.FechaCierre != null)
            return null;

        // Si el JWT/sesión ya expiraron, marcar como inactiva
        if (sesion.FechaExpiracion <= DateTime.UtcNow)
        {
            sesion.Activa = false;
            sesion.FechaCierre = DateTime.UtcNow;
            await _repo.UpdateAsync(sesion, ct);
            return null;
        }

        // Actualizar last activity
        sesion.FechaUltimaActividad = DateTime.UtcNow;
        await _repo.UpdateAsync(sesion, ct);

        return sesion;
    }

    public async Task<IEnumerable<SesionDto>> GetActivasDelUsuarioActualAsync(CancellationToken ct = default)
    {
        var idUsuario = _currentUser.IdUsuario
            ?? throw new ForbiddenOperationException("No hay usuario autenticado.");

        var jtiActual = _currentUser.Jti;

        var sesiones = await _repo.GetActivasByUsuarioAsync(idUsuario, ct);
        return sesiones.Select(s => MapToDto(s, jtiActual));
    }

    public async Task<LogoutResponseDto> CerrarSesionActualAsync(CancellationToken ct = default)
    {
        var jti = _currentUser.Jti
            ?? throw new ForbiddenOperationException("No se pudo identificar la sesión actual.");

        var sesion = await _repo.GetByJtiAsync(jti, ct)
            ?? throw new NotFoundException("No se encontró una sesión activa para el token actual.");

        if (!sesion.Activa)
            throw new BusinessRuleException("La sesión ya está cerrada.");

        await _repo.RevokeAsync(sesion, ct);

        return new LogoutResponseDto(
            sesion.IdSesion,
            "Sesión cerrada exitosamente.",
            sesion.FechaCierre!.Value);
    }

    public async Task<int> CerrarTodasLasSesionesAsync(CancellationToken ct = default)
    {
        var idUsuario = _currentUser.IdUsuario
            ?? throw new ForbiddenOperationException("No hay usuario autenticado.");

        var activas = (await _repo.GetActivasByUsuarioAsync(idUsuario, ct)).Count();
        await _repo.RevokeAllByUsuarioAsync(idUsuario, ct);
        return activas;
    }

    private static SesionDto MapToDto(Sesion s, string? jtiActual) =>
        new(s.IdSesion,
            s.IdUsuario,
            s.Usuario != null ? $"{s.Usuario.Nombre} {s.Usuario.Apellidos}" : null,
            s.Jti,
            s.FechaInicio,
            s.FechaExpiracion,
            s.FechaUltimaActividad,
            s.FechaCierre,
            s.DireccionIP,
            s.UserAgent,
            s.Activa,
            jtiActual != null && s.Jti == jtiActual);
}
