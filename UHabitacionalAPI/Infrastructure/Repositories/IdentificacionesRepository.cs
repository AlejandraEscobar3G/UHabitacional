using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UHabitacionalAPI.Application.Interfaces;
using UHabitacionalAPI.Domain.Entities;
using UHabitacionalAPI.Domain.Exceptions;
using UHabitacionalAPI.Infrastructure.Contexts;
using UHabitacionalAPI.Presentation.Dtos;

namespace UHabitacionalAPI.Infrastructure.Repositories
{
    public class IdentificacionesRepository : IIdentificacionesRepository
    {
        private readonly UHabitacionalContext _context;
        public IdentificacionesRepository(UHabitacionalContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(Identificacion identificacion)
        {
            try
            {
                await _context.Identificacion.AddAsync(identificacion);
                await _context.SaveChangesAsync();

                return identificacion.Id;
            }
            catch (DbUpdateException dbEx)
            {
                throw new DomainException(
                    "No se pudo crear la identificación en la base de datos, intentelo de nuevo más tarde.", dbEx
                );
            }
            catch (Exception ex)
            {
                throw new DomainException(
                    "Ocurrió un error inesperado en la creación de la identificación.", ex
                );
            }
        }

        public async Task<List<Identificacion>> GetAsync(IdentificacionFilterRequest filters)
        {
            try
            {
                IQueryable<Identificacion> query = _context.Identificacion.AsQueryable();

                if (filters.Estatus.HasValue)
                {
                    query = query.Where(e => e.Estatus == filters.Estatus.Value);
                }

                if (!filters.Descripcion.IsNullOrEmpty())
                {
                    query = query.Where(e => EF.Functions.Like(e.Descripcion, $"%{filters.Descripcion}%"));
                }

                List<Identificacion> identificaciones = await query.ToListAsync();

                if (identificaciones.IsNullOrEmpty())
                {
                    throw new UhNotFoundException("No se encontraron identificaciones con los filtros seleccionados.");
                }

                return identificaciones;
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
                    "Ocurrió un error inesperado en la consulta de identificaciones.", ex
                );
            }
        }

        public async Task<Identificacion> GetByIdAsync(int id)
        {
            try
            {
                Identificacion? identificacion = await _context.Identificacion
                    .FirstOrDefaultAsync(e => e.Id == id);

                if (identificacion is null)
                {
                    throw new UhNotFoundException($"Identificación no encontrada (ID. {id})");
                }

                return identificacion;
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
                    $"Ocurrió un error inesperado en la consulta de la identificación (ID. {id})", ex
                );
            }
        }

        public async Task<int> UpdateAsync(Identificacion request)
        {
            try
            {
                _context.Identificacion.Update(request);
                return await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new DomainException(
                    "No se pudo actualizar la identificación en la base de datos, intentelo de nuevo más tarde.", ex
                );
            }
            catch (ArgumentNullException ex)
            {
                throw new DomainException("Valor nulo no aceptado", ex);
            }
            catch (Exception ex)
            {
                throw new DomainException(
                    $"Ocurrió un error inesperado con la actualización de la identificación (ID. {request.Id})", ex
                );
            }
        }
    }
}
