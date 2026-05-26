using Microsoft.AspNetCore.Mvc;
using UHabitacional.MVC.Models;
using UHabitacional.MVC.Services;
using UHabitacional.MVC.ViewModels;

namespace UHabitacional.MVC.Controllers;

[AuthorizeRole("Vigilante,Administrador")]
public class RegistroVisitanteController : Controller
{
    private readonly IApiClient _api;
    public RegistroVisitanteController(IApiClient api) => _api = api;

    private void Crumbs()
    {
        ViewBag.Breadcrumbs = new List<BreadcrumbItem>
        {
            new() { Label = "Home", Url = Url.Action("Index","Home") },
            new() { Label = "Registro de visitante" }
        };
    }

    [HttpGet]
    public IActionResult Index()
    {
        ViewBag.PageTitle = "Registro de visitante";
        Crumbs();
        return View(new RegistroVisitanteViewModel());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Validar(string codigo)
    {
        ViewBag.PageTitle = "Registro de visitante";
        Crumbs();

        var vm = new RegistroVisitanteViewModel { Codigo = (codigo ?? string.Empty).Trim().ToUpper() };

        if (string.IsNullOrWhiteSpace(vm.Codigo))
        {
            vm.Mensaje = "Ingresa un código de visita.";
            vm.EsError = true;
            return View("Index", vm);
        }

        try
        {
            var todos = await _api.GetListAsync<BitacoraVisitanteDto>("api/bitacora-visitante");
            var v = todos.FirstOrDefault(x => string.Equals(x.CodigoVisita, vm.Codigo, StringComparison.OrdinalIgnoreCase));
            if (v == null)
            {
                vm.Mensaje = "El código no existe o no está activo. Verifica con el visitante.";
                vm.EsError = true;
            }
            else
            {
                vm.Visitante = v;
            }
        }
        catch (ApiException ex)
        {
            vm.Mensaje = ex.Message;
            vm.EsError = true;
        }
        return View("Index", vm);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> RegistrarEntrada(int id, string codigo)
    {
        try
        {
            await _api.PatchAsync<BitacoraVisitanteDto>($"api/bitacora-visitante/{id}/registro",
                new BitacoraVisitanteRegistroDto { EsLlegada = true });
            TempData["Success"] = "Entrada del visitante registrada.";
        }
        catch (ApiException ex) { TempData["Error"] = ex.Message; }
        return await Validar(codigo);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> RegistrarSalida(int id, string codigo)
    {
        try
        {
            await _api.PatchAsync<BitacoraVisitanteDto>($"api/bitacora-visitante/{id}/registro",
                new BitacoraVisitanteRegistroDto { EsLlegada = false });
            TempData["Success"] = "Salida del visitante registrada.";
        }
        catch (ApiException ex) { TempData["Error"] = ex.Message; }
        return await Validar(codigo);
    }
}
