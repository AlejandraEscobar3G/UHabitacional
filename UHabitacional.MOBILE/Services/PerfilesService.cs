using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using UHabitacional.MOBILE.Models;

namespace UHabitacional.MOBILE.Services;

public class PerfilesService
{
    private const string BaseUrl = "http://localhost:5000";

    private static readonly HttpClient _http = new()
    {
        BaseAddress = new Uri(BaseUrl),
        Timeout = TimeSpan.FromSeconds(30)
    };

    public async Task<List<Perfil>> ObtenerPerfilesAsync()
    {
        var peticion = new HttpRequestMessage(HttpMethod.Get, "/api/TiposUsuario");

        if (!string.IsNullOrWhiteSpace(SessionService.Token))
        {
            peticion.Headers.Authorization = new AuthenticationHeaderValue("Bearer", SessionService.Token);
        }

        HttpResponseMessage respuesta;
        try
        {
            respuesta = await _http.SendAsync(peticion);
        }
        catch (Exception)
        {
            throw new Exception("No se pudo conectar con el servidor.");
        }

        if (respuesta.StatusCode == HttpStatusCode.Unauthorized ||
            respuesta.StatusCode == HttpStatusCode.Forbidden)
        {
            throw new Exception("Tu sesión no es válida o no tienes permisos.");
        }

        if (!respuesta.IsSuccessStatusCode)
        {
            throw new Exception("No se pudieron obtener los perfiles de usuario.");
        }

        var perfiles = await respuesta.Content.ReadFromJsonAsync<List<Perfil>>();
        return perfiles ?? new List<Perfil>();
    }
}
