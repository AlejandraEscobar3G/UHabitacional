using Microsoft.EntityFrameworkCore;
using UHabitacional.API.Application.Interfaces.Repositories;
using UHabitacional.API.Domain.Entities;
using UHabitacional.API.Infrastructure.Data;

namespace UHabitacional.API.Infrastructure.Repositories;

public class BitacoraVigilanteRepository : IBitacoraVigilanteRepository
{
    private readonly ApplicationDbContext _context;

    public BitacoraVigilanteRepository(ApplicationDbContext context) => _context = context;

    public async Task<IEnumerable<BitacoraVigilante>> GetAllAsync(CancellationToken ct = default) =>
        await _context.BitacorasVigilante.AsNoTracking()
            .Include(b => b.Usuario)
            .OrderByDescending(b => b.FechaHoraEntrada)
            .ToListAsync(ct);

    public async Task<BitacoraVigilante?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await _context.BitacorasVigilante
            .Include(b => b.Usuario)
            .FirstOrDefaultAsync(b => b.IdBitacoraVigilante == id, ct);

    public async Task<BitacoraVigilante> AddAsync(BitacoraVigilante entity, CancellationToken ct = default)
    {
        _context.BitacorasVigilante.Add(entity);
        await _context.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateAsync(BitacoraVigilante entity, CancellationToken ct = default)
    {
        entity.FechaModificacion = DateTime.UtcNow;
        _context.BitacorasVigilante.Update(entity);
        await _context.SaveChangesAsync(ct);
    }
}
