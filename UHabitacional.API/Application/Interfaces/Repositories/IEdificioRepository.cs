using UHabitacional.API.Domain.Entities;

namespace UHabitacional.API.Application.Interfaces.Repositories;

public interface IEdificioRepository
{
    Task<IEnumerable<Edificio>> GetAllAsync(CancellationToken ct = default);
    Task<Edificio?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Edificio> AddAsync(Edificio entity, CancellationToken ct = default);
    Task UpdateAsync(Edificio entity, CancellationToken ct = default);
    Task<bool> ExistsByNameAsync(string nombre, int? excludeId = null, CancellationToken ct = default);
    Task<int> CountDepartamentosAsync(int idEdificio, CancellationToken ct = default);
}
