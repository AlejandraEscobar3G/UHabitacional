using Microsoft.AspNetCore.Mvc;
using UHabitacionalAPI.Remote.DTOs;
using UHabitacionalAPI.Remote.Interfaces;

namespace AppServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DragonBallController : ControllerBase
    {
        private readonly IDragonBallService _dragonBallService;

        public DragonBallController(IDragonBallService dragonBallService)
        {
            _dragonBallService = dragonBallService;
        }

        [HttpGet]
        public async Task<ActionResult<DragonBallCharactersResponse>> GetCharacters([FromQuery] int? page, [FromQuery] int? limit)
        {
            var charactersResponse = await _dragonBallService.GetAllCharactersAsync(page, limit);

            if (charactersResponse == null || charactersResponse.Items.Count == 0)
                return NotFound();

            return Ok(charactersResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DragonBallCharacterResponse>> GetCharacter(int id)
        {
            var character = await _dragonBallService.GetCharacterAsync(id);

            if (character == null)
                return NotFound();

            return Ok(character);
        }

    }
}
