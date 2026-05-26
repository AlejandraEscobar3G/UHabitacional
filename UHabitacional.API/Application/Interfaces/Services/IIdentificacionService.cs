using UHabitacional.API.Application.DTOs;

namespace UHabitacional.API.Application.Interfaces.Services;

public interface IIdentificacionService
{
    Task<IEnumerable<IdentificacionDto>> GetAllAsync(CancellationToken ct = default);
    Task<IdentificacionDto> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IdentificacionDto> CreateAsync(IdentificacionCreateDto dto, CancellationToken ct = default);
    Task<IdentificacionDto> UpdateAsync(int id, IdentificacionUpdateDto dto, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}
