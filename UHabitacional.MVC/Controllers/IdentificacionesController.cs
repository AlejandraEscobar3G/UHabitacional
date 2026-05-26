using Microsoft.AspNetCore.Mvc;
using UHabitacional.MVC.Models;
using UHabitacional.MVC.Services;
using UHabitacional.MVC.ViewModels;

namespace UHabitacional.MVC.Controllers;

[AuthorizeRole("Administrador")]
public class IdentificacionesController : Controller
{
    private readonly IApiClient _api;
    public IdentificacionesController(IApiClient api) => _api = api;

    private void Crumbs(string? last = null)
    {
        var list = new List<BreadcrumbItem>
        {
            new() { Label = "Home", Url = Url.Action("Index","Home") },
            new() { Label = "Identificaciones", Url = Url.Action("Index","Identificaciones") }
        };
        if (last != null) list.Add(new BreadcrumbItem { Label = last });
        ViewBag.Breadcrumbs = list;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.PageTitle = "Identificaciones";
        Crumbs();
        try { return View(await _api.GetListAsync<IdentificacionDto>("api/identificaciones")); }
        catch (ApiException ex) { TempData["Error"] = ex.Message; return View(new List<IdentificacionDto>()); }
    }

    public IActionResult Create()
    {
        ViewBag.PageTitle = "Nuevo tipo de identificación";
        Crumbs("Nuevo");
        return View("Form", new IdentificacionCreateDto());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(IdentificacionCreateDto dto)
    {
        ViewBag.PageTitle = "Nuevo tipo de identificación";
        Crumbs("Nuevo");
        if (!ModelState.IsValid) return View("Form", dto);
        try
        {
            await _api.PostAsync<IdentificacionDto>("api/identificaciones", dto);
            TempData["Success"] = "Identificación creada exitosamente.";
            return RedirectToAction(nameof(Index));
        }
        catch (ApiException ex) { ModelState.AddModelError("", ex.Message); return View("Form", dto); }
    }

    public async Task<IActionResult> Edit(int id)
    {
        ViewBag.PageTitle = "Editar identificación";
        Crumbs("Editar");
        try
        {
            var ed = await _api.GetAsync<IdentificacionDto>($"api/identificaciones/{id}");
            if (ed == null) return NotFound();
            ViewBag.Id = id;
            return View("Form", new IdentificacionCreateDto { Nombre = ed.Nombre, Descripcion = ed.Descripcion });
        }
        catch (ApiException ex) { TempData["Error"] = ex.Message; return RedirectToAction(nameof(Index)); }
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, IdentificacionCreateDto dto)
    {
        ViewBag.PageTitle = "Editar identificación";
        Crumbs("Editar");
        ViewBag.Id = id;
        if (!ModelState.IsValid) return View("Form", dto);
        try
        {
            await _api.PutAsync<IdentificacionDto>($"api/identificaciones/{id}", dto);
            TempData["Success"] = "Identificación actualizada.";
            return RedirectToAction(nameof(Index));
        }
        catch (ApiException ex) { ModelState.AddModelError("", ex.Message); return View("Form", dto); }
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try { await _api.DeleteAsync($"api/identificaciones/{id}"); TempData["Success"] = "Identificación eliminada."; }
        catch (ApiException ex) { TempData["Error"] = ex.Message; }
        return RedirectToAction(nameof(Index));
    }
}
