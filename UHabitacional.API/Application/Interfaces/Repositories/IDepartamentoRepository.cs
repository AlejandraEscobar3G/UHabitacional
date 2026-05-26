using UHabitacional.API.Domain.Entities;

namespace UHabitacional.API.Application.Interfaces.Repositories;

public interface IDepartamentoRepository
{
    Task<IEnumerable<Departamento>> GetAllAsync(CancellationToken ct = default);
    Task<Departamento?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Departamento> AddAsync(Departamento entity, CancellationToken ct = default);
    Task UpdateAsync(Departamento entity, CancellationToken ct = default);
    Task<bool> HasInquilinoActivoAsync(int idDepartamento, CancellationToken ct = default);
    Task<bool> ExistsByNumeroAsync(int idEdificio, string numero, int? excludeId = null, CancellationToken ct = default);
}
