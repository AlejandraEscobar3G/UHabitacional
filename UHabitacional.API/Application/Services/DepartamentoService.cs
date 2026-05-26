using UHabitacional.API.Application.DTOs;
using UHabitacional.API.Application.Interfaces.Repositories;
using UHabitacional.API.Application.Interfaces.Services;
using UHabitacional.API.Domain.Constants;
using UHabitacional.API.Domain.Entities;
using UHabitacional.API.Domain.Exceptions;

namespace UHabitacional.API.Application.Services;

public class DepartamentoService : IDepartamentoService
{
    private readonly IDepartamentoRepository _repo;
    private readonly IEdificioRepository _edificioRepo;
    private readonly ICurrentUserService _currentUser;

    public DepartamentoService(
        IDepartamentoRepository repo,
        IEdificioRepository edificioRepo,
        ICurrentUserService currentUser)
    {
        _repo = repo;
        _edificioRepo = edificioRepo;
        _currentUser = currentUser;
    }

    public async Task<IEnumerable<DepartamentoDto>> GetAllAsync(CancellationToken ct = default)
    {
        var entities = await _repo.GetAllAsync(ct);
        return entities.Select(MapToDto);
    }

    public async Task<DepartamentoDto> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException(nameof(Departamento), id);
        return MapToDto(entity);
    }

    public async Task<DepartamentoDto> CreateAsync(DepartamentoCreateDto dto, CancellationToken ct = default)
    {
        EnsureAdmin();

        var edificio = await _edificioRepo.GetByIdAsync(dto.IdEdificio, ct)
            ?? throw new NotFoundException(nameof(Edificio), dto.IdEdificio);

        // Regla 1: cantidad de departamentos no puede exceder TotalDeptos
        var totalActual = await _edificioRepo.CountDepartamentosAsync(dto.IdEdificio, ct);
        if (totalActual >= edificio.TotalDeptos)
            throw new BusinessRuleException(
                $"El edificio '{edificio.Nombre}' ya alcanzó el total de departamentos permitidos ({edificio.TotalDeptos}).");

        // Regla 2: el piso no puede ser mayor al NumeroPisos del edificio
        if (dto.Piso > edificio.NumeroPisos)
            throw new BusinessRuleException(
                $"El piso {dto.Piso} es mayor al número de pisos del edificio '{edificio.Nombre}' ({edificio.NumeroPisos}).");

        if (await _repo.ExistsByNumeroAsync(dto.IdEdificio, dto.NumeroDepartamento, null, ct))
            throw new BusinessRuleException(
                $"Ya existe un departamento con número '{dto.NumeroDepartamento}' en el edificio '{edificio.Nombre}'.");

        var entity = new Departamento
        {
            IdEdificio = dto.IdEdificio,
            NumeroDepartamento = dto.NumeroDepartamento,
            Piso = dto.Piso
        };

        await _repo.AddAsync(entity, ct);
        entity.Edificio = edificio;
        return MapToDto(entity);
    }

    public async Task<DepartamentoDto> UpdateAsync(int id, DepartamentoUpdateDto dto, CancellationToken ct = default)
    {
        EnsureAdmin();

        var entity = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException(nameof(Departamento), id);

        var edificio = await _edificioRepo.GetByIdAsync(dto.IdEdificio, ct)
            ?? throw new NotFoundException(nameof(Edificio), dto.IdEdificio);

        // Si cambia de edificio, validar capacidad
        if (entity.IdEdificio != dto.IdEdificio)
        {
            var totalDeptosNuevoEdificio = await _edificioRepo.CountDepartamentosAsync(dto.IdEdificio, ct);
            if (totalDeptosNuevoEdificio >= edificio.TotalDeptos)
                throw new BusinessRuleException(
                    $"El edificio '{edificio.Nombre}' ya alcanzó el total de departamentos permitidos ({edificio.TotalDeptos}).");
        }

        if (dto.Piso > edificio.NumeroPisos)
            throw new BusinessRuleException(
                $"El piso {dto.Piso} es mayor al número de pisos del edificio '{edificio.Nombre}' ({edificio.NumeroPisos}).");

        if (await _repo.ExistsByNumeroAsync(dto.IdEdificio, dto.NumeroDepartamento, id, ct))
            throw new BusinessRuleException(
                $"Ya existe otro departamento con número '{dto.NumeroDepartamento}' en el edificio '{edificio.Nombre}'.");

        entity.IdEdificio = dto.IdEdificio;
        entity.NumeroDepartamento = dto.NumeroDepartamento;
        entity.Piso = dto.Piso;

        await _repo.UpdateAsync(entity, ct);
        entity.Edificio = edificio;
        return MapToDto(entity);
    }

    private void EnsureAdmin()
    {
        if (!_currentUser.IsInRole(RolesUsuario.Administrador))
            throw new ForbiddenOperationException(
                $"Solo los usuarios '{RolesUsuario.Administrador}' pueden gestionar el catálogo de Departamentos.");
    }

    private static DepartamentoDto MapToDto(Departamento d) =>
        new(d.IdDepartamento, d.IdEdificio, d.Edificio?.Nombre, d.NumeroDepartamento, d.Piso);
}
