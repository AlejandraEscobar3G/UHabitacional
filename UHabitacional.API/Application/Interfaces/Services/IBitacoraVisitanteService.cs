using UHabitacional.API.Application.DTOs;

namespace UHabitacional.API.Application.Interfaces.Services;

public interface IBitacoraVisitanteService
{
    Task<IEnumerable<BitacoraVisitanteDto>> GetAllAsync(CancellationToken ct = default);
    Task<BitacoraVisitanteDto> GetByIdAsync(int id, CancellationToken ct = default);
    Task<BitacoraVisitanteDto> CreateAsync(BitacoraVisitanteCreateDto dto, CancellationToken ct = default);
    Task<BitacoraVisitanteDto> UpdateAsync(int id, BitacoraVisitanteUpdateDto dto, CancellationToken ct = default);
    Task<BitacoraVisitanteDto> RegistrarEntradaSalidaAsync(int id, BitacoraVisitanteRegistroDto dto, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}
