using UHabitacional.API.Application.DTOs;
using UHabitacional.API.Application.Interfaces.Repositories;
using UHabitacional.API.Application.Interfaces.Services;
using UHabitacional.API.Domain.Constants;
using UHabitacional.API.Domain.Entities;
using UHabitacional.API.Domain.Exceptions;

namespace UHabitacional.API.Application.Services;

public class IdentificacionService : IIdentificacionService
{
    private readonly IIdentificacionRepository _repo;
    private readonly ICurrentUserService _currentUser;

    public IdentificacionService(IIdentificacionRepository repo, ICurrentUserService currentUser)
    {
        _repo = repo;
        _currentUser = currentUser;
    }

    public async Task<IEnumerable<IdentificacionDto>> GetAllAsync(CancellationToken ct = default)
    {
        var entities = await _repo.GetAllAsync(ct);
        return entities.Select(MapToDto);
    }

    public async Task<IdentificacionDto> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException(nameof(Identificacion), id);
        return MapToDto(entity);
    }

    public async Task<IdentificacionDto> CreateAsync(IdentificacionCreateDto dto, CancellationToken ct = default)
    {
        EnsureAdmin();

        if (await _repo.ExistsByNameAsync(dto.Nombre, null, ct))
            throw new BusinessRuleException($"Ya existe una identificación con el nombre '{dto.Nombre}'.");

        var entity = new Identificacion
        {
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            Activo = true
        };

        await _repo.AddAsync(entity, ct);
        return MapToDto(entity);
    }

    public async Task<IdentificacionDto> UpdateAsync(int id, IdentificacionUpdateDto dto, CancellationToken ct = default)
    {
        EnsureAdmin();

        var entity = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException(nameof(Identificacion), id);

        if (await _repo.ExistsByNameAsync(dto.Nombre, id, ct))
            throw new BusinessRuleException($"Ya existe otra identificación con el nombre '{dto.Nombre}'.");

        entity.Nombre = dto.Nombre;
        entity.Descripcion = dto.Descripcion;

        await _repo.UpdateAsync(entity, ct);
        return MapToDto(entity);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        EnsureAdmin();

        var entity = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException(nameof(Identificacion), id);

        await _repo.SoftDeleteAsync(entity, ct);
    }

    private void EnsureAdmin()
    {
        if (!_currentUser.IsInRole(RolesUsuario.Administrador))
            throw new ForbiddenOperationException(
                $"Solo los usuarios '{RolesUsuario.Administrador}' pueden gestionar el catálogo de Identificaciones.");
    }

    private static IdentificacionDto MapToDto(Identificacion e) =>
        new(e.IdIdentificacion, e.Nombre, e.Descripcion, e.Activo);
}
