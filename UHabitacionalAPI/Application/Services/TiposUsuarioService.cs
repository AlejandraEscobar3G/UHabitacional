using Azure.Core;
using UHabitacionalAPI.Application.Interfaces;
using UHabitacionalAPI.Domain.Entities;
using UHabitacionalAPI.Domain.Enums;
using UHabitacionalAPI.Presentation.Dtos;

namespace UHabitacionalAPI.Application.Services
{
    public class TiposUsuarioService : ITiposUsuarioService
    {
        private readonly ITiposUsuarioRepository _tiposUsuarioRepository;
        public TiposUsuarioService(ITiposUsuarioRepository repository)
        {
            _tiposUsuarioRepository = repository;
        }

        public async Task<int> CreateAsync(TipoUsuarioRequest request, int userId)
        {
            TipoUsuario tipoUsuario = new TipoUsuario()
            {
                Descripcion = request.Descripcion,
                Estatus = request.Estatus,
                CreatedAt = DateTime.Now,
                CreatedBy = userId
            };

            return await _tiposUsuarioRepository.CreateAsync(tipoUsuario);
        }

        public async Task<int> DeleteAsync(int id, int userId)
        {
            TipoUsuario tipoUsuario = await _tiposUsuarioRepository.GetByIdAsync(id);

            tipoUsuario.Estatus = EstatusTipoUsuario.Inactivo;
            tipoUsuario.ModifyAt = DateTime.Now;
            tipoUsuario.ModifyBy = userId;

            return await _tiposUsuarioRepository.UpdateAsync(tipoUsuario);
        }

        public async Task<List<TipoUsuarioResponse>> GetAsync(TipoUsuarioFilterRequest filters)
        {
            List<TipoUsuario> tipoUsuarios = await _tiposUsuarioRepository.GetAsync(filters);

            return tipoUsuarios.Select(u => new TipoUsuarioResponse()
            {
                Id = u.Id,
                Descripcion = u.Descripcion,
                Estatus = u.Estatus
            }).ToList();
        }

        public async Task<TipoUsuarioResponse> GetByIdAsync(int id)
        {
            TipoUsuario tipoUsuario = await _tiposUsuarioRepository.GetByIdAsync(id);

            TipoUsuarioResponse response = new TipoUsuarioResponse()
            {
                Id = tipoUsuario.Id,
                Descripcion = tipoUsuario.Descripcion,
                Estatus = tipoUsuario.Estatus
            };

            return response;
        }

        public async Task<int> UpdateAsync(int id, TipoUsuarioRequest request, int userId)
        {
            TipoUsuario tipoUsuario = await _tiposUsuarioRepository.GetByIdAsync(id);

            tipoUsuario.Descripcion = request.Descripcion;
            tipoUsuario.Estatus = request.Estatus;
            tipoUsuario.ModifyAt = DateTime.Now;
            tipoUsuario.ModifyBy = userId;

            return await _tiposUsuarioRepository.UpdateAsync(tipoUsuario);
        }
    }
}
