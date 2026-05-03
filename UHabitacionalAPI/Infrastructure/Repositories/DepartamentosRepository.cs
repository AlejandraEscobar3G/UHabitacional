using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UHabitacionalAPI.Application.Interfaces;
using UHabitacionalAPI.Domain.Entities;
using UHabitacionalAPI.Domain.Exceptions;
using UHabitacionalAPI.Infrastructure.Contexts;
using UHabitacionalAPI.Presentation.Dtos;

namespace UHabitacionalAPI.Infrastructure.Repositories
{
    public class DepartamentosRepository : IDepartamentosRepository
    {
        private readonly UHabitacionalContext _context;
        public DepartamentosRepository(UHabitacionalContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(Departamento departamento)
        {
            try
            {
                await _context.Departamento.AddAsync(departamento);
                await _context.SaveChangesAsync();

                return departamento.Id;
            }
            catch (DbUpdateException dbEx)
            {
                throw new DomainException(
                    "No se pudo crear el departamento en la base de datos, intentelo de nuevo más tarde.", dbEx
                );
            }
            catch (Exception ex)
            {
                throw new DomainException(
                    "Ocurrió un error inesperado en la creación del departamento.", ex
                );
            }
        }

        public async Task<List<Departamento>> GetAsync(DepartamentoFilterRequest filters)
        {
            try
            {
                IQueryable<Departamento> query = _context.Departamento
                    .Include(d => d.Edificio)
                    .AsQueryable();

                if (!filters.NumeroInt.IsNullOrEmpty())
                {
                    query = query.Where(e => e.NumeroInt == filters.NumeroInt);
                }

                if (filters.Piso.HasValue)
                {
                    query = query.Where(e => e.Piso == filters.Piso.Value);
                }

                if (!filters.EdificioId.IsNullOrEmpty())
                {
                    query = query.Where(e => e.EdificioId == filters.EdificioId);
                }

                List<Departamento> departamentos = await query.ToListAsync();

                if (departamentos.IsNullOrEmpty())
                {
                    throw new UhNotFoundException("No se encontraron departamentos con los filtros seleccionados.");
                }

                return departamentos;
            }
            catch (UhNotFoundException ex)
            {
                throw new DomainException(ex.Message, ex);
            }
            catch (ArgumentNullException ex)
            {
                throw new DomainException("Valor nulo no aceptado", ex);
            }
            catch (Exception ex)
            {
                throw new DomainException(
                    "Ocurrió un error inesperado en la consulta de departamentos.", ex
                );
            }
        }

        public async Task<Departamento> GetByIdAsync(int id)
        {
            try
            {
                Departamento? departamento = await _context.Departamento
                    .FirstOrDefaultAsync(e => e.Id == id);

                if (departamento is null)
                {
                    throw new UhNotFoundException($"Departamento no encontrado (ID. {id})");
                }

                return departamento;
            }
            catch (UhNotFoundException ex)
            {
                throw new DomainException(ex.Message, ex);
            }
            catch (ArgumentNullException ex)
            {
                throw new DomainException("Valor nulo no aceptado", ex);
            }
            catch (Exception ex)
            {
                throw new DomainException(
                    $"Ocurrió un error inesperado en la consulta de departamento (ID. {id})", ex
                );
            }
        }

        public async Task<int> UpdateAsync(Departamento departamento)
        {
            try
            {
                _context.Departamento.Update(departamento);
                return await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new DomainException(
                    "No se pudo actualizar el departamento en la base de datos, intentelo de nuevo más tarde.", ex
                );
            }
            catch (ArgumentNullException ex)
            {
                throw new DomainException("Valor nulo no aceptado", ex);
            }
            catch (Exception ex)
            {
                throw new DomainException(
                    $"Ocurrió un error inesperado con la actualización del departamento (ID. {departamento.Id})", ex
                );
            }
        }
    }
}
