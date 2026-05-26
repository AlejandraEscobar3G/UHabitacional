using Microsoft.EntityFrameworkCore;
using UHabitacional.API.Application.Interfaces.Repositories;
using UHabitacional.API.Domain.Entities;
using UHabitacional.API.Infrastructure.Data;

namespace UHabitacional.API.Infrastructure.Repositories;

public class BitacoraVisitanteRepository : IBitacoraVisitanteRepository
{
    private readonly ApplicationDbContext _context;

    public BitacoraVisitanteRepository(ApplicationDbContext context) => _context = context;

    public async Task<IEnumerable<BitacoraVisitante>> GetAllAsync(CancellationToken ct = default) =>
        await _context.BitacorasVisitante.AsNoTracking()
            .Include(b => b.Inquilino)
                .ThenInclude(i => i!.Usuario)
            .Include(b => b.Identificacion)
            .OrderByDescending(b => b.FechaCreacion)
            .ToListAsync(ct);

    public async Task<BitacoraVisitante?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await _context.BitacorasVisitante
            .Include(b => b.Inquilino)
                .ThenInclude(i => i!.Usuario)
            .Include(b => b.Identificacion)
            .FirstOrDefaultAsync(b => b.IdBitacoraVisitante == id, ct);

    public async Task<BitacoraVisitante?> GetByCodigoAsync(string codigo, CancellationToken ct = default) =>
        await _context.BitacorasVisitante
            .Include(b => b.Inquilino)
                .ThenInclude(i => i!.Usuario)
            .Include(b => b.Identificacion)
            .FirstOrDefaultAsync(b => b.CodigoVisita == codigo, ct);

    public async Task<BitacoraVisitante> AddAsync(BitacoraVisitante entity, CancellationToken ct = default)
    {
        _context.BitacorasVisitante.Add(entity);
        await _context.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateAsync(BitacoraVisitante entity, CancellationToken ct = default)
    {
        entity.FechaModificacion = DateTime.UtcNow;
        _context.BitacorasVisitante.Update(entity);
        await _context.SaveChangesAsync(ct);
    }

    public async Task SoftDeleteAsync(BitacoraVisitante entity, CancellationToken ct = default)
    {
        entity.Activo = false;
        entity.FechaModificacion = DateTime.UtcNow;
        await _context.SaveChangesAsync(ct);
    }
}
