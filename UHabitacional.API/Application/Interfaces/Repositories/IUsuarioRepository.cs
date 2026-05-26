using UHabitacional.API.Domain.Entities;

namespace UHabitacional.API.Application.Interfaces.Repositories;

public interface IUsuarioRepository
{
    Task<IEnumerable<Usuario>> GetAllAsync(CancellationToken ct = default);
    Task<Usuario?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Usuario?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<Usuario> AddAsync(Usuario entity, CancellationToken ct = default);
    Task UpdateAsync(Usuario entity, CancellationToken ct = default);
    Task SoftDeleteAsync(Usuario entity, CancellationToken ct = default);
    Task<bool> ExistsByEmailAsync(string email, int? excludeId = null, CancellationToken ct = default);
}
