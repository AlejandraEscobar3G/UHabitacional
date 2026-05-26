using Microsoft.AspNetCore.Mvc;
using UHabitacional.MVC.Models;
using UHabitacional.MVC.Services;
using UHabitacional.MVC.ViewModels;

namespace UHabitacional.MVC.Controllers;

[AuthorizeRole("Administrador")]
public class DepartamentosController : Controller
{
    private readonly IApiClient _api;
    public DepartamentosController(IApiClient api) => _api = api;

    private void Crumbs(string? last = null)
    {
        var list = new List<BreadcrumbItem>
        {
            new() { Label = "Home", Url = Url.Action("Index","Home") },
            new() { Label = "Departamentos", Url = Url.Action("Index","Departamentos") }
        };
        if (last != null) list.Add(new BreadcrumbItem { Label = last });
        ViewBag.Breadcrumbs = list;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.PageTitle = "Departamentos";
        Crumbs();
        try { return View(await _api.GetListAsync<DepartamentoDto>("api/departamentos")); }
        catch (ApiException ex) { TempData["Error"] = ex.Message; return View(new List<DepartamentoDto>()); }
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.PageTitle = "Nuevo departamento";
        Crumbs("Nuevo");
        await CargarEdificios();
        return View("Form", new DepartamentoCreateDto());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DepartamentoCreateDto dto)
    {
        ViewBag.PageTitle = "Nuevo departamento";
        Crumbs("Nuevo");
        if (!ModelState.IsValid) { await CargarEdificios(); return View("Form", dto); }
        try
        {
            await _api.PostAsync<DepartamentoDto>("api/departamentos", dto);
            TempData["Success"] = "Departamento creado.";
            return RedirectToAction(nameof(Index));
        }
        catch (ApiException ex) { ModelState.AddModelError("", ex.Message); await CargarEdificios(); return View("Form", dto); }
    }

    public async Task<IActionResult> Edit(int id)
    {
        ViewBag.PageTitle = "Editar departamento";
        Crumbs("Editar");
        try
        {
            var dep = await _api.GetAsync<DepartamentoDto>($"api/departamentos/{id}");
            if (dep == null) return NotFound();
            ViewBag.Id = id;
            await CargarEdificios();
            return View("Form", new DepartamentoCreateDto
            {
                IdEdificio = dep.IdEdificio,
                NumeroDepartamento = dep.NumeroDepartamento,
                Piso = dep.Piso
            });
        }
        catch (ApiException ex) { TempData["Error"] = ex.Message; return RedirectToAction(nameof(Index)); }
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, DepartamentoCreateDto dto)
    {
        ViewBag.PageTitle = "Editar departamento";
        Crumbs("Editar");
        ViewBag.Id = id;
        if (!ModelState.IsValid) { await CargarEdificios(); return View("Form", dto); }
        try
        {
            await _api.PutAsync<DepartamentoDto>($"api/departamentos/{id}", dto);
            TempData["Success"] = "Departamento actualizado.";
            return RedirectToAction(nameof(Index));
        }
        catch (ApiException ex) { ModelState.AddModelError("", ex.Message); await CargarEdificios(); return View("Form", dto); }
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try { await _api.DeleteAsync($"api/departamentos/{id}"); TempData["Success"] = "Departamento eliminado."; }
        catch (ApiException ex) { TempData["Error"] = ex.Message; }
        return RedirectToAction(nameof(Index));
    }

    private async Task CargarEdificios()
    {
        try { ViewBag.Edificios = await _api.GetListAsync<EdificioDto>("api/edificios"); }
        catch { ViewBag.Edificios = new List<EdificioDto>(); }
    }
}
