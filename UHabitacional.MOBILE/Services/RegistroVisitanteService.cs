using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using UHabitacional.MOBILE.Models;

namespace UHabitacional.MOBILE.Services;

public class RegistroVisitanteService
{
    private const string BaseUrl = "http://localhost:5000";

    private static readonly HttpClient _http = new()
    {
        BaseAddress = new Uri(BaseUrl),
        Timeout = TimeSpan.FromSeconds(30)
    };

    public async Task<Visitante?> BuscarPorCodigoAsync(string codigo)
    {
        var peticion = new HttpRequestMessage(HttpMethod.Get, "/api/bitacora-visitante");
        AplicarToken(peticion);

        HttpResponseMessage respuesta = await EnviarAsync(peticion);

        if (!respuesta.IsSuccessStatusCode)
        {
            throw new Exception("No se pudo consultar la bitácora de visitantes.");
        }

        var todos = await respuesta.Content.ReadFromJsonAsync<List<Visitante>>();
        return todos?.FirstOrDefault(v =>
            string.Equals(v.CodigoVisita, codigo.Trim(), StringComparison.OrdinalIgnoreCase));
    }

    public async Task<Visitante> RegistrarEntradaAsync(int id)
    {
        return await RegistrarAsync(id, esLlegada: true);
    }

    public async Task<Visitante> RegistrarSalidaAsync(int id)
    {
        return await RegistrarAsync(id, esLlegada: false);
    }

    private async Task<Visitante> RegistrarAsync(int id, bool esLlegada)
    {
        var payload = new { EsLlegada = esLlegada };

        var peticion = new HttpRequestMessage(HttpMethod.Patch, $"/api/bitacora-visitante/{id}/registro")
        {
            Content = JsonContent.Create(payload)
        };
        AplicarToken(peticion);

        HttpResponseMessage respuesta = await EnviarAsync(peticion);

        if (!respuesta.IsSuccessStatusCode)
        {
            string detalle = await ExtraerDetalleErrorAsync(respuesta);
            throw new Exception(detalle ?? "No se pudo registrar el acceso.");
        }

        var visitante = await respuesta.Content.ReadFromJsonAsync<Visitante>();
        return visitante ?? throw new Exception("El servidor no devolvió el registro actualizado.");
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
