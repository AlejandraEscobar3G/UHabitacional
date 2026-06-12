using System.Net;
using System.Net.Http.Json;
using UHabitacional.MOBILE.Models;

namespace UHabitacional.MOBILE.Services;

/**
 * Servicio de autenticacion
 */
public class AuthService
{
    private const string BaseUrl = "http://localhost:5000";

    private static readonly HttpClient _http = new()
    {
        BaseAddress = new Uri(BaseUrl),
        Timeout = TimeSpan.FromSeconds(30)
    };

    public async Task<LoginResponse> LoginAsync(string email, string password)
    {
        var datos = new LoginRequest { Email = email, Password = password };

        HttpResponseMessage respuesta;
        try
        {
            respuesta = await _http.PostAsJsonAsync("/api/auth/login", datos);
        }
        catch (Exception)
        {
            throw new Exception("No se pudo conectar con el servidor. Verifica que la API esté en ejecución.");
        }

        if (respuesta.StatusCode == HttpStatusCode.Forbidden ||
            respuesta.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new Exception("Correo o contraseña incorrectos.");
        }

        if (!respuesta.IsSuccessStatusCode)
        {
            throw new Exception("Ocurrió un error al iniciar sesión. Inténtalo de nuevo.");
        }

        var resultado = await respuesta.Content.ReadFromJsonAsync<LoginResponse>();
        if (resultado is null)
        {
            throw new Exception("La respuesta del servidor no es válida.");
        }

        return resultado;
    }
}
