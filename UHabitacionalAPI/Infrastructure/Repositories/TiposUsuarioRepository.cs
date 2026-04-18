using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UHabitacionalAPI.Application.Interfaces;
using UHabitacionalAPI.Domain.Entities;
using UHabitacionalAPI.Domain.Exceptions;
using UHabitacionalAPI.Infrastructure.Contexts;
using UHabitacionalAPI.Presentation.Dtos;

namespace UHabitacionalAPI.Infrastructure.Repositories
{
    public class TiposUsuarioRepository : ITiposUsuarioRepository
    {
        private readonly UHabitacionalContext _context;
        public TiposUsuarioRepository(UHabitacionalContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(TipoUsuario tipoUsuario)
        {
            try
            {
                await _context.TiposUsuario.AddAsync(tipoUsuario);
                await _context.SaveChangesAsync();

                return tipoUsuario.Id;
            }
            catch (DbUpdateException dbEx)
            {
                throw new DomainException(
                    "No se pudo crear el tipo de usuario en la base de datos, intentelo de nuevo más tarde.", dbEx
                );
            }
            catch (Exception ex)
            {
                throw new DomainException(
                    "Ocurrió un error inesperado en la creación del tipo de usuario.", ex
                );
            }
        }

        public async Task<List<TipoUsuario>> GetAsync(TipoUsuarioFilterRequest filters)
        {
            try
            {
                IQueryable<TipoUsuario> query = _context.TiposUsuario.AsQueryable();

                if (filters.Estatus.HasValue)
                {
                    query = query.Where(e => e.Estatus == filters.Estatus.Value);
                }

                if (!filters.Descripcion.IsNullOrEmpty())
                {
                    query = query.Where(e => EF.Functions.Like(e.Descripcion, $"%{filters.Descripcion}%"));
                }

                List<TipoUsuario> tipoUsuarios = await query.ToListAsync();

                if (tipoUsuarios.IsNullOrEmpty())
                {
                    throw new UhNotFoundException("No se encontraron tipos de usuario con los filtros seleccionados.");
                }

                return tipoUsuarios;
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
                    "Ocurrió un error inesperado en la consulta de tipos de usuario.", ex
                );
            }
        }

        public async Task<TipoUsuario> GetByIdAsync(int id)
        {
            try
            {
                TipoUsuario? tipoUsuario = await _context.TiposUsuario
                    .FirstOrDefaultAsync(e => e.Id == id);

                if (tipoUsuario is null)
                {
                    throw new UhNotFoundException($"Tipo de usuario no encontrado (ID. {id})");
                }

                return tipoUsuario;
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
                    $"Ocurrió un error inesperado en la consulta de tipo de usuario (ID. {id})", ex
                );
            }
        }

        public async Task<int> UpdateAsync(TipoUsuario request)
        {
            try
            {
                _context.TiposUsuario.Update(request);
                return await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new DomainException(
                    "No se pudo actualizar el tipo de usuario en la base de datos, intentelo de nuevo más tarde.", ex
                );
            }
            catch (ArgumentNullException ex)
            {
                throw new DomainException("Valor nulo no aceptado", ex);
            }
            catch (Exception ex)
            {
                throw new DomainException(
                    $"Ocurrió un error inesperado con la actualización del tipo de usuario (ID. {request.Id})", ex
                );
            }
        }
    }
}
