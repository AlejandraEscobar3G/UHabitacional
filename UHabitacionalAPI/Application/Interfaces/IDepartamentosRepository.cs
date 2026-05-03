using UHabitacionalAPI.Domain.Entities;
using UHabitacionalAPI.Presentation.Dtos;

namespace UHabitacionalAPI.Application.Interfaces
{
    public interface IDepartamentosRepository
    {
        Task<int> CreateAsync(Departamento departamento);
        Task<List<Departamento>> GetAsync(DepartamentoFilterRequest filters);
        Task<Departamento> GetByIdAsync(int id);
        Task<int> UpdateAsync(Departamento departamento);
    }
}
