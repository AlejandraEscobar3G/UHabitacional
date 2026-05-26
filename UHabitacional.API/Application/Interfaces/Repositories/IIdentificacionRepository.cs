using UHabitacional.API.Domain.Entities;

namespace UHabitacional.API.Application.Interfaces.Repositories;

public interface IIdentificacionRepository
{
    Task<IEnumerable<Identificacion>> GetAllAsync(CancellationToken ct = default);
    Task<Identificacion?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Identificacion> AddAsync(Identificacion entity, CancellationToken ct = default);
    Task UpdateAsync(Identificacion entity, CancellationToken ct = default);
    Task SoftDeleteAsync(Identificacion entity, CancellationToken ct = default);
    Task<bool> ExistsByNameAsync(string nombre, int? excludeId = null, CancellationToken ct = default);
}
