using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UHabitacional.API.Application.DTOs;
using UHabitacional.API.Application.Interfaces.Services;

namespace UHabitacional.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
public class EdificiosController : ControllerBase
{
    private readonly IEdificioService _service;

    public EdificiosController(IEdificioService service) => _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EdificioDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<EdificioDto>>> GetAll(CancellationToken ct) =>
        Ok(await _service.GetAllAsync(ct));

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(EdificioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EdificioDto>> GetById(int id, CancellationToken ct) =>
        Ok(await _service.GetByIdAsync(id, ct));

    [HttpPost]
    [ProducesResponseType(typeof(EdificioDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<EdificioDto>> Create(
        [FromBody] EdificioCreateDto dto, CancellationToken ct)
    {
        var created = await _service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.IdEdificio }, created);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(EdificioDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<EdificioDto>> Update(
        int id, [FromBody] EdificioUpdateDto dto, CancellationToken ct) =>
        Ok(await _service.UpdateAsync(id, dto, ct));
}
