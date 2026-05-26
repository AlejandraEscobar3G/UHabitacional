using UHabitacional.API.Domain.Entities;

namespace UHabitacional.API.Application.Interfaces.Repositories;

public interface ISesionRepository
{
    Task<Sesion?> GetByJtiAsync(string jti, CancellationToken ct = default);
    Task<Sesion?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<Sesion>> GetActivasByUsuarioAsync(int idUsuario, CancellationToken ct = default);
    Task<Sesion> AddAsync(Sesion entity, CancellationToken ct = default);
    Task UpdateAsync(Sesion entity, CancellationToken ct = default);
    Task RevokeAsync(Sesion entity, CancellationToken ct = default);
    Task RevokeAllByUsuarioAsync(int idUsuario, CancellationToken ct = default);
}
