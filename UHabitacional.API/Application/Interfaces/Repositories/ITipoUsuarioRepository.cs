using UHabitacional.API.Domain.Entities;

namespace UHabitacional.API.Application.Interfaces.Repositories;

public interface ITipoUsuarioRepository
{
    Task<IEnumerable<TipoUsuario>> GetAllAsync(CancellationToken ct = default);
    Task<TipoUsuario?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<TipoUsuario?> GetByNameAsync(string nombre, CancellationToken ct = default);
    Task<TipoUsuario> AddAsync(TipoUsuario entity, CancellationToken ct = default);
    Task UpdateAsync(TipoUsuario entity, CancellationToken ct = default);
    Task SoftDeleteAsync(TipoUsuario entity, CancellationToken ct = default);
    Task<bool> ExistsByNameAsync(string nombre, int? excludeId = null, CancellationToken ct = default);
}
