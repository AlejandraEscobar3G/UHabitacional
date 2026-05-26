using UHabitacional.API.Application.DTOs;

namespace UHabitacional.API.Application.Interfaces.Services;

public interface IUsuarioService
{
    Task<IEnumerable<UsuarioDto>> GetAllAsync(CancellationToken ct = default);
    Task<UsuarioDto> GetByIdAsync(int id, CancellationToken ct = default);
    Task<UsuarioDto> CreateAsync(UsuarioCreateDto dto, CancellationToken ct = default);
    Task<UsuarioDto> UpdateAsync(int id, UsuarioUpdateDto dto, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}
