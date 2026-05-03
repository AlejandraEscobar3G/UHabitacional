using UHabitacionalAPI.Presentation.Dtos;

namespace UHabitacionalAPI.Application.Interfaces
{
    public interface IDepartamentosService
    {
        Task<List<DepartamentoResponse>> GetAsync(DepartamentoFilterRequest filters);
        Task<DepartamentoResponse> GetByIdAsync(int id);
        Task<int> CreateAsync(DepartamentoRequest request, int userId);
        Task<int> UpdateAsync(int id, DepartamentoRequest request, int userId);
    }
}
