using Microsoft.EntityFrameworkCore;
using UHabitacional.API.Application.Interfaces.Repositories;
using UHabitacional.API.Domain.Entities;
using UHabitacional.API.Infrastructure.Data;

namespace UHabitacional.API.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly ApplicationDbContext _context;

    public UsuarioRepository(ApplicationDbContext context) => _context = context;

    public async Task<IEnumerable<Usuario>> GetAllAsync(CancellationToken ct = default) =>
        await _context.Usuarios.AsNoTracking()
            .Include(u => u.TipoUsuario)
            .Include(u => u.Identificacion)
            .ToListAsync(ct);

    public async Task<Usuario?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await _context.Usuarios
            .Include(u => u.TipoUsuario)
            .Include(u => u.Identificacion)
            .FirstOrDefaultAsync(u => u.IdUsuario == id, ct);

    public async Task<Usuario?> GetByEmailAsync(string email, CancellationToken ct = default) =>
        await _context.Usuarios
            .Include(u => u.TipoUsuario)
            .FirstOrDefaultAsync(u => u.Email == email, ct);

    public async Task<Usuario> AddAsync(Usuario entity, CancellationToken ct = default)
    {
        _context.Usuarios.Add(entity);
        await _context.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateAsync(Usuario entity, CancellationToken ct = default)
    {
        entity.FechaModificacion = DateTime.UtcNow;
        _context.Usuarios.Update(entity);
        await _context.SaveChangesAsync(ct);
    }

    public async Task SoftDeleteAsync(Usuario entity, CancellationToken ct = default)
    {
        entity.Activo = false;
        entity.FechaModificacion = DateTime.UtcNow;
        // Si tiene un Inquilino asociado, también se elimina lógicamente
        var inquilino = await _context.Inquilinos
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(i => i.IdUsuario == entity.IdUsuario);
        if (inquilino is not null)
        {
            inquilino.Activo = false;
            inquilino.FechaModificacion = DateTime.UtcNow;
        }
        await _context.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsByEmailAsync(string email, int? excludeId = null, CancellationToken ct = default) =>
        await _context.Usuarios.AnyAsync(u =>
            u.Email == email && (excludeId == null || u.IdUsuario != excludeId), ct);
}
