using Microsoft.AspNetCore.Mvc;
using UHabitacional.MVC.Models;
using UHabitacional.MVC.Services;
using UHabitacional.MVC.ViewModels;

namespace UHabitacional.MVC.Controllers;

/// <summary>
/// Los Vigilantes son Usuarios cuyo TipoUsuario es "Vigilante".
/// Este controlador filtra y simplifica la administración de ese subconjunto.
/// </summary>
[AuthorizeRole("Administrador")]
public class VigilantesController : Controller
{
    private const string RolVigilante = "Vigilante";

    private readonly IApiClient _api;
    public VigilantesController(IApiClient api) => _api = api;

    private void Crumbs(string? last = null)
    {
        var list = new List<BreadcrumbItem>
        {
            new() { Label = "Home", Url = Url.Action("Index","Home") },
            new() { Label = "Vigilantes", Url = Url.Action("Index","Vigilantes") }
        };
        if (last != null) list.Add(new BreadcrumbItem { Label = last });
        ViewBag.Breadcrumbs = list;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.PageTitle = "Vigilantes";
        Crumbs();
        try
        {
            var todos = await _api.GetListAsync<UsuarioDto>("api/usuarios");
            var vigilantes = todos
                .Where(u => string.Equals(u.TipoUsuario, RolVigilante, StringComparison.OrdinalIgnoreCase))
                .ToList();
            return View(vigilantes);
        }
        catch (ApiException ex)
        {
            TempData["Error"] = ex.Message;
            return View(new List<UsuarioDto>());
        }
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.PageTitle = "Nuevo vigilante";
        Crumbs("Nuevo");
        await CargarCatalogos();
        return View("Form", new UsuarioCreateDto());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UsuarioCreateDto dto)
    {
        ViewBag.PageTitle = "Nuevo vigilante";
        Crumbs("Nuevo");

        // El form no expone IdTipoUsuario (siempre es Vigilante).
        // Lo resolvemos del catálogo ANTES de validar y lo eliminamos del ModelState
        // para que no falle el Range(1, ...).
        var idRolVig = await ObtenerIdTipoUsuarioAsync(RolVigilante);
        if (idRolVig.HasValue)
        {
            dto.IdTipoUsuario = idRolVig.Value;
        }
        else
        {
            ModelState.AddModelError("", "No se encontró el perfil 'Vigilante' en el catálogo de tipos de usuario.");
            await CargarCatalogos();
            return View("Form", dto);
        }
        ModelState.Remove(nameof(UsuarioCreateDto.IdTipoUsuario));

        if (!ModelState.IsValid) { await CargarCatalogos(); return View("Form", dto); }

        try
        {
            await _api.PostAsync<UsuarioDto>("api/usuarios", dto);
            TempData["Success"] = "Vigilante creado exitosamente.";
            return RedirectToAction(nameof(Index));
        }
        catch (ApiException ex)
        {
            ModelState.AddModelError("", ex.Message);
            await CargarCatalogos();
            return View("Form", dto);
        }
    }

    public async Task<IActionResult> Edit(int id)
    {
        ViewBag.PageTitle = "Editar vigilante";
        Crumbs("Editar");
        try
        {
            var u = await _api.GetAsync<UsuarioDto>($"api/usuarios/{id}");
            if (u == null) return NotFound();
            ViewBag.Id = id;
            await CargarCatalogos();
            return View("Form", new UsuarioCreateDto
            {
                IdTipoUsuario = u.IdTipoUsuario,
                IdIdentificacion = u.IdIdentificacion,
                NumeroIdentificacion = u.NumeroIdentificacion,
                Nombre = u.Nombre,
                Apellidos = u.Apellidos,
                Email = u.Email,
                Telefono = u.Telefono
            });
        }
        catch (ApiException ex) { TempData["Error"] = ex.Message; return RedirectToAction(nameof(Index)); }
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UsuarioCreateDto dto)
    {
        ViewBag.PageTitle = "Editar vigilante";
        Crumbs("Editar");
        ViewBag.Id = id;
        // En edición la contraseña es opcional, evitamos validarla
        ModelState.Remove(nameof(UsuarioCreateDto.Password));

        // IdTipoUsuario no viene en el form: forzamos rol Vigilante y lo quitamos del ModelState
        var idRolVig = await ObtenerIdTipoUsuarioAsync(RolVigilante);
        if (idRolVig.HasValue) dto.IdTipoUsuario = idRolVig.Value;
        ModelState.Remove(nameof(UsuarioCreateDto.IdTipoUsuario));

        if (!ModelState.IsValid) { await CargarCatalogos(); return View("Form", dto); }

        var upd = new UsuarioUpdateDto
        {
            IdTipoUsuario = dto.IdTipoUsuario,
            IdIdentificacion = dto.IdIdentificacion,
            NumeroIdentificacion = dto.NumeroIdentificacion,
            Nombre = dto.Nombre,
            Apellidos = dto.Apellidos,
            Email = dto.Email,
            Password = string.IsNullOrWhiteSpace(dto.Password) ? null : dto.Password,
            Telefono = dto.Telefono
        };

        try
        {
            await _api.PutAsync<UsuarioDto>($"api/usuarios/{id}", upd);
            TempData["Success"] = "Vigilante actualizado.";
            return RedirectToAction(nameof(Index));
        }
        catch (ApiException ex)
        {
            ModelState.AddModelError("", ex.Message);
            await CargarCatalogos();
            return View("Form", dto);
        }
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try { await _api.DeleteAsync($"api/usuarios/{id}"); TempData["Success"] = "Vigilante eliminado."; }
        catch (ApiException ex) { TempData["Error"] = ex.Message; }
        return RedirectToAction(nameof(Index));
    }

    private async Task CargarCatalogos()
    {
        try { ViewBag.Identificaciones = await _api.GetListAsync<IdentificacionDto>("api/identificaciones"); }
        catch { ViewBag.Identificaciones = new List<IdentificacionDto>(); }
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
