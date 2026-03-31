using UHabitacionalAPI.Domain.Entities;
using UHabitacionalAPI.Presentation.Dtos;

namespace UHabitacionalAPI.Application.Interfaces
{
    public interface IEdificiosRepository
    {
        Task<string> CreateAsync(Edificio edificio);
        Task<List<EdificioResponse>> GetAsync(EdificioFilterRequest filters);
        Task<EdificioResponse> GetByIdAsync(string id);
        Task<int> UpdateAsync(string id, EdificioRequest request, int userId);
    }
}