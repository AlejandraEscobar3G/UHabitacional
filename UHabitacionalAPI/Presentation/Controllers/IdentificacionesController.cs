using Microsoft.AspNetCore.Mvc;
using UHabitacionalAPI.Application.Interfaces;
using UHabitacionalAPI.Application.Services;
using UHabitacionalAPI.Presentation.Dtos;

namespace UHabitacionalAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentificacionesController : ControllerBase
    {
        private readonly IIdentificacionesService _identificacionesService;
        public IdentificacionesController(IIdentificacionesService identificacionesService)
        {
            _identificacionesService = identificacionesService;
        }

        /// <summary>
        /// Obtiene la lista de identificaciones filtradas.
        /// </summary>
        /// <param name="filters">Filtros de búsqueda.</param>
        /// <returns>Lista de identificaciones.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<IdentificacionResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<List<IdentificacionResponse>>>>
            Get([FromQuery] IdentificacionFilterRequest filters)
        {
            List<IdentificacionResponse> identificaciones = await _identificacionesService.GetAsync(filters);

            return Ok(ApiResponse<List<IdentificacionResponse>>.Ok(identificaciones));
        }

        /// <summary>
        /// Obtiene una identificación por su identificador único.
        /// </summary>
        /// <param name="id">Identificador de la identificación.</param>
        /// <returns>La identificación encontrada.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<IdentificacionResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IdentificacionResponse>>> GetById(int id)
        {
            IdentificacionResponse? identificacion = await _identificacionesService.GetByIdAsync(id);

            return Ok(ApiResponse<IdentificacionResponse>.Ok(identificacion));
        }

        /// <summary>
        /// Crea una nueva identificación en la base de datos.
        /// </summary>
        /// <param name="request">Datos de la identificación a crear.</param>
        /// <returns>El identificador de la identificación creada.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<string>>> Create([FromBody] IdentificacionRequest request)
        {
            if (!ModelState.IsValid)
            {
                List<string> errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(ApiResponse<string>.Fail(StatusCodes.Status400BadRequest, "Errores de validación", errors));
            }

            int userId = 123;

            int identificacionId = await _identificacionesService.CreateAsync(request, userId);
            return CreatedAtAction(nameof(GetById), new { identificacionId }, ApiResponse<string>.Created("Identificación creada con éxito"));
        }

        /// <summary>
        /// Actualiza una identificación existente.
        /// </summary>
        /// <param name="id">Identificador de la identificación a actualizar.</param>
        /// <param name="request">Datos actualizados de la identificación.</param>
        /// <returns>Mensaje de confirmación.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<string>>> Update(int id, [FromBody] IdentificacionRequest request)
        {
            if (!ModelState.IsValid)
            {
                List<string> errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(ApiResponse<string>.Fail(StatusCodes.Status400BadRequest, "Errores de validación", errors));
            }

            int userId = 123;

            int result = await _identificacionesService.UpdateAsync(id, request, userId);

            if (result == 0)
            {
                return NotFound(ApiResponse<string>.Fail(
                    StatusCodes.Status404NotFound,
                    $"No se pudo actualizar la identificación con ID {id}.",
                    new List<string> { $"No se pudo actualizar la identificación con ID {id}." }
                ));
            }

            return Ok(ApiResponse<string>.Created("Actualizada con éxito"));
        }
    }
}
