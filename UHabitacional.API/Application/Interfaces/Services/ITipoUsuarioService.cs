using UHabitacional.API.Application.DTOs;

namespace UHabitacional.API.Application.Interfaces.Services;

public interface ITipoUsuarioService
{
    Task<IEnumerable<TipoUsuarioDto>> GetAllAsync(CancellationToken ct = default);
    Task<TipoUsuarioDto> GetByIdAsync(int id, CancellationToken ct = default);
    Task<TipoUsuarioDto> CreateAsync(TipoUsuarioCreateDto dto, CancellationToken ct = default);
    Task<TipoUsuarioDto> UpdateAsync(int id, TipoUsuarioUpdateDto dto, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}
