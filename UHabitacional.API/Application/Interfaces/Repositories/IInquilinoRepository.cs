using UHabitacional.API.Domain.Entities;

namespace UHabitacional.API.Application.Interfaces.Repositories;

public interface IInquilinoRepository
{
    Task<IEnumerable<Inquilino>> GetAllAsync(CancellationToken ct = default);
    Task<Inquilino?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Inquilino?> GetByUsuarioIdAsync(int idUsuario, CancellationToken ct = default);
    Task<Inquilino?> GetActivoByDepartamentoAsync(int idDepartamento, CancellationToken ct = default);
    Task<Inquilino> AddAsync(Inquilino entity, CancellationToken ct = default);
    Task UpdateAsync(Inquilino entity, CancellationToken ct = default);
    Task SoftDeleteAsync(Inquilino entity, CancellationToken ct = default);
}
