using UHabitacionalAPI.Application.Interfaces;
using UHabitacionalAPI.Domain.Entities;
using UHabitacionalAPI.Presentation.Dtos;

namespace UHabitacionalAPI.Application.Services
{
    public class EdificiosService : IEdificiosService
    {
        private readonly IEdificiosRepository _edificiosRepository;
        public EdificiosService(IEdificiosRepository edificiosRepository)
        {
            _edificiosRepository = edificiosRepository;
        }

        public async Task<string> CreateAsync(EdificioRequest request, int userId)
        {
            Edificio edificio = new Edificio()
            {
                Calle = request.Calle,
                TotalDeptos = request.TotalDeptos,
                NumeroPisos = request.NumeroPisos,
                Estatus = request.Estatus,
                CreatedAt = DateTime.Now,
                CreatedBy = userId
            };

            return await _edificiosRepository.CreateAsync(edificio);
        }

        public async Task<List<EdificioResponse>> GetAsync(EdificioFilterRequest filters)
        {
            return await _edificiosRepository.GetAsync(filters);
        }

        public async Task<EdificioResponse> GetByIdAsync(string id)
        {
            return await _edificiosRepository.GetByIdAsync(id);
        }

        public async Task<int> UpdateAsync(string id, EdificioRequest request, int userId)
        {
            return await _edificiosRepository.UpdateAsync(id, request, userId);
        }
    }
}
