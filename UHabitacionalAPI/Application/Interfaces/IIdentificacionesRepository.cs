using UHabitacionalAPI.Domain.Entities;
using UHabitacionalAPI.Presentation.Dtos;

namespace UHabitacionalAPI.Application.Interfaces
{
    public interface IIdentificacionesRepository
    {
        Task<int> CreateAsync(Identificacion identificacion);
        Task<List<Identificacion>> GetAsync(IdentificacionFilterRequest filter);
        Task<Identificacion> GetByIdAsync(int id);
        Task<int> UpdateAsync(Identificacion request);
    }
}
