using Microsoft.EntityFrameworkCore;
using UHabitacional.API.Application.Interfaces.Repositories;
using UHabitacional.API.Domain.Entities;
using UHabitacional.API.Infrastructure.Data;

namespace UHabitacional.API.Infrastructure.Repositories;

public class EdificioRepository : IEdificioRepository
{
    private readonly ApplicationDbContext _context;

    public EdificioRepository(ApplicationDbContext context) => _context = context;

    public async Task<IEnumerable<Edificio>> GetAllAsync(CancellationToken ct = default) =>
        await _context.Edificios.AsNoTracking().ToListAsync(ct);

    public async Task<Edificio?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await _context.Edificios.FirstOrDefaultAsync(e => e.IdEdificio == id, ct);

    public async Task<Edificio> AddAsync(Edificio entity, CancellationToken ct = default)
    {
        _context.Edificios.Add(entity);
        await _context.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateAsync(Edificio entity, CancellationToken ct = default)
    {
        entity.FechaModificacion = DateTime.UtcNow;
        _context.Edificios.Update(entity);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsByNameAsync(string nombre, int? excludeId = null, CancellationToken ct = default) =>
        await _context.Edificios.AnyAsync(e =>
            e.Nombre == nombre && (excludeId == null || e.IdEdificio != excludeId), ct);

    public async Task<int> CountDepartamentosAsync(int idEdificio, CancellationToken ct = default) =>
        await _context.Departamentos.CountAsync(d => d.IdEdificio == idEdificio, ct);
}
