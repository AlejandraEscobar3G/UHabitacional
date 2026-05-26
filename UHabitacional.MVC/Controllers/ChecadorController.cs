using Microsoft.AspNetCore.Mvc;
using UHabitacional.MVC.Models;
using UHabitacional.MVC.Services;
using UHabitacional.MVC.ViewModels;

namespace UHabitacional.MVC.Controllers;

[AuthorizeRole("Vigilante,Administrador")]
public class ChecadorController : Controller
{
    private readonly IApiClient _api;
    private readonly ICurrentUser _current;
    public ChecadorController(IApiClient api, ICurrentUser current) { _api = api; _current = current; }

    private void Crumbs()
    {
        ViewBag.Breadcrumbs = new List<BreadcrumbItem>
        {
            new() { Label = "Home", Url = Url.Action("Index","Home") },
            new() { Label = "Checador" }
        };
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.PageTitle = "Checador";
        Crumbs();

        var vm = new ChecadorViewModel();
        try
        {
            var todas = await _api.GetListAsync<BitacoraVigilanteDto>("api/bitacora-vigilante");
            vm.Historial = todas
                .Where(b => b.IdUsuario == _current.IdUsuario)
                .OrderByDescending(b => b.FechaHoraEntrada)
                .Take(7)
                .ToList();

            // Última entrada sin salida = turno abierto
            vm.TurnoAbierto = todas
                .Where(b => b.IdUsuario == _current.IdUsuario && b.FechaHoraSalida == null)
                .OrderByDescending(b => b.FechaHoraEntrada)
                .FirstOrDefault();
        }
        catch (ApiException ex) { TempData["Error"] = ex.Message; }

        return View(vm);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Entrada(string? observaciones)
    {
        try
        {
            await _api.PostAsync<BitacoraVigilanteDto>("api/bitacora-vigilante",
                new BitacoraVigilanteCreateDto { FechaHoraEntrada = DateTime.Now, Observaciones = observaciones });
            TempData["Success"] = "Entrada registrada.";
        }
        catch (ApiException ex) { TempData["Error"] = ex.Message; }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Salida(int id, string? observaciones)
    {
        try
        {
            await _api.PutAsync<BitacoraVigilanteDto>($"api/bitacora-vigilante/{id}",
                new BitacoraVigilanteUpdateDto { FechaHoraSalida = DateTime.Now, Observaciones = observaciones });
            TempData["Success"] = "Salida registrada.";
        }
        catch (ApiException ex) { TempData["Error"] = ex.Message; }
        return RedirectToAction(nameof(Index));
    }
}
