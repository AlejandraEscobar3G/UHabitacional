using UHabitacionalAPI.Application.Interfaces;
using UHabitacionalAPI.Domain.Entities;
using UHabitacionalAPI.Domain.Exceptions;
using UHabitacionalAPI.Infrastructure.Repositories;
using UHabitacionalAPI.Presentation.Dtos;

namespace UHabitacionalAPI.Application.Services
{
    public class DepartamentosService : IDepartamentosService
    {
        private readonly IDepartamentosRepository _repository;
        private readonly IEdificiosRepository _edificioRepository;
        public DepartamentosService(IDepartamentosRepository repository, IEdificiosRepository edificioRepository)
        {
            _repository = repository;
            _edificioRepository = edificioRepository;
        }

        public async Task<int> CreateAsync(DepartamentoRequest request, int userId)
        {
            Edificio edificio = await _edificioRepository.GetByIdAsync(request.EdificioId);
            int totalDepartamentos = await _edificioRepository.GetTotalDepartamentos(request.EdificioId);

            // Validando espacios disponibles
            if (edificio.TotalDeptos < totalDepartamentos + 1)
            {
                throw new UhRuleViolationException("El edificio ya alcanzó su capacidad máxima de departamentos.");
            }

            // Validando piso del departamento
            if (request.Piso > edificio.NumeroPisos || request.Piso < 0)
            {
                throw new UhRuleViolationException($"El piso {request.Piso} no existe en el edificio {request.EdificioId}.");
            }

            Departamento departamento = new Departamento()
            {
                NumeroInt = request.NumeroInt,
                Piso = request.Piso,
                EdificioId = request.EdificioId,
                CreatedAt = DateTime.Now,
                CreatedBy = userId
            };

            return await _repository.CreateAsync(departamento);
        }

        public async Task<List<DepartamentoResponse>> GetAsync(DepartamentoFilterRequest filters)
        {
            List<Departamento> departamentos = await _repository.GetAsync(filters);

            return departamentos.Select(d => new DepartamentoResponse()
            {
                Id = d.Id,
                NumeroInt = d.NumeroInt,
                Piso = d.Piso,
                Edificio = d.Edificio == null ? null : new EdificioResponse()
                {
                    Id = d.Edificio.Id,
                    Calle = d.Edificio.Calle,
                    Estatus = d.Edificio.Estatus,
                    NumeroPisos = d.Edificio.NumeroPisos,
                    TotalDeptos = d.Edificio.TotalDeptos
                }
            }).ToList();
        }

        public async Task<DepartamentoResponse> GetByIdAsync(int id)
        {
            Departamento departamento = await _repository.GetByIdAsync(id);
            DepartamentoResponse response = new DepartamentoResponse()
            {
                Id = departamento.Id,
                NumeroInt = departamento.NumeroInt,
                Piso = departamento.Piso,
                Edificio = departamento.Edificio == null ? null : new EdificioResponse()
                {
                    Id = departamento.Edificio.Id,
                    Calle = departamento.Edificio.Calle,
                    Estatus = departamento.Edificio.Estatus,
                    NumeroPisos = departamento.Edificio.NumeroPisos,
                    TotalDeptos = departamento.Edificio.TotalDeptos
                }
            };

            return response;
        }

        public async Task<int> UpdateAsync(int id, DepartamentoRequest request, int userId)
        {
            throw new NotImplementedException();
        }
    }
}
