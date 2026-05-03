using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UHabitacionalAPI.Application.Interfaces;
using UHabitacionalAPI.Domain.Entities;
using UHabitacionalAPI.Domain.Exceptions;
using UHabitacionalAPI.Infrastructure.Contexts;
using UHabitacionalAPI.Presentation.Dtos;

namespace UHabitacionalAPI.Infrastructure.Repositories
{
    public class EdificiosRepository : IEdificiosRepository
    {
        private readonly UHabitacionalContext _context;
        public EdificiosRepository(UHabitacionalContext context)
        {
            _context = context;
        }

        public async Task<string> CreateAsync(Edificio edificio)
        {
            try
            {
                await _context.Edificio.AddAsync(edificio);
                await _context.SaveChangesAsync();

                return edificio.Id;
            }
            catch (DbUpdateException dbEx)
            {
                throw new DomainException(
                    "No se pudo crear el edificio en la base de datos, intentelo de nuevo más tarde.", dbEx
                );
            }
            catch (Exception ex)
            {
                throw new DomainException(
                    "Ocurrió un error inesperado en la creación del edificio.", ex
                );
            }
        }

        public async Task<List<Edificio>> GetAsync(EdificioFilterRequest filters)
        {
            try
            {
                IQueryable<Edificio> query = _context.Edificio.AsQueryable();

                if (filters.Estatus.HasValue)
                {
                    query = query.Where(e => e.Estatus == filters.Estatus.Value);
                }

                if (filters.NumeroPisos.HasValue)
                {
                    query = query.Where(e => e.NumeroPisos == filters.NumeroPisos);
                }

                if (filters.TotalDeptos.HasValue)
                {
                    query = query.Where(e => e.TotalDeptos == filters.TotalDeptos);
                }

                List<Edificio> edificios = await query.ToListAsync();

                if (edificios.IsNullOrEmpty())
                {
                    throw new UhNotFoundException("No se encontraron edificios con los filtros seleccionados.");
                }

                return edificios;
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
                    "Ocurrió un error inesperado en la consulta de edificios.", ex
                );
            }
        }

        public async Task<Edificio> GetByIdAsync(string id)
        {
            try
            {
                Edificio? edificio = await _context.Edificio
                    .FirstOrDefaultAsync(e => e.Id == id);

                if (edificio is null)
                {
                    throw new UhNotFoundException($"Edificio no encontrado (ID. {id})");
                }

                return edificio;
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
                    $"Ocurrió un error inesperado en la consulta de edificio (ID. {id})", ex
                );
            }
        }

        public async Task<int> GetTotalDepartamentos(string edificioId)
        {
            try
            {
                Edificio? edificio = await _context.Edificio
                    .Include(e => e.Departamentos)
                    .FirstOrDefaultAsync(e => e.Id == edificioId);

                if (edificio is null)
                {
                    throw new UhNotFoundException($"Edificio no encontrado (ID. {edificioId})");
                }

                return edificio.Departamentos.Count;
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
                    $"Ocurrió un error inesperado en la consulta de edificio (ID. {edificioId})", ex
                );
            }
        }

        public async Task<int> UpdateAsync(Edificio request)
        {
            try
            {
                _context.Edificio.Update(request);
                return await _context.SaveChangesAsync();
            } 
            catch (DbUpdateException ex)
            {
                throw new DomainException(
                    "No se pudo actualizar el edificio en la base de datos, intentelo de nuevo más tarde.", ex
                );
            }
            catch (ArgumentNullException ex)
            {
                throw new DomainException("Valor nulo no aceptado", ex);
            }
            catch (Exception ex)
            {
                throw new DomainException(
                    $"Ocurrió un error inesperado con la actualización del edificio (ID. {request.Id})", ex
                );
            }
        }
    }
}
