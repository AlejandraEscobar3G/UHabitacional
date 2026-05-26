using UHabitacional.API.Application.DTOs;

namespace UHabitacional.API.Application.Interfaces.Services;

public interface IEdificioService
{
    Task<IEnumerable<EdificioDto>> GetAllAsync(CancellationToken ct = default);
    Task<EdificioDto> GetByIdAsync(int id, CancellationToken ct = default);
    Task<EdificioDto> CreateAsync(EdificioCreateDto dto, CancellationToken ct = default);
    Task<EdificioDto> UpdateAsync(int id, EdificioUpdateDto dto, CancellationToken ct = default);
}
