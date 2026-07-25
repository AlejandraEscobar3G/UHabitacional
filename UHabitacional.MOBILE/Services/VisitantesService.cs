using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using UHabitacional.MOBILE.Models;

namespace UHabitacional.MOBILE.Services;

public class VisitantesService
{
    private const string BaseUrl = "http://localhost:5000";

    private static readonly HttpClient _http = new()
    {
        BaseAddress = new Uri(BaseUrl),
        Timeout = TimeSpan.FromSeconds(30)
    };

    // Devuelve solo los visitantes del inquilino autenticado
    public async Task<List<Visitante>> ObtenerVisitantesPropiosAsync()
    {
        // Obtener todos los visitantes
        var peticionVisitantes = new HttpRequestMessage(HttpMethod.Get, "/api/bitacora-visitante");
        AplicarToken(peticionVisitantes);
        HttpResponseMessage respVisitantes = await EnviarAsync(peticionVisitantes);

        if (!respVisitantes.IsSuccessStatusCode)
        {
            throw new Exception("No se pudieron obtener los visitantes.");
        }

        var todos = await respVisitantes.Content.ReadFromJsonAsync<List<Visitante>>();
        if (todos is null || todos.Count == 0)
        {
            return [];
        }

        // Encontrar el IdInquilino del usuario autenticado
        int idInquilino = await ObtenerIdInquilinoAsync();
        if (idInquilino == 0)
        {
            return [];
        }

        return todos.Where(v => v.IdInquilino == idInquilino)
                    .OrderByDescending(v => v.FechaCreacion)
                    .ToList();
    }

    public async Task<Visitante> ObtenerVisitantePorIdAsync(int id)
    {
        var peticion = new HttpRequestMessage(HttpMethod.Get, $"/api/bitacora-visitante/{id}");
        AplicarToken(peticion);

        HttpResponseMessage respuesta = await EnviarAsync(peticion);

        if (!respuesta.IsSuccessStatusCode)
        {
            throw new Exception("No se pudo obtener el registro de visita.");
        }

        var visitante = await respuesta.Content.ReadFromJsonAsync<Visitante>();
        return visitante ?? throw new Exception("No se encontró el registro de visita.");
    }

    public async Task<Visitante> CrearVisitanteAsync(VisitanteCreateRequest request)
    {
        var peticion = new HttpRequestMessage(HttpMethod.Post, "/api/bitacora-visitante")
        {
            Content = JsonContent.Create(request)
        };
        AplicarToken(peticion);

        HttpResponseMessage respuesta = await EnviarAsync(peticion);

        if (!respuesta.IsSuccessStatusCode)
        {
            string detalle = await ExtraerDetalleErrorAsync(respuesta);
            throw new Exception(detalle ?? "No se pudo registrar al visitante.");
        }

        var visitante = await respuesta.Content.ReadFromJsonAsync<Visitante>();
        return visitante ?? throw new Exception("El servidor no devolvió el registro creado.");
    }

    public async Task<Visitante> ActualizarVisitanteAsync(int id, VisitanteUpdateRequest request)
    {
        var peticion = new HttpRequestMessage(HttpMethod.Put, $"/api/bitacora-visitante/{id}")
        {
            Content = JsonContent.Create(request)
        };
        AplicarToken(peticion);

        HttpResponseMessage respuesta = await EnviarAsync(peticion);

        if (!respuesta.IsSuccessStatusCode)
        {
            string detalle = await ExtraerDetalleErrorAsync(respuesta);
            throw new Exception(detalle ?? "No se pudo actualizar el registro de visita.");
        }

        var visitante = await respuesta.Content.ReadFromJsonAsync<Visitante>();
        return visitante ?? throw new Exception("El servidor no devolvió el registro actualizado.");
    }

    public async Task EliminarVisitanteAsync(int id)
    {
        var peticion = new HttpRequestMessage(HttpMethod.Delete, $"/api/bitacora-visitante/{id}");
        AplicarToken(peticion);

        HttpResponseMessage respuesta = await EnviarAsync(peticion);

        if (!respuesta.IsSuccessStatusCode)
        {
            string detalle = await ExtraerDetalleErrorAsync(respuesta);
            throw new Exception(detalle ?? "No se pudo eliminar el registro de visita.");
        }
    }

    // Obtiene el IdInquilino correspondiente al usuario autenticado
    private static async Task<int> ObtenerIdInquilinoAsync()
    {
        var peticion = new HttpRequestMessage(HttpMethod.Get, "/api/Inquilinos");
        AplicarToken(peticion);

        HttpResponseMessage respuesta;
        try
        {
            respuesta = await _http.SendAsync(peticion);
        }
        catch
        {
            return 0;
        }

        if (!respuesta.IsSuccessStatusCode)
        {
            return 0;
        }

        var inquilinos = await respuesta.Content.ReadFromJsonAsync<List<Inquilino>>();
        Inquilino? mio = inquilinos?.FirstOrDefault(i => i.IdUsuario == SessionService.IdUsuario);
        return mio?.IdInquilino ?? 0;
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
        catch
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

    private static async Task<string> ExtraerDetalleErrorAsync(HttpResponseMessage respuesta)
    {
        try
        {
            string json = await respuesta.Content.ReadAsStringAsync();
            using JsonDocument doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("detail", out JsonElement detail))
            {
                return detail.GetString() ?? string.Empty;
            }
        }
        catch { }

        return string.Empty;
    }
}
