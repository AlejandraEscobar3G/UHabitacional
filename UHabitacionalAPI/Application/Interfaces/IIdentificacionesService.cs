using UHabitacionalAPI.Presentation.Dtos;

namespace UHabitacionalAPI.Application.Interfaces
{
    public interface IIdentificacionesService
    {
        Task<List<IdentificacionResponse>> GetAsync(IdentificacionFilterRequest filters);
        Task<IdentificacionResponse> GetByIdAsync(int id);
        Task<int> CreateAsync(IdentificacionRequest request, int userId);
        Task<int> UpdateAsync(int id, IdentificacionRequest request, int userId);
    }
}
