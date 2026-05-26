using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UHabitacional.API.Application.DTOs;
using UHabitacional.API.Application.Interfaces.Services;

namespace UHabitacional.API.Controllers;

[ApiController]
[Route("api/bitacora-visitante")]
[Produces("application/json")]
[Authorize]
public class BitacoraVisitanteController : ControllerBase
{
    private readonly IBitacoraVisitanteService _service;

    public BitacoraVisitanteController(IBitacoraVisitanteService service) => _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BitacoraVisitanteDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<BitacoraVisitanteDto>>> GetAll(CancellationToken ct) =>
        Ok(await _service.GetAllAsync(ct));

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(BitacoraVisitanteDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<BitacoraVisitanteDto>> GetById(int id, CancellationToken ct) =>
        Ok(await _service.GetByIdAsync(id, ct));

    /// <summary>Registra un nuevo visitante. (Solo Inquilino)</summary>
    [HttpPost]
    [ProducesResponseType(typeof(BitacoraVisitanteDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<BitacoraVisitanteDto>> Create(
        [FromBody] BitacoraVisitanteCreateDto dto, CancellationToken ct)
    {
        var created = await _service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.IdBitacoraVisitante }, created);
    }

    /// <summary>Actualiza los datos del visitante. (Solo el Inquilino que lo registró)</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(BitacoraVisitanteDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<BitacoraVisitanteDto>> Update(
        int id, [FromBody] BitacoraVisitanteUpdateDto dto, CancellationToken ct) =>
        Ok(await _service.UpdateAsync(id, dto, ct));

    /// <summary>Registra entrada o salida del visitante. (Solo Vigilante)</summary>
    [HttpPatch("{id:int}/registro")]
    [ProducesResponseType(typeof(BitacoraVisitanteDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<BitacoraVisitanteDto>> RegistrarEntradaSalida(
        int id, [FromBody] BitacoraVisitanteRegistroDto dto, CancellationToken ct) =>
        Ok(await _service.RegistrarEntradaSalidaAsync(id, dto, ct));

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _service.DeleteAsync(id, ct);
        return NoContent();
    }
}
