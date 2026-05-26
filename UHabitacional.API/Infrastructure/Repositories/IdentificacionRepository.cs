using Microsoft.EntityFrameworkCore;
using UHabitacional.API.Application.Interfaces.Repositories;
using UHabitacional.API.Domain.Entities;
using UHabitacional.API.Infrastructure.Data;

namespace UHabitacional.API.Infrastructure.Repositories;

public class IdentificacionRepository : IIdentificacionRepository
{
    private readonly ApplicationDbContext _context;

    public IdentificacionRepository(ApplicationDbContext context) => _context = context;

    public async Task<IEnumerable<Identificacion>> GetAllAsync(CancellationToken ct = default) =>
        await _context.Identificaciones.AsNoTracking().ToListAsync(ct);

    public async Task<Identificacion?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await _context.Identificaciones.FirstOrDefaultAsync(i => i.IdIdentificacion == id, ct);

    public async Task<Identificacion> AddAsync(Identificacion entity, CancellationToken ct = default)
    {
        _context.Identificaciones.Add(entity);
        await _context.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateAsync(Identificacion entity, CancellationToken ct = default)
    {
        entity.FechaModificacion = DateTime.UtcNow;
        _context.Identificaciones.Update(entity);
        await _context.SaveChangesAsync(ct);
    }

    public async Task SoftDeleteAsync(Identificacion entity, CancellationToken ct = default)
    {
        entity.Activo = false;
        entity.FechaModificacion = DateTime.UtcNow;
        await _context.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsByNameAsync(string nombre, int? excludeId = null, CancellationToken ct = default) =>
        await _context.Identificaciones.AnyAsync(i =>
            i.Nombre == nombre && (excludeId == null || i.IdIdentificacion != excludeId), ct);
}
