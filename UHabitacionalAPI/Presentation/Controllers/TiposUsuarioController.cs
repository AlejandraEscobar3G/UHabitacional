using Microsoft.AspNetCore.Mvc;
using UHabitacionalAPI.Application.Interfaces;
using UHabitacionalAPI.Domain.Entities;
using UHabitacionalAPI.Presentation.Dtos;

namespace UHabitacionalAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TiposUsuarioController : ControllerBase
    {
        private readonly ILogger<TiposUsuarioController> _logger;
        private readonly ITiposUsuarioService _tiposUsuarioService;
        public TiposUsuarioController(ILogger<TiposUsuarioController> logger, ITiposUsuarioService repository)
        {
            _logger = logger;
            _tiposUsuarioService = repository;
        }

        /// <summary>
        /// Obtiene la lista de tipos de usuario filtrados.
        /// </summary>
        /// <param name="filters">Filtros de búsqueda.</param>
        /// <returns>Lista de tipos de usuario.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<TipoUsuarioResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<List<TipoUsuarioResponse>>>>
            Get([FromQuery] TipoUsuarioFilterRequest filters)
        {
            _logger.LogInformation("Consultando lista de tipos de usuario...");

            List<TipoUsuarioResponse> tiposUsuario = await _tiposUsuarioService.GetAsync(filters);

            _logger.LogInformation($"Se obtuvieron {tiposUsuario.Count} tipos de usuario");
            return Ok(ApiResponse<List<TipoUsuarioResponse>>.Ok(tiposUsuario));
        }

        /// <summary>
        /// Obtiene un tipo de usuario por su identificador único.
        /// </summary>
        /// <param name="id">Identificador del tipo de usuario.</param>
        /// <returns>El tipo de usuario encontrada.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<TipoUsuarioResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<TipoUsuarioResponse>>> GetById(int id)
        {
            _logger.LogInformation($"Consultando tipo de usuario con id {id}");

            TipoUsuarioResponse? response = await _tiposUsuarioService.GetByIdAsync(id);

            if (response == null)
            {
                _logger.LogInformation("Tipo de usuario no encontrado");
            }

            return Ok(ApiResponse<TipoUsuarioResponse>.Ok(response));
        }

        /// <summary>
        /// Crea un nuevo tipo de usuario en la base de datos.
        /// </summary>
        /// <param name="request">Datos del tipo de usuario a crear.</param>
        /// <returns>El identificador del tipo de usuario creado.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<string>>> Create([FromBody] TipoUsuarioRequest request)
        {
            _logger.LogInformation("Creación de tipo de usuario...");

            int userId = 123;

            int tipoUsuarioId = await _tiposUsuarioService.CreateAsync(request, userId);

            _logger.LogInformation($"Tipo de usuario creado {tipoUsuarioId}");
            return CreatedAtAction(nameof(GetById), new { tipoUsuarioId }, ApiResponse<string>.Created("Tipo de usuario creado con éxito"));
        }

        /// <summary>
        /// Actualiza un tipo de usuario existente.
        /// </summary>
        /// <param name="id">Identificador del tipo de usuario a actualizar.</param>
        /// <param name="request">Datos actualizados del tipo de usuario.</param>
        /// <returns>Mensaje de confirmación.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<string>>> Update(int id, [FromBody] TipoUsuarioRequest request)
        {
            int userId = 123;

            int result = await _tiposUsuarioService.UpdateAsync(id, request, userId);

            if (result == 0)
            {
                _logger.LogInformation("Tipo de usuario no encontrado");
                return NotFound(ApiResponse<string>.Fail(
                    StatusCodes.Status404NotFound,
                    $"No se pudo actualizar el tipo de usuario con ID {id}.",
                    new List<string> { $"No se pudo actualizar el tipo de usuario con ID {id}." }
                ));
            }

            _logger.LogInformation($"Tipo de usuario con ID {id} actualizado");
            return Ok(ApiResponse<string>.Created("Actualizada con éxito"));
        }

        /// <summary>
        /// Elimina un tipo de usuario existente.
        /// </summary>
        /// <param name="id">Identificador del tipo de usuario a eliminar.</param>
        /// <returns>Mensaje de confirmación.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<string>>> Delete(int id)
        {
            int userId = 123;

            int result = await _tiposUsuarioService.DeleteAsync(id, userId);

            if (result == 0)
            {
                _logger.LogInformation("Tipo de usuario no encontrado");
                return NotFound(ApiResponse<string>.Fail(
                    StatusCodes.Status404NotFound,
                    $"No se pudo eliminar el tipo de usuario con ID {id}.",
                    new List<string> { $"No se pudo eliminar el tipo de usuario con ID {id}." }
                ));
            }

            _logger.LogInformation($"Tipo de usuario con ID {id} eliminado");
            return Ok(ApiResponse<string>.Created("Eliminada con éxito"));
        }
    }
}
