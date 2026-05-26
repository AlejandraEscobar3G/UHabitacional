using UHabitacional.API.Application.DTOs;
using UHabitacional.API.Application.Interfaces.Repositories;
using UHabitacional.API.Application.Interfaces.Services;
using UHabitacional.API.Domain.Constants;
using UHabitacional.API.Domain.Entities;
using UHabitacional.API.Domain.Exceptions;

namespace UHabitacional.API.Application.Services;

public class EdificioService : IEdificioService
{
    private readonly IEdificioRepository _repo;
    private readonly ICurrentUserService _currentUser;

    public EdificioService(IEdificioRepository repo, ICurrentUserService currentUser)
    {
        _repo = repo;
        _currentUser = currentUser;
    }

    public async Task<IEnumerable<EdificioDto>> GetAllAsync(CancellationToken ct = default)
    {
        var entities = await _repo.GetAllAsync(ct);
        return entities.Select(MapToDto);
    }

    public async Task<EdificioDto> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException(nameof(Edificio), id);
        return MapToDto(entity);
    }

    public async Task<EdificioDto> CreateAsync(EdificioCreateDto dto, CancellationToken ct = default)
    {
        EnsureAdmin();

        if (await _repo.ExistsByNameAsync(dto.Nombre, null, ct))
            throw new BusinessRuleException($"Ya existe un edificio con el nombre '{dto.Nombre}'.");

        var entity = new Edificio
        {
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            NumeroPisos = dto.NumeroPisos,
            TotalDeptos = dto.TotalDeptos
        };

        await _repo.AddAsync(entity, ct);
        return MapToDto(entity);
    }

    public async Task<EdificioDto> UpdateAsync(int id, EdificioUpdateDto dto, CancellationToken ct = default)
    {
        EnsureAdmin();

        var entity = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException(nameof(Edificio), id);

        if (await _repo.ExistsByNameAsync(dto.Nombre, id, ct))
            throw new BusinessRuleException($"Ya existe otro edificio con el nombre '{dto.Nombre}'.");

        // Verificación: no se puede reducir TotalDeptos/NumeroPisos a un valor menor del actual
        var currentDeptosCount = await _repo.CountDepartamentosAsync(id, ct);
        if (dto.TotalDeptos < currentDeptosCount)
            throw new BusinessRuleException(
                $"No se puede establecer TotalDeptos en {dto.TotalDeptos} porque ya hay {currentDeptosCount} departamento(s) registrado(s).");

        entity.Nombre = dto.Nombre;
        entity.Descripcion = dto.Descripcion;
        entity.NumeroPisos = dto.NumeroPisos;
        entity.TotalDeptos = dto.TotalDeptos;

        await _repo.UpdateAsync(entity, ct);
        return MapToDto(entity);
    }

    private void EnsureAdmin()
    {
        if (!_currentUser.IsInRole(RolesUsuario.Administrador))
            throw new ForbiddenOperationException(
                $"Solo los usuarios '{RolesUsuario.Administrador}' pueden gestionar el catálogo de Edificios.");
    }

    private static EdificioDto MapToDto(Edificio e) =>
        new(e.IdEdificio, e.Nombre, e.Descripcion, e.NumeroPisos, e.TotalDeptos);
}
