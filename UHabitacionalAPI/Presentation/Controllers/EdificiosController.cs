using Microsoft.AspNetCore.Mvc;
using UHabitacionalAPI.Application.Interfaces;
using UHabitacionalAPI.Presentation.Dtos;

namespace UHabitacionalAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EdificiosController : ControllerBase
    {
        private readonly ILogger<EdificiosController> _logger;
        private readonly IEdificiosService _edificiosService;
        public EdificiosController(ILogger<EdificiosController> logger, IEdificiosService edificiosService)
        {
            _edificiosService = edificiosService;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene la lista de edificios filtrados.
        /// </summary>
        /// <param name="filters">Filtros de búsqueda.</param>
        /// <returns>Lista de edificios.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<EdificioResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<List<EdificioResponse>>>> Get([FromQuery] EdificioFilterRequest filters)
        {
            _logger.LogInformation("Consultando lista de edificios...");

            List<EdificioResponse> edificios = await _edificiosService.GetAsync(filters);

            _logger.LogInformation($"Se obtuvieron {edificios.Count} edificios");
            return Ok(ApiResponse<List<EdificioResponse>>.Ok(edificios));
        }

        /// <summary>
        /// Obtiene un edificio por su identificador único.
        /// </summary>
        /// <param name="id">Identificador del edificio.</param>
        /// <returns>El edificio encontrado.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<EdificioResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<EdificioResponse>>> GetById(string id)
        {
            _logger.LogInformation($"Consultando edificio con id {id}");

            EdificioResponse? edificio = await _edificiosService.GetByIdAsync(id);

            if (edificio is null)
            {
                _logger.LogInformation("Edificio no encontrado");
            }

            return Ok(ApiResponse<EdificioResponse>.Ok(edificio));
        }

        /// <summary>
        /// Crea un nuevo edificio en la base de datos.
        /// </summary>
        /// <param name="request">Datos del edificio a crear.</param>
        /// <returns>El identificador del edificio creado.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<string>>> Create([FromBody] EdificioRequest request)
        {
            _logger.LogInformation("Creación de edificio...");
            int userId = 123;

            string edificioId = await _edificiosService.CreateAsync(request, userId);

            _logger.LogInformation($"Edificio creado {edificioId}");
            return CreatedAtAction(nameof(GetById), new { id = edificioId }, ApiResponse<string>.Created("Edificio creado con éxito"));
        }

        /// <summary>
        /// Actualiza un edificio existente.
        /// </summary>
        /// <param name="id">Identificador del edificio a actualizar.</param>
        /// <param name="request">Datos actualizados del edificio.</param>
        /// <returns>Mensaje de confirmación.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<string>>> Update(string id, [FromBody] EdificioRequest request)
        {
            int userId = 123;

            int result = await _edificiosService.UpdateAsync(id, request, userId);

            if (result == 0)
            {
                _logger.LogInformation("Edificio no encontrado");
                return NotFound(ApiResponse<string>.Fail(
                    StatusCodes.Status404NotFound,
                    $"No se pudo actualizar el edificio con ID {id}.",
                    new List<string> { $"No se pudo actualizar el edificio con ID {id}." }
                ));
            }

            _logger.LogInformation($"Edificio con ID {id} actualizado");
            return Ok(ApiResponse<string>.Created("Actualizado con éxito"));
        }
    }
}
