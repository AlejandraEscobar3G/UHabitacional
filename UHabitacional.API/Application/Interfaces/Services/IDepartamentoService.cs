using UHabitacional.API.Application.DTOs;

namespace UHabitacional.API.Application.Interfaces.Services;

public interface IDepartamentoService
{
    Task<IEnumerable<DepartamentoDto>> GetAllAsync(CancellationToken ct = default);
    Task<DepartamentoDto> GetByIdAsync(int id, CancellationToken ct = default);
    Task<DepartamentoDto> CreateAsync(DepartamentoCreateDto dto, CancellationToken ct = default);
    Task<DepartamentoDto> UpdateAsync(int id, DepartamentoUpdateDto dto, CancellationToken ct = default);
}
