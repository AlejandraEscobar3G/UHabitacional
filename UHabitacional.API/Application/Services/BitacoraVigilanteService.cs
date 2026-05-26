using UHabitacional.API.Application.DTOs;
using UHabitacional.API.Application.Interfaces.Repositories;
using UHabitacional.API.Application.Interfaces.Services;
using UHabitacional.API.Domain.Constants;
using UHabitacional.API.Domain.Entities;
using UHabitacional.API.Domain.Exceptions;

namespace UHabitacional.API.Application.Services;

public class BitacoraVigilanteService : IBitacoraVigilanteService
{
    private readonly IBitacoraVigilanteRepository _repo;
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly ICurrentUserService _currentUser;

    public BitacoraVigilanteService(
        IBitacoraVigilanteRepository repo,
        IUsuarioRepository usuarioRepo,
        ICurrentUserService currentUser)
    {
        _repo = repo;
        _usuarioRepo = usuarioRepo;
        _currentUser = currentUser;
    }

    public async Task<IEnumerable<BitacoraVigilanteDto>> GetAllAsync(CancellationToken ct = default)
    {
        // Permite Read a usuarios autenticados
        if (!_currentUser.IsAuthenticated)
            throw new ForbiddenOperationException("Debe estar autenticado para consultar la bitácora.");

        var entities = await _repo.GetAllAsync(ct);
        return entities.Select(MapToDto);
    }

    public async Task<BitacoraVigilanteDto> GetByIdAsync(int id, CancellationToken ct = default)
    {
        if (!_currentUser.IsAuthenticated)
            throw new ForbiddenOperationException("Debe estar autenticado para consultar la bitácora.");

        var entity = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException(nameof(BitacoraVigilante), id);
        return MapToDto(entity);
    }

    public async Task<BitacoraVigilanteDto> CreateAsync(BitacoraVigilanteCreateDto dto, CancellationToken ct = default)
    {
        EnsureVigilante();

        var idUsuario = _currentUser.IdUsuario
            ?? throw new ForbiddenOperationException("No se pudo identificar al usuario autenticado.");

        var vigilante = await _usuarioRepo.GetByIdAsync(idUsuario, ct)
            ?? throw new NotFoundException(nameof(Usuario), idUsuario);

        var entity = new BitacoraVigilante
        {
            IdUsuario = vigilante.IdUsuario,
            FechaHoraEntrada = dto.FechaHoraEntrada ?? DateTime.UtcNow,
            Observaciones = dto.Observaciones
        };

        await _repo.AddAsync(entity, ct);
        entity.Usuario = vigilante;
        return MapToDto(entity);
    }

    public async Task<BitacoraVigilanteDto> UpdateAsync(int id, BitacoraVigilanteUpdateDto dto, CancellationToken ct = default)
    {
        EnsureVigilante();

        var entity = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException(nameof(BitacoraVigilante), id);

        // Solo el mismo vigilante que creó la bitácora puede cerrarla
        if (entity.IdUsuario != _currentUser.IdUsuario)
            throw new ForbiddenOperationException("Solo el vigilante que abrió la bitácora puede actualizarla.");

        entity.FechaHoraSalida = dto.FechaHoraSalida ?? DateTime.UtcNow;
        if (dto.Observaciones != null)
            entity.Observaciones = dto.Observaciones;

        await _repo.UpdateAsync(entity, ct);
        return MapToDto(entity);
    }

    private void EnsureVigilante()
    {
        if (!_currentUser.IsInRole(RolesUsuario.Vigilante))
            throw new ForbiddenOperationException(
                $"Solo los usuarios '{RolesUsuario.Vigilante}' pueden registrar entradas/salidas de vigilancia.");
    }

    private static BitacoraVigilanteDto MapToDto(BitacoraVigilante b) =>
        new(b.IdBitacoraVigilante,
            b.IdUsuario,
            b.Usuario != null ? $"{b.Usuario.Nombre} {b.Usuario.Apellidos}" : null,
            b.FechaHoraEntrada,
            b.FechaHoraSalida,
            b.Observaciones);
}
