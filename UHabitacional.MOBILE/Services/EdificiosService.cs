using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using UHabitacional.MOBILE.Models;

namespace UHabitacional.MOBILE.Services;

public class EdificiosService
{
    private const string BaseUrl = "http://localhost:5000";

    private static readonly HttpClient _http = new()
    {
        BaseAddress = new Uri(BaseUrl),
        Timeout = TimeSpan.FromSeconds(30)
    };

    public async Task<List<Edificio>> ObtenerEdificiosAsync()
    {
        var peticion = new HttpRequestMessage(HttpMethod.Get, "/api/Edificios");

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
            throw new Exception("No se pudieron obtener los edificios.");
        }

        var edificios = await respuesta.Content.ReadFromJsonAsync<List<Edificio>>();
        return edificios ?? new List<Edificio>();
    }

    public async Task<Edificio> ObtenerEdificioPorIdAsync(int id)
    {
        var peticion = new HttpRequestMessage(HttpMethod.Get, $"/api/Edificios/{id}");
        AplicarToken(peticion);

        HttpResponseMessage respuesta = await EnviarAsync(peticion);
        if (!respuesta.IsSuccessStatusCode)
        {
            throw new Exception("No se pudo obtener el edificio.");
        }

        var edificio = await respuesta.Content.ReadFromJsonAsync<Edificio>();
        return edificio ?? throw new Exception("No se encontró el edificio.");
    }

    public async Task<Edificio> CrearEdificioAsync(EdificioRequest request)
    {
        var peticion = new HttpRequestMessage(HttpMethod.Post, "/api/Edificios")
        {
            Content = JsonContent.Create(request)
        };
        AplicarToken(peticion);

        HttpResponseMessage respuesta = await EnviarAsync(peticion);
        if (!respuesta.IsSuccessStatusCode)
        {
            throw new Exception("No se pudo crear el edificio. Verifica los datos e intenta de nuevo.");
        }

        var edificio = await respuesta.Content.ReadFromJsonAsync<Edificio>();
        return edificio ?? throw new Exception("El servidor no devolvió el edificio creado.");
    }

    public async Task<Edificio> ActualizarEdificioAsync(int id, EdificioRequest request)
    {
        var peticion = new HttpRequestMessage(HttpMethod.Put, $"/api/Edificios/{id}")
        {
            Content = JsonContent.Create(request)
        };
        AplicarToken(peticion);

        HttpResponseMessage respuesta = await EnviarAsync(peticion);
        if (!respuesta.IsSuccessStatusCode)
        {
            throw new Exception("No se pudo actualizar el edificio. Verifica los datos e intenta de nuevo.");
        }

        var edificio = await respuesta.Content.ReadFromJsonAsync<Edificio>();
        return edificio ?? throw new Exception("El servidor no devolvió el edificio actualizado.");
    }

    private static void AplicarToken(HttpRequestMessage peticion)
    {
        if (!string.IsNullOrWhiteSpace(SessionService.Token))
        {
            peticion.Headers.Authorization = new AuthenticationHeaderValue("Bearer", SessionService.Token);
        }
    }

    private static async Task<HttpResponseMessage> EnviarAsync(HttpRequestMessage peticion)
    {
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

        return respuesta;
    }
}
