using UHabitacionalAPI.Presentation.Dtos;

namespace UHabitacionalAPI.Application.Interfaces
{
    public interface IEdificiosService
    {
        Task<List<EdificioResponse>> GetAsync(EdificioFilterRequest filters);
        Task<EdificioResponse> GetByIdAsync(string id);
        Task<string> CreateAsync(EdificioRequest request, int userId);
        Task<int> UpdateAsync(string id, EdificioRequest request, int userId);
    }
}
