using UHabitacional.API.Application.DTOs;
using UHabitacional.API.Application.Interfaces.Repositories;
using UHabitacional.API.Application.Interfaces.Services;
using UHabitacional.API.Domain.Constants;
using UHabitacional.API.Domain.Entities;
using UHabitacional.API.Domain.Exceptions;

namespace UHabitacional.API.Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _repo;
    private readonly ITipoUsuarioRepository _tipoRepo;
    private readonly IIdentificacionRepository _idRepo;
    private readonly ICurrentUserService _currentUser;

    public UsuarioService(
        IUsuarioRepository repo,
        ITipoUsuarioRepository tipoRepo,
        IIdentificacionRepository idRepo,
        ICurrentUserService currentUser)
    {
        _repo = repo;
        _tipoRepo = tipoRepo;
        _idRepo = idRepo;
        _currentUser = currentUser;
    }

    public async Task<IEnumerable<UsuarioDto>> GetAllAsync(CancellationToken ct = default)
    {
        var entities = await _repo.GetAllAsync(ct);
        return entities.Select(MapToDto);
    }

    public async Task<UsuarioDto> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException(nameof(Usuario), id);
        return MapToDto(entity);
    }

    public async Task<UsuarioDto> CreateAsync(UsuarioCreateDto dto, CancellationToken ct = default)
    {
        EnsureAdmin();

        var tipo = await _tipoRepo.GetByIdAsync(dto.IdTipoUsuario, ct)
            ?? throw new NotFoundException(nameof(TipoUsuario), dto.IdTipoUsuario);

        var identificacion = await _idRepo.GetByIdAsync(dto.IdIdentificacion, ct)
            ?? throw new NotFoundException(nameof(Identificacion), dto.IdIdentificacion);

        if (await _repo.ExistsByEmailAsync(dto.Email, null, ct))
            throw new BusinessRuleException($"Ya existe un usuario con el correo '{dto.Email}'.");

        var entity = new Usuario
        {
            IdTipoUsuario = dto.IdTipoUsuario,
            IdIdentificacion = dto.IdIdentificacion,
            NumeroIdentificacion = dto.NumeroIdentificacion,
            Nombre = dto.Nombre,
            Apellidos = dto.Apellidos,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Telefono = dto.Telefono,
            Activo = true
        };

        await _repo.AddAsync(entity, ct);
        entity.TipoUsuario = tipo;
        entity.Identificacion = identificacion;
        return MapToDto(entity);
    }

    public async Task<UsuarioDto> UpdateAsync(int id, UsuarioUpdateDto dto, CancellationToken ct = default)
    {
        EnsureAdmin();

        var entity = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException(nameof(Usuario), id);

        var tipo = await _tipoRepo.GetByIdAsync(dto.IdTipoUsuario, ct)
            ?? throw new NotFoundException(nameof(TipoUsuario), dto.IdTipoUsuario);

        var identificacion = await _idRepo.GetByIdAsync(dto.IdIdentificacion, ct)
            ?? throw new NotFoundException(nameof(Identificacion), dto.IdIdentificacion);

        if (await _repo.ExistsByEmailAsync(dto.Email, id, ct))
            throw new BusinessRuleException($"Ya existe otro usuario con el correo '{dto.Email}'.");

        entity.IdTipoUsuario = dto.IdTipoUsuario;
        entity.IdIdentificacion = dto.IdIdentificacion;
        entity.NumeroIdentificacion = dto.NumeroIdentificacion;
        entity.Nombre = dto.Nombre;
        entity.Apellidos = dto.Apellidos;
        entity.Email = dto.Email;
        entity.Telefono = dto.Telefono;

        if (!string.IsNullOrWhiteSpace(dto.Password))
            entity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        await _repo.UpdateAsync(entity, ct);
        entity.TipoUsuario = tipo;
        entity.Identificacion = identificacion;
        return MapToDto(entity);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        EnsureAdmin();

        var entity = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException(nameof(Usuario), id);

        await _repo.SoftDeleteAsync(entity, ct);
    }

    private void EnsureAdmin()
    {
        if (!_currentUser.IsInRole(RolesUsuario.Administrador))
            throw new ForbiddenOperationException(
                $"Solo los usuarios '{RolesUsuario.Administrador}' pueden gestionar el catálogo de Usuarios.");
    }

    private static UsuarioDto MapToDto(Usuario u) =>
        new(u.IdUsuario,
            u.IdTipoUsuario,
            u.TipoUsuario?.Nombre,
            u.IdIdentificacion,
            u.Identificacion?.Nombre,
            u.NumeroIdentificacion,
            u.Nombre,
            u.Apellidos,
            u.Email,
            u.Telefono,
            u.Activo);
}
