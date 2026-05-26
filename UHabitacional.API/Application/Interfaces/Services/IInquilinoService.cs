using UHabitacional.API.Application.DTOs;

namespace UHabitacional.API.Application.Interfaces.Services;

public interface IInquilinoService
{
    Task<IEnumerable<InquilinoDto>> GetAllAsync(CancellationToken ct = default);
    Task<InquilinoDto> GetByIdAsync(int id, CancellationToken ct = default);
    Task<InquilinoDto> CreateAsync(InquilinoCreateDto dto, CancellationToken ct = default);
    Task<InquilinoDto> UpdateAsync(int id, InquilinoUpdateDto dto, CancellationToken ct = default);
    Task<InquilinoDto> UpdateFechaFinAsync(int id, InquilinoFechaFinDto dto, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}
