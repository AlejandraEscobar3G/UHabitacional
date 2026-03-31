using UHabitacionalAPI.Application.Interfaces;
using UHabitacionalAPI.Domain.Entities;
using UHabitacionalAPI.Domain.Enums;
using UHabitacionalAPI.Presentation.Dtos;

namespace UHabitacionalAPI.Application.Services
{
    public class IdentificacionesService : IIdentificacionesService
    {
        private readonly IIdentificacionesRepository _identificacionesRepository;
        public IdentificacionesService(IIdentificacionesRepository identificacionesRepository)
        {
            _identificacionesRepository = identificacionesRepository;
        }

        public async Task<int> CreateAsync(IdentificacionRequest request, int userId)
        {
            Identificacion identificacion = new Identificacion()
            {
                Descripcion = request.Descripcion,
                Estatus = EstatusIdentificacion.Activo,
                CreatedAt = DateTime.Now,
                CreatedBy = userId
            };

            return await _identificacionesRepository.CreateAsync(identificacion);
        }

        public async Task<List<IdentificacionResponse>> GetAsync(IdentificacionFilterRequest filters)
        {
            List<Identificacion> identificaciones = await _identificacionesRepository.GetAsync(filters);

            return identificaciones.Select(i => new IdentificacionResponse()
            {
                Id = i.Id,
                Descripcion = i.Descripcion,
                Estatus = i.Estatus
            }).ToList();
        }

        public async Task<IdentificacionResponse> GetByIdAsync(int id)
        {
            Identificacion identificacion = await _identificacionesRepository.GetByIdAsync(id);

            IdentificacionResponse response = new IdentificacionResponse()
            {
                Id = identificacion.Id,
                Descripcion = identificacion.Descripcion,
                Estatus = identificacion.Estatus
            };

            return response;
        }

        public async Task<int> UpdateAsync(int id, IdentificacionRequest request, int userId)
        {
            Identificacion identificacion = await _identificacionesRepository.GetByIdAsync(id);

            identificacion.Descripcion = request.Descripcion;
            identificacion.Estatus = request.Estatus;
            identificacion.ModifyAt = DateTime.Now;
            identificacion.ModifyBy = userId;

            return await _identificacionesRepository.UpdateAsync(identificacion);
        }

        public async Task<int> DeleteAsync(int id, int userId)
        {
            Identificacion identificacion = await _identificacionesRepository.GetByIdAsync(id);

            identificacion.Estatus = EstatusIdentificacion.Inactivo;
            identificacion.ModifyAt = DateTime.Now;
            identificacion.ModifyBy = userId;

            return await _identificacionesRepository.UpdateAsync(identificacion);
        }
    }
}
