using UHabitacional.API.Application.DTOs;

namespace UHabitacional.API.Application.Interfaces.Services;

public interface IBitacoraVigilanteService
{
    Task<IEnumerable<BitacoraVigilanteDto>> GetAllAsync(CancellationToken ct = default);
    Task<BitacoraVigilanteDto> GetByIdAsync(int id, CancellationToken ct = default);
    Task<BitacoraVigilanteDto> CreateAsync(BitacoraVigilanteCreateDto dto, CancellationToken ct = default);
    Task<BitacoraVigilanteDto> UpdateAsync(int id, BitacoraVigilanteUpdateDto dto, CancellationToken ct = default);
}
