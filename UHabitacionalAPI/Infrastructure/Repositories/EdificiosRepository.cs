using Microsoft.EntityFrameworkCore;
using UHabitacionalAPI.Application.Interfaces;
using UHabitacionalAPI.Domain.Entities;
using UHabitacionalAPI.Domain.Enums;
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
                    DomainEntity.EDIFICIO, DomainOperation.CREATE,
                    "No se pudo crear el edificio en la base de datos, intentelo de nuevo más tarde.", dbEx
                );
            }
            catch (Exception ex)
            {
                throw new DomainException(
                    DomainEntity.EDIFICIO, DomainOperation.CREATE,
                    "Ocurrió un error inesperado en la creación del edificio.", ex
                );
            }
        }

        public async Task<List<EdificioResponse>> GetAsync(EdificioFilterRequest filters)
        {
            try
            {
                var query = _context.Edificio.AsQueryable();

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

                List<EdificioResponse> edificios = await query
                    .Select(e => new EdificioResponse
                    {
                        Id = e.Id,
                        Calle = e.Calle,
                        NumeroPisos = e.NumeroPisos,
                        Estatus = e.Estatus,
                        TotalDeptos = e.TotalDeptos
                    })
                    .ToListAsync();

                return edificios;
            }
            catch (ArgumentNullException ex)
            {
                throw new DomainException(DomainEntity.EDIFICIO, DomainOperation.READ, "Valor nulo no aceptado", ex);
            }
            catch (Exception ex)
            {
                throw new DomainException(
                    DomainEntity.EDIFICIO, DomainOperation.READ,
                    "Ocurrió un error inesperado en la consulta de edificios.", ex
                );
            }
        }

        public async Task<EdificioResponse?> GetByIdAsync(string id)
        {
            try
            {
                EdificioResponse? edificio = await _context.Edificio
                    .Where(e => e.Id == id)
                    .Select(e => new EdificioResponse
                    {
                        Id = e.Id,
                        Calle = e.Calle,
                        NumeroPisos = e.NumeroPisos,
                        Estatus = e.Estatus,
                        TotalDeptos = e.TotalDeptos
                    })
                    .FirstOrDefaultAsync();

                if (edificio is null)
                {
                    throw new UhNotFoundException(DomainEntity.EDIFICIO, $"Edificio no encontrado (ID. {id})");
                }

                return edificio;
            }
            catch (UhNotFoundException ex)
            {
                throw new DomainException(ex.Entity, ex.Operation, ex.Message, ex);
            }
            catch (ArgumentNullException ex)
            {
                throw new DomainException(DomainEntity.EDIFICIO, DomainOperation.READ, "Valor nulo no aceptado", ex);
            }
            catch (Exception ex)
            {
                throw new DomainException(
                    DomainEntity.EDIFICIO, DomainOperation.READ,
                    $"Ocurrió un error inesperado en la consulta de edificio (ID. {id})", ex
                );
            }
        }

        public async Task<int> UpdateAsync(string id, EdificioRequest request, int userId)
        {
            try
            {
                Edificio? edificio = _context.Edificio
                    .Where(e => e.Id == id)
                    .FirstOrDefault(e => e.Id == id);

                if (edificio is null)
                {
                    throw new UhNotFoundException(DomainEntity.EDIFICIO, $"Edificio no encontrado (ID. {id})");
                }

                edificio.Calle = request.Calle;
                edificio.TotalDeptos = request.TotalDeptos;
                edificio.NumeroPisos = request.NumeroPisos;
                edificio.Estatus = request.Estatus;
                edificio.ModifyAt = DateTime.Now;
                edificio.ModifyBy = userId;

                return await _context.SaveChangesAsync();
            } 
            catch (DbUpdateException ex)
            {
                throw new DomainException(
                    DomainEntity.EDIFICIO, DomainOperation.UPDATE,
                    "No se pudo actualizar el edificio en la base de datos, intentelo de nuevo más tarde.", ex
                );
            }
            catch (ArgumentNullException ex)
            {
                throw new DomainException(DomainEntity.EDIFICIO, DomainOperation.READ, "Valor nulo no aceptado", ex);
            }
            catch(UhNotFoundException ex)
            {
                throw new DomainException(ex.Entity, ex.Operation, ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new DomainException(
                    DomainEntity.EDIFICIO, DomainOperation.READ,
                    $"Ocurrió un error inesperado con la actualización del edificio (ID. {id})", ex
                );
            }
        }
    }
}
