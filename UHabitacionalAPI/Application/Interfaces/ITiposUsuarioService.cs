using UHabitacionalAPI.Presentation.Dtos;

namespace UHabitacionalAPI.Application.Interfaces
{
    public interface ITiposUsuarioService
    {
        Task<List<TipoUsuarioResponse>> GetAsync(TipoUsuarioFilterRequest filters);
        Task<TipoUsuarioResponse> GetByIdAsync(int id);
        Task<int> CreateAsync(TipoUsuarioRequest request, int userId);
        Task<int> UpdateAsync(int id, TipoUsuarioRequest request, int userId);
        Task<int> DeleteAsync(int id, int userId);
    }
}
