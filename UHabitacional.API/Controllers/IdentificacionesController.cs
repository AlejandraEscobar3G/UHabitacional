using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UHabitacional.API.Application.DTOs;
using UHabitacional.API.Application.Interfaces.Services;

namespace UHabitacional.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
public class IdentificacionesController : ControllerBase
{
    private readonly IIdentificacionService _service;

    public IdentificacionesController(IIdentificacionService service) => _service = service;

    /// <summary>Obtiene todas las identificaciones activas.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<IdentificacionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<IdentificacionDto>>> GetAll(CancellationToken ct) =>
        Ok(await _service.GetAllAsync(ct));

    /// <summary>Obtiene una identificación por su Id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(IdentificacionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IdentificacionDto>> GetById(int id, CancellationToken ct) =>
        Ok(await _service.GetByIdAsync(id, ct));

    /// <summary>Crea una nueva identificación. (Solo Administrador)</summary>
    [HttpPost]
    [ProducesResponseType(typeof(IdentificacionDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IdentificacionDto>> Create(
        [FromBody] IdentificacionCreateDto dto, CancellationToken ct)
    {
        var created = await _service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.IdIdentificacion }, created);
    }

    /// <summary>Actualiza una identificación. (Solo Administrador)</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(IdentificacionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IdentificacionDto>> Update(
        int id, [FromBody] IdentificacionUpdateDto dto, CancellationToken ct) =>
        Ok(await _service.UpdateAsync(id, dto, ct));

    /// <summary>Elimina (lógico) una identificación. (Solo Administrador)</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _service.DeleteAsync(id, ct);
        return NoContent();
    }
}
