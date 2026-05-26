using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UHabitacional.API.Application.DTOs;
using UHabitacional.API.Application.Interfaces.Services;

namespace UHabitacional.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
public class InquilinosController : ControllerBase
{
    private readonly IInquilinoService _service;

    public InquilinosController(IInquilinoService service) => _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<InquilinoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<InquilinoDto>>> GetAll(CancellationToken ct) =>
        Ok(await _service.GetAllAsync(ct));

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(InquilinoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<InquilinoDto>> GetById(int id, CancellationToken ct) =>
        Ok(await _service.GetByIdAsync(id, ct));

    [HttpPost]
    [ProducesResponseType(typeof(InquilinoDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<InquilinoDto>> Create(
        [FromBody] InquilinoCreateDto dto, CancellationToken ct)
    {
        var created = await _service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.IdInquilino }, created);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(InquilinoDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<InquilinoDto>> Update(
        int id, [FromBody] InquilinoUpdateDto dto, CancellationToken ct) =>
        Ok(await _service.UpdateAsync(id, dto, ct));

    /// <summary>
    /// Actualiza el campo FechaFin para cambiar el estado de un inquilino.
    /// Solo el Administrador puede hacerlo.
    /// </summary>
    [HttpPatch("{id:int}/fecha-fin")]
    [ProducesResponseType(typeof(InquilinoDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<InquilinoDto>> UpdateFechaFin(
        int id, [FromBody] InquilinoFechaFinDto dto, CancellationToken ct) =>
        Ok(await _service.UpdateFechaFinAsync(id, dto, ct));

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _service.DeleteAsync(id, ct);
        return NoContent();
    }
}
