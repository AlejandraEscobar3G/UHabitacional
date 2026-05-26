using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UHabitacional.API.Application.DTOs;
using UHabitacional.API.Application.Interfaces.Services;

namespace UHabitacional.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
public class DepartamentosController : ControllerBase
{
    private readonly IDepartamentoService _service;

    public DepartamentosController(IDepartamentoService service) => _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<DepartamentoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DepartamentoDto>>> GetAll(CancellationToken ct) =>
        Ok(await _service.GetAllAsync(ct));

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(DepartamentoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DepartamentoDto>> GetById(int id, CancellationToken ct) =>
        Ok(await _service.GetByIdAsync(id, ct));

    [HttpPost]
    [ProducesResponseType(typeof(DepartamentoDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<DepartamentoDto>> Create(
        [FromBody] DepartamentoCreateDto dto, CancellationToken ct)
    {
        var created = await _service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.IdDepartamento }, created);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(DepartamentoDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<DepartamentoDto>> Update(
        int id, [FromBody] DepartamentoUpdateDto dto, CancellationToken ct) =>
        Ok(await _service.UpdateAsync(id, dto, ct));
}
