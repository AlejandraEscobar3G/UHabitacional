using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using UHabitacionalAPI.Application.Interfaces;
using UHabitacionalAPI.Presentation.Dtos;

namespace UHabitacionalAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EdificiosController : ControllerBase
    {
        private readonly IEdificiosService _edificiosService;
        public EdificiosController(IEdificiosService edificiosService)
        {
            _edificiosService = edificiosService;
        }

        [HttpGet]
        public async Task<ActionResult<List<EdificioResponse>>> Get([FromQuery] EdificioFilterRequest filters)
        {
            List<EdificioResponse> edificios = await _edificiosService.GetAsync(filters);

            return Ok(edificios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EdificioResponse>> GetById(string id)
        {
            EdificioResponse? edificio = await _edificiosService.GetByIdAsync(id);

            return Ok(edificio);
        }

        [HttpPost]
        public async Task<ActionResult<string>> Create([FromBody] EdificioRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int userId = 123;

            string edificioId = await _edificiosService.CreateAsync(request, userId);
            return CreatedAtAction(nameof(GetById), new { edificioId }, null);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, [FromBody] EdificioRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int userId = 123;

            int result = await _edificiosService.UpdateAsync(id, request, userId);

            if (result == 0)
            {
                return NotFound($"No se pudo actualizar el edificio con ID {id}.");
            }

            return Ok("Actualizado con éxito");
        }
    }
}
