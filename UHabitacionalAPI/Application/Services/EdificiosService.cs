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
                Id = request.Id,
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
            List<Edificio> edificios = await _edificiosRepository.GetAsync(filters);

            return edificios.Select(e => new EdificioResponse()
            {
                Id = e.Id,
                Calle = e.Calle,
                Estatus = e.Estatus,
                NumeroPisos = e.NumeroPisos,
                TotalDeptos = e.TotalDeptos
            }).ToList();
        }

        public async Task<EdificioResponse> GetByIdAsync(string id)
        {
            Edificio edificio = await _edificiosRepository.GetByIdAsync(id);
            EdificioResponse response = new EdificioResponse()
            {
                Id = edificio.Id,
                Calle = edificio.Calle,
                Estatus = edificio.Estatus,
                NumeroPisos = edificio.NumeroPisos,
                TotalDeptos = edificio.TotalDeptos
            };

            return response;
        }

        public async Task<int> UpdateAsync(string id, EdificioRequest request, int userId)
        {
            Edificio edificio = await _edificiosRepository.GetByIdAsync(id);

            edificio.Calle = request.Calle;
            edificio.Estatus = request.Estatus;
            edificio.NumeroPisos = request.NumeroPisos;
            edificio.TotalDeptos = request.TotalDeptos;
            edificio.ModifyAt = DateTime.Now;
            edificio.ModifyBy = userId;

            return await _edificiosRepository.UpdateAsync(edificio);
        }
    }
}
