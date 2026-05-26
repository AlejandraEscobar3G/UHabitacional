using Microsoft.AspNetCore.Mvc;
using UHabitacional.MVC.Models;
using UHabitacional.MVC.Services;
using UHabitacional.MVC.ViewModels;

namespace UHabitacional.MVC.Controllers;

[AuthorizeRole("Administrador")]
public class EdificiosController : Controller
{
    private readonly IApiClient _api;
    public EdificiosController(IApiClient api) => _api = api;

    private void Crumbs(string? last = null, string? lastUrl = null)
    {
        var list = new List<BreadcrumbItem>
        {
            new() { Label = "Home", Url = Url.Action("Index","Home") },
            new() { Label = "Edificios", Url = Url.Action("Index","Edificios") }
        };
        if (last != null) list.Add(new BreadcrumbItem { Label = last, Url = lastUrl });
        ViewBag.Breadcrumbs = list;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.PageTitle = "Edificios";
        Crumbs();
        try
        {
            var data = await _api.GetListAsync<EdificioDto>("api/edificios");
            return View(data);
        }
        catch (ApiException ex)
        {
            TempData["Error"] = ex.Message;
            return View(new List<EdificioDto>());
        }
    }

    public IActionResult Create()
    {
        ViewBag.PageTitle = "Nuevo edificio";
        Crumbs("Nuevo");
        return View("Form", new EdificioCreateDto());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EdificioCreateDto dto)
    {
        ViewBag.PageTitle = "Nuevo edificio";
        Crumbs("Nuevo");
        if (!ModelState.IsValid) return View("Form", dto);

        try
        {
            await _api.PostAsync<EdificioDto>("api/edificios", dto);
            TempData["Success"] = "Edificio creado exitosamente.";
            return RedirectToAction(nameof(Index));
        }
        catch (ApiException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View("Form", dto);
        }
    }

    public async Task<IActionResult> Edit(int id)
    {
        ViewBag.PageTitle = "Editar edificio";
        Crumbs("Editar");
        try
        {
            var ed = await _api.GetAsync<EdificioDto>($"api/edificios/{id}");
            if (ed == null) return NotFound();
            ViewBag.Id = id;
            return View("Form", new EdificioCreateDto
            {
                Nombre = ed.Nombre,
                Descripcion = ed.Descripcion,
                NumeroPisos = ed.NumeroPisos,
                TotalDeptos = ed.TotalDeptos
            });
        }
        catch (ApiException ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EdificioCreateDto dto)
    {
        ViewBag.PageTitle = "Editar edificio";
        Crumbs("Editar");
        ViewBag.Id = id;
        if (!ModelState.IsValid) return View("Form", dto);
        try
        {
            await _api.PutAsync<EdificioDto>($"api/edificios/{id}", dto);
            TempData["Success"] = "Edificio actualizado.";
            return RedirectToAction(nameof(Index));
        }
        catch (ApiException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View("Form", dto);
        }
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _api.DeleteAsync($"api/edificios/{id}");
            TempData["Success"] = "Edificio eliminado.";
        }
        catch (ApiException ex) { TempData["Error"] = ex.Message; }
        return RedirectToAction(nameof(Index));
    }
}
