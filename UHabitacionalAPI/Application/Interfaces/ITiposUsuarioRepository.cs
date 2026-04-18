using UHabitacionalAPI.Domain.Entities;
using UHabitacionalAPI.Presentation.Dtos;

namespace UHabitacionalAPI.Application.Interfaces
{
    public interface ITiposUsuarioRepository
    {
        Task<int> CreateAsync(TipoUsuario tipoUsuario);
        Task<List<TipoUsuario>> GetAsync(TipoUsuarioFilterRequest tipoUsuarioFilterRequest);
        Task<TipoUsuario> GetByIdAsync(int id);
        Task<int> UpdateAsync(TipoUsuario request);
    }
}
