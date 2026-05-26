using Microsoft.EntityFrameworkCore;
using UHabitacional.API.Application.Interfaces.Repositories;
using UHabitacional.API.Domain.Entities;
using UHabitacional.API.Infrastructure.Data;

namespace UHabitacional.API.Infrastructure.Repositories;

public class TipoUsuarioRepository : ITipoUsuarioRepository
{
    private readonly ApplicationDbContext _context;

    public TipoUsuarioRepository(ApplicationDbContext context) => _context = context;

    public async Task<IEnumerable<TipoUsuario>> GetAllAsync(CancellationToken ct = default) =>
        await _context.TiposUsuario.AsNoTracking().ToListAsync(ct);

    public async Task<TipoUsuario?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await _context.TiposUsuario.FirstOrDefaultAsync(t => t.IdTipoUsuario == id, ct);

    public async Task<TipoUsuario?> GetByNameAsync(string nombre, CancellationToken ct = default) =>
        await _context.TiposUsuario.FirstOrDefaultAsync(t => t.Nombre == nombre, ct);

    public async Task<TipoUsuario> AddAsync(TipoUsuario entity, CancellationToken ct = default)
    {
        _context.TiposUsuario.Add(entity);
        await _context.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateAsync(TipoUsuario entity, CancellationToken ct = default)
    {
        entity.FechaModificacion = DateTime.UtcNow;
        _context.TiposUsuario.Update(entity);
        await _context.SaveChangesAsync(ct);
    }

    public async Task SoftDeleteAsync(TipoUsuario entity, CancellationToken ct = default)
    {
        entity.Activo = false;
        entity.FechaModificacion = DateTime.UtcNow;
        await _context.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsByNameAsync(string nombre, int? excludeId = null, CancellationToken ct = default) =>
        await _context.TiposUsuario.AnyAsync(t =>
            t.Nombre == nombre && (excludeId == null || t.IdTipoUsuario != excludeId), ct);
}
