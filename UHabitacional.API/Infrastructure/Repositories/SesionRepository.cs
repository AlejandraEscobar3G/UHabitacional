using Microsoft.EntityFrameworkCore;
using UHabitacional.API.Application.Interfaces.Repositories;
using UHabitacional.API.Domain.Entities;
using UHabitacional.API.Infrastructure.Data;

namespace UHabitacional.API.Infrastructure.Repositories;

public class SesionRepository : ISesionRepository
{
    private readonly ApplicationDbContext _context;

    public SesionRepository(ApplicationDbContext context) => _context = context;

    public async Task<Sesion?> GetByJtiAsync(string jti, CancellationToken ct = default) =>
        await _context.Sesiones
            .Include(s => s.Usuario)
                .ThenInclude(u => u!.TipoUsuario)
            .FirstOrDefaultAsync(s => s.Jti == jti, ct);

    public async Task<Sesion?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await _context.Sesiones
            .Include(s => s.Usuario)
            .FirstOrDefaultAsync(s => s.IdSesion == id, ct);

    public async Task<IEnumerable<Sesion>> GetActivasByUsuarioAsync(int idUsuario, CancellationToken ct = default) =>
        await _context.Sesiones
            .Where(s => s.IdUsuario == idUsuario && s.Activa)
            .OrderByDescending(s => s.FechaInicio)
            .AsNoTracking()
            .ToListAsync(ct);

    public async Task<Sesion> AddAsync(Sesion entity, CancellationToken ct = default)
    {
        _context.Sesiones.Add(entity);
        await _context.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateAsync(Sesion entity, CancellationToken ct = default)
    {
        _context.Sesiones.Update(entity);
        await _context.SaveChangesAsync(ct);
    }

    public async Task RevokeAsync(Sesion entity, CancellationToken ct = default)
    {
        entity.Activa = false;
        entity.FechaCierre = DateTime.UtcNow;
        await _context.SaveChangesAsync(ct);
    }

    public async Task RevokeAllByUsuarioAsync(int idUsuario, CancellationToken ct = default)
    {
        var activas = await _context.Sesiones
            .Where(s => s.IdUsuario == idUsuario && s.Activa)
            .ToListAsync(ct);

        var ahora = DateTime.UtcNow;
        foreach (var s in activas)
        {
            s.Activa = false;
            s.FechaCierre = ahora;
        }

        if (activas.Count > 0)
            await _context.SaveChangesAsync(ct);
    }
}
