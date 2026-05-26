using Microsoft.AspNetCore.Mvc;
using UHabitacional.MVC.Models;
using UHabitacional.MVC.Services;
using UHabitacional.MVC.ViewModels;

namespace UHabitacional.MVC.Controllers;

/// <summary>
/// Gestión de visitantes del inquilino logueado.
/// El backend deriva el inquilino del usuario autenticado.
/// </summary>
[AuthorizeRole("Inquilino,Administrador")]
public class VisitantesController : Controller
{
    private readonly IApiClient _api;
    private readonly ICurrentUser _current;
    public VisitantesController(IApiClient api, ICurrentUser current) { _api = api; _current = current; }

    private void Crumbs(string? last = null)
    {
        var list = new List<BreadcrumbItem>
        {
            new() { Label = "Home", Url = Url.Action("Index","Home") },
            new() { Label = "Mis visitantes", Url = Url.Action("Index","Visitantes") }
        };
        if (last != null) list.Add(new BreadcrumbItem { Label = last });
        ViewBag.Breadcrumbs = list;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.PageTitle = "Mis visitantes";
        Crumbs();
        try
        {
            var todos = await _api.GetListAsync<BitacoraVisitanteDto>("api/bitacora-visitante");
            return View(todos.OrderByDescending(v => v.FechaCreacion).ToList());
        }
        catch (ApiException ex) { TempData["Error"] = ex.Message; return View(new List<BitacoraVisitanteDto>()); }
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.PageTitle = "Nuevo visitante";
        Crumbs("Nuevo");
        await CargarCatalogos();
        return View("Form", new BitacoraVisitanteCreateDto());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BitacoraVisitanteCreateDto dto)
    {
        ViewBag.PageTitle = "Nuevo visitante";
        Crumbs("Nuevo");
        if (!ModelState.IsValid) { await CargarCatalogos(); return View("Form", dto); }
        try
        {
            var creado = await _api.PostAsync<BitacoraVisitanteDto>("api/bitacora-visitante", dto);
            TempData["Success"] = $"Visitante registrado. Código de visita: {creado?.CodigoVisita}";
            return RedirectToAction(nameof(Detail), new { id = creado?.IdBitacoraVisitante });
        }
        catch (ApiException ex) { ModelState.AddModelError("", ex.Message); await CargarCatalogos(); return View("Form", dto); }
    }

    public async Task<IActionResult> Edit(int id)
    {
        ViewBag.PageTitle = "Editar visitante";
        Crumbs("Editar");
        try
        {
            var v = await _api.GetAsync<BitacoraVisitanteDto>($"api/bitacora-visitante/{id}");
            if (v == null) return NotFound();
            ViewBag.Id = id;
            ViewBag.CodigoExistente = v.CodigoVisita;
            await CargarCatalogos();
            return View("Form", new BitacoraVisitanteCreateDto
            {
                NombreVisitante = v.NombreVisitante,
                IdIdentificacion = v.IdIdentificacion,
                NumeroIdentificacion = v.NumeroIdentificacion,
                Observaciones = v.Observaciones
            });
        }
        catch (ApiException ex) { TempData["Error"] = ex.Message; return RedirectToAction(nameof(Index)); }
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, BitacoraVisitanteCreateDto dto)
    {
        ViewBag.PageTitle = "Editar visitante";
        Crumbs("Editar");
        ViewBag.Id = id;
        if (!ModelState.IsValid) { await CargarCatalogos(); return View("Form", dto); }
        try
        {
            var upd = new BitacoraVisitanteUpdateDto
            {
                NombreVisitante = dto.NombreVisitante,
                IdIdentificacion = dto.IdIdentificacion,
                NumeroIdentificacion = dto.NumeroIdentificacion,
                Observaciones = dto.Observaciones
            };
            await _api.PutAsync<BitacoraVisitanteDto>($"api/bitacora-visitante/{id}", upd);
            TempData["Success"] = "Visitante actualizado.";
            return RedirectToAction(nameof(Index));
        }
        catch (ApiException ex) { ModelState.AddModelError("", ex.Message); await CargarCatalogos(); return View("Form", dto); }
    }

    public async Task<IActionResult> Detail(int id)
    {
        ViewBag.PageTitle = "Detalle de visitante";
        Crumbs("Detalle");
        try
        {
            var v = await _api.GetAsync<BitacoraVisitanteDto>($"api/bitacora-visitante/{id}");
            if (v == null) return NotFound();
            return View(v);
        }
        catch (ApiException ex) { TempData["Error"] = ex.Message; return RedirectToAction(nameof(Index)); }
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try { await _api.DeleteAsync($"api/bitacora-visitante/{id}"); TempData["Success"] = "Visitante eliminado."; }
        catch (ApiException ex) { TempData["Error"] = ex.Message; }
        return RedirectToAction(nameof(Index));
    }

    private async Task CargarCatalogos()
    {
        try { ViewBag.Identificaciones = await _api.GetListAsync<IdentificacionDto>("api/identificaciones"); }
        catch { ViewBag.Identificaciones = new List<IdentificacionDto>(); }
    }
}
