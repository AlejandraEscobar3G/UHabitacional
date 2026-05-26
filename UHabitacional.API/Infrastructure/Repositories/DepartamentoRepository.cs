using Microsoft.EntityFrameworkCore;
using UHabitacional.API.Application.Interfaces.Repositories;
using UHabitacional.API.Domain.Entities;
using UHabitacional.API.Infrastructure.Data;

namespace UHabitacional.API.Infrastructure.Repositories;

public class DepartamentoRepository : IDepartamentoRepository
{
    private readonly ApplicationDbContext _context;

    public DepartamentoRepository(ApplicationDbContext context) => _context = context;

    public async Task<IEnumerable<Departamento>> GetAllAsync(CancellationToken ct = default) =>
        await _context.Departamentos.AsNoTracking()
            .Include(d => d.Edificio)
            .ToListAsync(ct);

    public async Task<Departamento?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await _context.Departamentos
            .Include(d => d.Edificio)
            .FirstOrDefaultAsync(d => d.IdDepartamento == id, ct);

    public async Task<Departamento> AddAsync(Departamento entity, CancellationToken ct = default)
    {
        _context.Departamentos.Add(entity);
        await _context.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateAsync(Departamento entity, CancellationToken ct = default)
    {
        entity.FechaModificacion = DateTime.UtcNow;
        _context.Departamentos.Update(entity);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<bool> HasInquilinoActivoAsync(int idDepartamento, CancellationToken ct = default) =>
        await _context.Inquilinos.AnyAsync(i =>
            i.IdDepartamento == idDepartamento && i.FechaFin == null, ct);

    public async Task<bool> ExistsByNumeroAsync(int idEdificio, string numero, int? excludeId = null, CancellationToken ct = default) =>
        await _context.Departamentos.AnyAsync(d =>
            d.IdEdificio == idEdificio &&
            d.NumeroDepartamento == numero &&
            (excludeId == null || d.IdDepartamento != excludeId), ct);
}
