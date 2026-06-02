using Microsoft.AspNetCore.Mvc;
using UHabitacional.MVC.Models;
using UHabitacional.MVC.Services;
using UHabitacional.MVC.ViewModels;

namespace UHabitacional.MVC.Controllers;

[AuthorizeRole("Administrador")]
public class InquilinosController : Controller
{
    private const string RolInquilino = "Inquilino";
    private readonly IApiClient _api;
    public InquilinosController(IApiClient api) => _api = api;

    private void Crumbs(string? last = null)
    {
        var list = new List<BreadcrumbItem>
        {
            new() { Label = "Home", Url = Url.Action("Index","Home") },
            new() { Label = "Inquilinos", Url = Url.Action("Index","Inquilinos") }
        };
        if (last != null) list.Add(new BreadcrumbItem { Label = last });
        ViewBag.Breadcrumbs = list;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.PageTitle = "Inquilinos";
        Crumbs();
        try { return View(await _api.GetListAsync<InquilinoDto>("api/inquilinos")); }
        catch (ApiException ex) { TempData["Error"] = ex.Message; return View(new List<InquilinoDto>()); }
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.PageTitle = "Nuevo inquilino";
        Crumbs("Nuevo");
        await CargarCatalogos();
        return View("Form", new InquilinoFormViewModel { FechaInicio = DateTime.Today });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(InquilinoFormViewModel vm)
    {
        ViewBag.PageTitle = "Nuevo inquilino";
        Crumbs("Nuevo");
        if (!ModelState.IsValid) { await CargarCatalogos(); return View("Form", vm); }

        // Resolvemos el rol Inquilino antes de continuar
        var idRol = await ObtenerIdTipoUsuarioAsync(RolInquilino);
        if (!idRol.HasValue)
        {
            ModelState.AddModelError("", "No se encontró el perfil 'Inquilino' en el catálogo de tipos de usuario.");
            await CargarCatalogos();
            return View("Form", vm);
        }

        try
        {
            // 1) Crear el Usuario con rol Inquilino
            var usuarioDto = new UsuarioCreateDto
            {
                IdTipoUsuario = idRol.Value,
                IdIdentificacion = vm.IdIdentificacion,
                NumeroIdentificacion = vm.NumeroIdentificacion,
                Nombre = vm.Nombre,
                Apellidos = vm.Apellidos,
                Email = vm.Email,
                Password = vm.Password ?? "Cambiar123!",
                Telefono = vm.Telefono
            };
            var usuario = await _api.PostAsync<UsuarioDto>("api/usuarios", usuarioDto);
            if (usuario == null) throw new ApiException(System.Net.HttpStatusCode.InternalServerError, "No se pudo crear el usuario base.");

            // 2) Crear el Inquilino
            var inqDto = new InquilinoCreateDto
            {
                IdUsuario = usuario.IdUsuario,
                IdDepartamento = vm.IdDepartamento,
                FechaInicio = vm.FechaInicio
            };
            await _api.PostAsync<InquilinoDto>("api/inquilinos", inqDto);

            TempData["Success"] = "Inquilino creado exitosamente.";
            return RedirectToAction(nameof(Index));
        }
        catch (ApiException ex)
        {
            ModelState.AddModelError("", ex.Message);
            await CargarCatalogos();
            return View("Form", vm);
        }
    }

    public async Task<IActionResult> Edit(int id)
    {
        ViewBag.PageTitle = "Editar inquilino";
        Crumbs("Editar");
        try
        {
            var inq = await _api.GetAsync<InquilinoDto>($"api/inquilinos/{id}");
            if (inq == null) return NotFound();
            var usr = await _api.GetAsync<UsuarioDto>($"api/usuarios/{inq.IdUsuario}");
            ViewBag.Id = id;
            await CargarCatalogos();

            return View("Form", new InquilinoFormViewModel
            {
                IdInquilino = inq.IdInquilino,
                IdUsuario = inq.IdUsuario,
                IdTipoUsuario = usr?.IdTipoUsuario ?? 0,
                IdIdentificacion = usr?.IdIdentificacion ?? 0,
                NumeroIdentificacion = usr?.NumeroIdentificacion ?? string.Empty,
                Nombre = usr?.Nombre ?? string.Empty,
                Apellidos = usr?.Apellidos ?? string.Empty,
                Email = usr?.Email ?? string.Empty,
                Telefono = usr?.Telefono,
                IdDepartamento = inq.IdDepartamento,
                FechaInicio = inq.FechaInicio,
                FechaFin = inq.FechaFin
            });
        }
        catch (ApiException ex) { TempData["Error"] = ex.Message; return RedirectToAction(nameof(Index)); }
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, InquilinoFormViewModel vm)
    {
        ViewBag.PageTitle = "Editar inquilino";
        Crumbs("Editar");
        ViewBag.Id = id;
        ModelState.Remove(nameof(InquilinoFormViewModel.Password));
        if (!ModelState.IsValid) { await CargarCatalogos(); return View("Form", vm); }

        try
        {
            // 1) Actualizar Usuario base
            var upd = new UsuarioUpdateDto
            {
                IdTipoUsuario = vm.IdTipoUsuario,
                IdIdentificacion = vm.IdIdentificacion,
                NumeroIdentificacion = vm.NumeroIdentificacion,
                Nombre = vm.Nombre,
                Apellidos = vm.Apellidos,
                Email = vm.Email,
                Password = string.IsNullOrWhiteSpace(vm.Password) ? null : vm.Password,
                Telefono = vm.Telefono
            };
            await _api.PutAsync<UsuarioDto>($"api/usuarios/{vm.IdUsuario}", upd);

            // 2) Actualizar Inquilino (departamento / fecha inicio)
            await _api.PutAsync<InquilinoDto>($"api/inquilinos/{id}",
                new InquilinoUpdateDto { IdDepartamento = vm.IdDepartamento, FechaInicio = vm.FechaInicio });

            // 3) Actualizar FechaFin si llegó
            if (vm.FechaFin.HasValue)
            {
                await _api.SendRawAsync(new HttpMethod("PATCH"), $"api/inquilinos/{id}/fecha-fin",
                    new InquilinoFechaFinDto { FechaFin = vm.FechaFin });
            }

            TempData["Success"] = "Inquilino actualizado.";
            return RedirectToAction(nameof(Index));
        }
        catch (ApiException ex)
        {
            ModelState.AddModelError("", ex.Message);
            await CargarCatalogos();
            return View("Form", vm);
        }
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try { await _api.DeleteAsync($"api/inquilinos/{id}"); TempData["Success"] = "Inquilino eliminado."; }
        catch (ApiException ex) { TempData["Error"] = ex.Message; }
        return RedirectToAction(nameof(Index));
    }

    private async Task CargarCatalogos()
    {
        try { ViewBag.Identificaciones = await _api.GetListAsync<IdentificacionDto>("api/identificaciones"); }
        catch { ViewBag.Identificaciones = new List<IdentificacionDto>(); }
        try { ViewBag.Departamentos = await _api.GetListAsync<DepartamentoDto>("api/departamentos"); }
        catch { ViewBag.Departamentos = new List<DepartamentoDto>(); }
    }

    private async Task<int?> ObtenerIdTipoUsuarioAsync(string nombre)
    {
        try
        {
            var tipos = await _api.GetListAsync<TipoUsuarioDto>("api/tiposusuario");
            return tipos.FirstOrDefault(t => string.Equals(t.Nombre, nombre, StringComparison.OrdinalIgnoreCase))?.IdTipoUsuario;
        }
        catch { return null; }
    }
}
