using UHabitacional.API.Application.DTOs;
using UHabitacional.API.Application.Helpers;
using UHabitacional.API.Application.Interfaces.Repositories;
using UHabitacional.API.Application.Interfaces.Services;
using UHabitacional.API.Domain.Constants;
using UHabitacional.API.Domain.Entities;
using UHabitacional.API.Domain.Exceptions;

namespace UHabitacional.API.Application.Services;

public class BitacoraVisitanteService : IBitacoraVisitanteService
{
    private readonly IBitacoraVisitanteRepository _repo;
    private readonly IInquilinoRepository _inquilinoRepo;
    private readonly IIdentificacionRepository _idRepo;
    private readonly ICurrentUserService _currentUser;

    public BitacoraVisitanteService(
        IBitacoraVisitanteRepository repo,
        IInquilinoRepository inquilinoRepo,
        IIdentificacionRepository idRepo,
        ICurrentUserService currentUser)
    {
        _repo = repo;
        _inquilinoRepo = inquilinoRepo;
        _idRepo = idRepo;
        _currentUser = currentUser;
    }

    public async Task<IEnumerable<BitacoraVisitanteDto>> GetAllAsync(CancellationToken ct = default)
    {
        if (!_currentUser.IsAuthenticated)
            throw new ForbiddenOperationException("Debe estar autenticado para consultar la bitácora de visitantes.");

        var entities = await _repo.GetAllAsync(ct);
        return entities.Select(MapToDto);
    }

    public async Task<BitacoraVisitanteDto> GetByIdAsync(int id, CancellationToken ct = default)
    {
        if (!_currentUser.IsAuthenticated)
            throw new ForbiddenOperationException("Debe estar autenticado para consultar la bitácora de visitantes.");

        var entity = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException(nameof(BitacoraVisitante), id);
        return MapToDto(entity);
    }

    public async Task<BitacoraVisitanteDto> CreateAsync(BitacoraVisitanteCreateDto dto, CancellationToken ct = default)
    {
        if (!_currentUser.IsInRole(RolesUsuario.Inquilino))
            throw new ForbiddenOperationException(
                $"Solo los usuarios '{RolesUsuario.Inquilino}' pueden registrar visitantes.");

        var idUsuario = _currentUser.IdUsuario
            ?? throw new ForbiddenOperationException("No se pudo identificar al inquilino autenticado.");

        var inquilino = await _inquilinoRepo.GetByUsuarioIdAsync(idUsuario, ct)
            ?? throw new NotFoundException(nameof(Inquilino), idUsuario);

        if (inquilino.FechaFin != null)
            throw new BusinessRuleException("El inquilino no está activo, no puede registrar visitantes.");

        // Validar identificación
        var identificacion = await _idRepo.GetByIdAsync(dto.IdIdentificacion, ct)
            ?? throw new NotFoundException(nameof(Identificacion), dto.IdIdentificacion);

        // Generar código único de 6 caracteres
        string codigo;
        var intentos = 0;
        do
        {
            codigo = CodigoVisitaGenerator.Generar();
            if (++intentos > 10)
                throw new BusinessRuleException("No se pudo generar un código único de visita. Intente nuevamente.");
        }
        while (await _repo.GetByCodigoAsync(codigo, ct) != null);

        var entity = new BitacoraVisitante
        {
            IdInquilino = inquilino.IdInquilino,
            NombreVisitante = dto.NombreVisitante,
            IdIdentificacion = dto.IdIdentificacion,
            NumeroIdentificacion = dto.NumeroIdentificacion,
            CodigoVisita = codigo,
            FechaHoraLlegada = null,
            FechaHoraSalida = null,
            Observaciones = dto.Observaciones,
            Activo = true
        };

        await _repo.AddAsync(entity, ct);
        return MapToDto(await _repo.GetByIdAsync(entity.IdBitacoraVisitante, ct) ?? entity);
    }

