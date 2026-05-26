using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UHabitacional.API.Application.DTOs;
using UHabitacional.API.Application.Interfaces.Services;

namespace UHabitacional.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
public class TiposUsuarioController : ControllerBase
{
    private readonly ITipoUsuarioService _service;

    public TiposUsuarioController(ITipoUsuarioService service) => _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TipoUsuarioDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TipoUsuarioDto>>> GetAll(CancellationToken ct) =>
        Ok(await _service.GetAllAsync(ct));

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TipoUsuarioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TipoUsuarioDto>> GetById(int id, CancellationToken ct) =>
        Ok(await _service.GetByIdAsync(id, ct));

    [HttpPost]
    [ProducesResponseType(typeof(TipoUsuarioDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<TipoUsuarioDto>> Create(
        [FromBody] TipoUsuarioCreateDto dto, CancellationToken ct)
    {
        var created = await _service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.IdTipoUsuario }, created);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(TipoUsuarioDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<TipoUsuarioDto>> Update(
        int id, [FromBody] TipoUsuarioUpdateDto dto, CancellationToken ct) =>
        Ok(await _service.UpdateAsync(id, dto, ct));

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _service.DeleteAsync(id, ct);
        return NoContent();
    }
}
