using Microsoft.AspNetCore.Mvc;
using UHabitacional.MVC.Models;
using UHabitacional.MVC.Services;
using UHabitacional.MVC.ViewModels;

namespace UHabitacional.MVC.Controllers;

[AuthorizeRole("Administrador")]
public class PerfilesController : Controller
{
    private readonly IApiClient _api;
    public PerfilesController(IApiClient api) => _api = api;

    private void Crumbs(string? last = null)
    {
        var list = new List<BreadcrumbItem>
        {
            new() { Label = "Home", Url = Url.Action("Index","Home") },
            new() { Label = "Perfiles", Url = Url.Action("Index","Perfiles") }
        };
        if (last != null) list.Add(new BreadcrumbItem { Label = last });
        ViewBag.Breadcrumbs = list;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.PageTitle = "Perfiles";
        Crumbs();
        try { return View(await _api.GetListAsync<TipoUsuarioDto>("api/tiposusuario")); }
        catch (ApiException ex) { TempData["Error"] = ex.Message; return View(new List<TipoUsuarioDto>()); }
    }

    public IActionResult Create()
    {
        ViewBag.PageTitle = "Nuevo perfil";
        Crumbs("Nuevo");
        return View("Form", new TipoUsuarioCreateDto());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TipoUsuarioCreateDto dto)
    {
        ViewBag.PageTitle = "Nuevo perfil";
        Crumbs("Nuevo");
        if (!ModelState.IsValid) return View("Form", dto);
        try
        {
            await _api.PostAsync<TipoUsuarioDto>("api/tiposusuario", dto);
            TempData["Success"] = "Perfil creado.";
            return RedirectToAction(nameof(Index));
        }
        catch (ApiException ex) { ModelState.AddModelError("", ex.Message); return View("Form", dto); }
    }

    public async Task<IActionResult> Edit(int id)
    {
        ViewBag.PageTitle = "Editar perfil";
        Crumbs("Editar");
        try
        {
            var ed = await _api.GetAsync<TipoUsuarioDto>($"api/tiposusuario/{id}");
            if (ed == null) return NotFound();
            ViewBag.Id = id;
            return View("Form", new TipoUsuarioCreateDto { Nombre = ed.Nombre, Descripcion = ed.Descripcion });
        }
        catch (ApiException ex) { TempData["Error"] = ex.Message; return RedirectToAction(nameof(Index)); }
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TipoUsuarioCreateDto dto)
    {
        ViewBag.PageTitle = "Editar perfil";
        Crumbs("Editar");
        ViewBag.Id = id;
        if (!ModelState.IsValid) return View("Form", dto);
        try
        {
            await _api.PutAsync<TipoUsuarioDto>($"api/tiposusuario/{id}", dto);
            TempData["Success"] = "Perfil actualizado.";
            return RedirectToAction(nameof(Index));
        }
        catch (ApiException ex) { ModelState.AddModelError("", ex.Message); return View("Form", dto); }
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try { await _api.DeleteAsync($"api/tiposusuario/{id}"); TempData["Success"] = "Perfil eliminado."; }
        catch (ApiException ex) { TempData["Error"] = ex.Message; }
        return RedirectToAction(nameof(Index));
    }
}