    public async Task<BitacoraVisitanteDto> UpdateAsync(int id, BitacoraVisitanteUpdateDto dto, CancellationToken ct = default)
    {
        // Permite que el inquilino que lo creó actualice datos del visitante
        if (!_currentUser.IsAuthenticated)
            throw new ForbiddenOperationException("Debe estar autenticado para actualizar el registro.");

        var entity = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException(nameof(BitacoraVisitante), id);

        var idUsuario = _currentUser.IdUsuario
            ?? throw new ForbiddenOperationException("No se pudo identificar al usuario.");

        if (_currentUser.IsInRole(RolesUsuario.Inquilino))
        {
            var inquilino = await _inquilinoRepo.GetByUsuarioIdAsync(idUsuario, ct);
            if (inquilino == null || inquilino.IdInquilino != entity.IdInquilino)
                throw new ForbiddenOperationException("Solo el inquilino que registró al visitante puede actualizarlo.");

            entity.NombreVisitante = dto.NombreVisitante;
            entity.IdIdentificacion = dto.IdIdentificacion;
            entity.NumeroIdentificacion = dto.NumeroIdentificacion;
            entity.Observaciones = dto.Observaciones;
        }
        else
        {
            throw new ForbiddenOperationException(
                "Solo el inquilino que registró al visitante puede actualizar los datos generales. " +
                "Si eres vigilante, utiliza el endpoint de registro de entrada/salida.");
        }

        await _repo.UpdateAsync(entity, ct);
        return MapToDto(await _repo.GetByIdAsync(entity.IdBitacoraVisitante, ct) ?? entity);
    }

    public async Task<BitacoraVisitanteDto> RegistrarEntradaSalidaAsync(int id, BitacoraVisitanteRegistroDto dto, CancellationToken ct = default)
    {
        if (!_currentUser.IsInRole(RolesUsuario.Vigilante))
            throw new ForbiddenOperationException(
                $"Solo los usuarios '{RolesUsuario.Vigilante}' pueden registrar entradas y salidas de visitantes.");

        var entity = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException(nameof(BitacoraVisitante), id);

        var idVigilante = _currentUser.IdUsuario
            ?? throw new ForbiddenOperationException("No se pudo identificar al vigilante.");

        if (dto.EsLlegada)
        {
            if (entity.FechaHoraLlegada != null)
                throw new BusinessRuleException("Ya se registró la llegada de este visitante.");

            entity.FechaHoraLlegada = DateTime.UtcNow;
            entity.IdVigilanteEntrada = idVigilante;
        }
        else
        {
            if (entity.FechaHoraLlegada == null)
                throw new BusinessRuleException("No se puede registrar la salida sin antes registrar la llegada.");
            if (entity.FechaHoraSalida != null)
                throw new BusinessRuleException("Ya se registró la salida de este visitante.");

            entity.FechaHoraSalida = DateTime.UtcNow;
            entity.IdVigilanteSalida = idVigilante;
        }

        await _repo.UpdateAsync(entity, ct);
        return MapToDto(await _repo.GetByIdAsync(entity.IdBitacoraVisitante, ct) ?? entity);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        if (!_currentUser.IsAuthenticated)
            throw new ForbiddenOperationException("Debe estar autenticado para eliminar el registro.");

        var entity = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException(nameof(BitacoraVisitante), id);

        var idUsuario = _currentUser.IdUsuario
            ?? throw new ForbiddenOperationException("No se pudo identificar al usuario.");

        if (!_currentUser.IsInRole(RolesUsuario.Inquilino) && !_currentUser.IsInRole(RolesUsuario.Administrador))
            throw new ForbiddenOperationException("Solo el inquilino que creó el registro (o un Administrador) puede eliminarlo.");

        if (_currentUser.IsInRole(RolesUsuario.Inquilino))
        {
            var inquilino = await _inquilinoRepo.GetByUsuarioIdAsync(idUsuario, ct);
            if (inquilino == null || inquilino.IdInquilino != entity.IdInquilino)
                throw new ForbiddenOperationException("Solo el inquilino que creó el registro puede eliminarlo.");
        }

        await _repo.SoftDeleteAsync(entity, ct);
    }

    private static BitacoraVisitanteDto MapToDto(BitacoraVisitante b) =>
        new(b.IdBitacoraVisitante,
            b.IdInquilino,
            b.Inquilino?.Usuario != null ? $"{b.Inquilino.Usuario.Nombre} {b.Inquilino.Usuario.Apellidos}" : null,
            b.NombreVisitante,
            b.IdIdentificacion,
            b.Identificacion?.Nombre,
            b.NumeroIdentificacion,
            b.CodigoVisita,
            b.FechaHoraLlegada,
            b.FechaHoraSalida,
            b.Observaciones,
            b.FechaCreacion);
}
