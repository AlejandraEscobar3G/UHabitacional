using UHabitacional.API.Domain.Entities;

namespace UHabitacional.API.Application.Interfaces.Repositories;

public interface IBitacoraVigilanteRepository
{
    Task<IEnumerable<BitacoraVigilante>> GetAllAsync(CancellationToken ct = default);
    Task<BitacoraVigilante?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<BitacoraVigilante> AddAsync(BitacoraVigilante entity, CancellationToken ct = default);
    Task UpdateAsync(BitacoraVigilante entity, CancellationToken ct = default);
}
