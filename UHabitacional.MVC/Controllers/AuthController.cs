using Microsoft.AspNetCore.Mvc;
using UHabitacional.MVC.Models;
using UHabitacional.MVC.Services;

namespace UHabitacional.MVC.Controllers;

public class AuthController : Controller
{
    private readonly IApiClient _api;
    private readonly ICurrentUser _current;

    public AuthController(IApiClient api, ICurrentUser current)
    {
        _api = api;
        _current = current;
    }

    [HttpGet]
    public IActionResult Login()
    {
        ViewBag.HideSidebar = true;
        if (_current.IsAuthenticated) return RedirectToAction("Index", "Home");
        return View(new LoginDto());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        ViewBag.HideSidebar = true;
        if (!ModelState.IsValid) return View(dto);

        try
        {
            var response = await _api.PostAsync<LoginResponseDto>("api/auth/login", dto);
            if (response == null)
            {
                ModelState.AddModelError("", "La API no devolvió respuesta.");
                return View(dto);
            }

            _current.SetSession(
                response.IdUsuario,
                response.Email,
                response.Nombre,
                response.TipoUsuario,
                response.Token,
                response.Expira);

            return RedirectToAction("Index", "Home");
        }
        catch (ApiException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(dto);
        }
        catch (HttpRequestException)
        {
            ModelState.AddModelError("", "No se pudo conectar con la API. Verifica que esté en ejecución.");
            return View(dto);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        try { await _api.PostAsync<object>("api/sesion/logout", new { }); }
        catch { /* ignoramos errores de logout remoto */ }

        _current.Clear();
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        ViewBag.HideSidebar = true;
        return View();
    }
}
