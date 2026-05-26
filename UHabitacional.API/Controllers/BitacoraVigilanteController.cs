using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UHabitacional.API.Application.DTOs;
using UHabitacional.API.Application.Interfaces.Services;

namespace UHabitacional.API.Controllers;

[ApiController]
[Route("api/bitacora-vigilante")]
[Produces("application/json")]
[Authorize]
public class BitacoraVigilanteController : ControllerBase
{
    private readonly IBitacoraVigilanteService _service;

    public BitacoraVigilanteController(IBitacoraVigilanteService service) => _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BitacoraVigilanteDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<BitacoraVigilanteDto>>> GetAll(CancellationToken ct) =>
        Ok(await _service.GetAllAsync(ct));

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(BitacoraVigilanteDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<BitacoraVigilanteDto>> GetById(int id, CancellationToken ct) =>
        Ok(await _service.GetByIdAsync(id, ct));

    /// <summary>Registra una nueva entrada de vigilancia. (Solo Vigilante)</summary>
    [HttpPost]
    [ProducesResponseType(typeof(BitacoraVigilanteDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<BitacoraVigilanteDto>> Create(
        [FromBody] BitacoraVigilanteCreateDto dto, CancellationToken ct)
    {
        var created = await _service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.IdBitacoraVigilante }, created);
    }

    /// <summary>Cierra la entrada de vigilancia (registro de salida). (Solo Vigilante)</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(BitacoraVigilanteDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<BitacoraVigilanteDto>> Update(
        int id, [FromBody] BitacoraVigilanteUpdateDto dto, CancellationToken ct) =>
        Ok(await _service.UpdateAsync(id, dto, ct));
}
