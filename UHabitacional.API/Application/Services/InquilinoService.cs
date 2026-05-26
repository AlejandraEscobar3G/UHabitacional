using UHabitacional.API.Application.DTOs;
using UHabitacional.API.Application.Interfaces.Repositories;
using UHabitacional.API.Application.Interfaces.Services;
using UHabitacional.API.Domain.Constants;
using UHabitacional.API.Domain.Entities;
using UHabitacional.API.Domain.Exceptions;

namespace UHabitacional.API.Application.Services;

public class InquilinoService : IInquilinoService
{
    private readonly IInquilinoRepository _repo;
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly IDepartamentoRepository _deptoRepo;
    private readonly ITipoUsuarioRepository _tipoRepo;
    private readonly ICurrentUserService _currentUser;

    public InquilinoService(
        IInquilinoRepository repo,
        IUsuarioRepository usuarioRepo,
        IDepartamentoRepository deptoRepo,
        ITipoUsuarioRepository tipoRepo,
        ICurrentUserService currentUser)
    {
        _repo = repo;
        _usuarioRepo = usuarioRepo;
        _deptoRepo = deptoRepo;
        _tipoRepo = tipoRepo;
        _currentUser = currentUser;
    }

    public async Task<IEnumerable<InquilinoDto>> GetAllAsync(CancellationToken ct = default)
    {
        var entities = await _repo.GetAllAsync(ct);
        return entities.Select(MapToDto);
    }

    public async Task<InquilinoDto> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException(nameof(Inquilino), id);
        return MapToDto(entity);
    }

    public async Task<InquilinoDto> CreateAsync(InquilinoCreateDto dto, CancellationToken ct = default)
    {
        EnsureAdmin();

        // Validar Usuario existe y es de tipo Inquilino
        var usuario = await _usuarioRepo.GetByIdAsync(dto.IdUsuario, ct)
            ?? throw new NotFoundException(nameof(Usuario), dto.IdUsuario);

        var tipoInquilino = await _tipoRepo.GetByNameAsync(RolesUsuario.Inquilino, ct)
            ?? throw new BusinessRuleException("No existe el tipo de usuario 'Inquilino'.");

        if (usuario.IdTipoUsuario != tipoInquilino.IdTipoUsuario)
            throw new BusinessRuleException(
                $"El usuario debe ser de tipo '{RolesUsuario.Inquilino}' para asignarlo como inquilino.");

        // No puede tener otra asignación previa
        var existente = await _repo.GetByUsuarioIdAsync(dto.IdUsuario, ct);
        if (existente != null)
            throw new BusinessRuleException(
                $"El usuario ya está asignado como inquilino (Id={existente.IdInquilino}).");

        // Validar departamento
        var depto = await _deptoRepo.GetByIdAsync(dto.IdDepartamento, ct)
            ?? throw new NotFoundException(nameof(Departamento), dto.IdDepartamento);

        // El departamento no puede tener un inquilino activo
        var inquilinoActivoDepto = await _repo.GetActivoByDepartamentoAsync(dto.IdDepartamento, ct);
        if (inquilinoActivoDepto != null)
            throw new BusinessRuleException(
                $"El departamento '{depto.NumeroDepartamento}' ya tiene un inquilino activo asignado.");

        var entity = new Inquilino
        {
            IdUsuario = dto.IdUsuario,
            IdDepartamento = dto.IdDepartamento,
            FechaInicio = dto.FechaInicio ?? DateTime.UtcNow,
            FechaFin = null,
            Activo = true
        };

        await _repo.AddAsync(entity, ct);
        return MapToDto(await _repo.GetByIdAsync(entity.IdInquilino, ct) ?? entity);
    }

    public async Task<InquilinoDto> UpdateAsync(int id, InquilinoUpdateDto dto, CancellationToken ct = default)
    {
        EnsureAdmin();

        var entity = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException(nameof(Inquilino), id);

        // Solo se puede asignar a un departamento si el inquilino está Activo
        if (entity.FechaFin != null)
            throw new BusinessRuleException("Solo inquilinos en estado 'Activo' pueden ser reasignados a un departamento.");

        var depto = await _deptoRepo.GetByIdAsync(dto.IdDepartamento, ct)
            ?? throw new NotFoundException(nameof(Departamento), dto.IdDepartamento);

        // Si cambia de departamento, el nuevo no debe tener un inquilino activo
        if (entity.IdDepartamento != dto.IdDepartamento)
        {
            var inquilinoActivoDepto = await _repo.GetActivoByDepartamentoAsync(dto.IdDepartamento, ct);
            if (inquilinoActivoDepto != null && inquilinoActivoDepto.IdInquilino != id)
                throw new BusinessRuleException(
                    $"El departamento '{depto.NumeroDepartamento}' ya tiene un inquilino activo asignado.");
        }

        entity.IdDepartamento = dto.IdDepartamento;
        if (dto.FechaInicio.HasValue)
            entity.FechaInicio = dto.FechaInicio.Value;

        await _repo.UpdateAsync(entity, ct);
        return MapToDto(await _repo.GetByIdAsync(entity.IdInquilino, ct) ?? entity);
    }

    public async Task<InquilinoDto> UpdateFechaFinAsync(int id, InquilinoFechaFinDto dto, CancellationToken ct = default)
    {
        EnsureAdmin();

        var entity = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException(nameof(Inquilino), id);

        entity.FechaFin = dto.FechaFin;
        await _repo.UpdateAsync(entity, ct);
        return MapToDto(await _repo.GetByIdAsync(entity.IdInquilino, ct) ?? entity);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        EnsureAdmin();

        var entity = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException(nameof(Inquilino), id);

        await _repo.SoftDeleteAsync(entity, ct);

        // Cuando se elimina al inquilino, también se elimina al usuario asociado
        var usuario = await _usuarioRepo.GetByIdAsync(entity.IdUsuario, ct);
        if (usuario != null)
            await _usuarioRepo.SoftDeleteAsync(usuario, ct);
    }

    private void EnsureAdmin()
    {
        if (!_currentUser.IsInRole(RolesUsuario.Administrador))
            throw new ForbiddenOperationException(
                $"Solo los usuarios '{RolesUsuario.Administrador}' pueden gestionar a los Inquilinos.");
    }

    private static InquilinoDto MapToDto(Inquilino i) =>
        new(i.IdInquilino,
            i.IdUsuario,
            i.Usuario != null ? $"{i.Usuario.Nombre} {i.Usuario.Apellidos}" : null,
            i.Usuario?.Email,
            i.IdDepartamento,
            i.Departamento?.NumeroDepartamento,
            i.Departamento?.Edificio?.Nombre,
            i.FechaInicio,
            i.FechaFin,
            i.FechaFin == null);
}
