using UHabitacional.API.Domain.Entities;

namespace UHabitacional.API.Application.Interfaces.Repositories;

public interface IBitacoraVisitanteRepository
{
    Task<IEnumerable<BitacoraVisitante>> GetAllAsync(CancellationToken ct = default);
    Task<BitacoraVisitante?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<BitacoraVisitante?> GetByCodigoAsync(string codigo, CancellationToken ct = default);
    Task<BitacoraVisitante> AddAsync(BitacoraVisitante entity, CancellationToken ct = default);
    Task UpdateAsync(BitacoraVisitante entity, CancellationToken ct = default);
    Task SoftDeleteAsync(BitacoraVisitante entity, CancellationToken ct = default);
}
