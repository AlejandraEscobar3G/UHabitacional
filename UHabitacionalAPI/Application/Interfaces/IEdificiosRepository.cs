using UHabitacionalAPI.Domain.Entities;
using UHabitacionalAPI.Presentation.Dtos;

namespace UHabitacionalAPI.Application.Interfaces
{
    public interface IEdificiosRepository
    {
        Task<string> CreateAsync(Edificio edificio);
        Task<List<Edificio>> GetAsync(EdificioFilterRequest filters);
        Task<Edificio> GetByIdAsync(string id);
        Task<int> UpdateAsync(Edificio request);
    }
}