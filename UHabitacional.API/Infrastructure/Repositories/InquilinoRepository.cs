using Microsoft.EntityFrameworkCore;
using UHabitacional.API.Application.Interfaces.Repositories;
using UHabitacional.API.Domain.Entities;
using UHabitacional.API.Infrastructure.Data;

namespace UHabitacional.API.Infrastructure.Repositories;

public class InquilinoRepository : IInquilinoRepository
{
    private readonly ApplicationDbContext _context;

    public InquilinoRepository(ApplicationDbContext context) => _context = context;

    public async Task<IEnumerable<Inquilino>> GetAllAsync(CancellationToken ct = default) =>
        await _context.Inquilinos.AsNoTracking()
            .Include(i => i.Usuario)
            .Include(i => i.Departamento)
                .ThenInclude(d => d!.Edificio)
            .ToListAsync(ct);

    public async Task<Inquilino?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await _context.Inquilinos
            .Include(i => i.Usuario)
            .Include(i => i.Departamento)
                .ThenInclude(d => d!.Edificio)
            .FirstOrDefaultAsync(i => i.IdInquilino == id, ct);

    public async Task<Inquilino?> GetByUsuarioIdAsync(int idUsuario, CancellationToken ct = default) =>
        await _context.Inquilinos
            .FirstOrDefaultAsync(i => i.IdUsuario == idUsuario, ct);

    public async Task<Inquilino?> GetActivoByDepartamentoAsync(int idDepartamento, CancellationToken ct = default) =>
        await _context.Inquilinos
            .FirstOrDefaultAsync(i => i.IdDepartamento == idDepartamento && i.FechaFin == null, ct);

    public async Task<Inquilino> AddAsync(Inquilino entity, CancellationToken ct = default)
    {
        _context.Inquilinos.Add(entity);
        await _context.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateAsync(Inquilino entity, CancellationToken ct = default)
    {
        entity.FechaModificacion = DateTime.UtcNow;
        _context.Inquilinos.Update(entity);
        await _context.SaveChangesAsync(ct);
    }

    public async Task SoftDeleteAsync(Inquilino entity, CancellationToken ct = default)
    {
        entity.Activo = false;
        entity.FechaModificacion = DateTime.UtcNow;
        await _context.SaveChangesAsync(ct);
    }
}
