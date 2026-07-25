using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using UHabitacional.MOBILE.Models;

namespace UHabitacional.MOBILE.Services;

public class ChecadorService
{
    private const string BaseUrl = "http://localhost:5000";

    private static readonly HttpClient _http = new()
    {
        BaseAddress = new Uri(BaseUrl),
        Timeout = TimeSpan.FromSeconds(30)
    };

    // Devuelve los últimos 7 turnos del vigilante autenticado
    public async Task<List<TurnoVigilante>> ObtenerTurnosPropiosAsync()
    {
        var peticion = new HttpRequestMessage(HttpMethod.Get, "/api/bitacora-vigilante");
        AplicarToken(peticion);

        HttpResponseMessage respuesta = await EnviarAsync(peticion);

        if (!respuesta.IsSuccessStatusCode)
        {
            throw new Exception("No se pudo obtener el historial de turnos.");
        }

        var todos = await respuesta.Content.ReadFromJsonAsync<List<TurnoVigilante>>();

        return todos?
            .Where(t => t.IdUsuario == SessionService.IdUsuario)
            .OrderByDescending(t => t.FechaHoraEntrada)
            .Take(7)
            .ToList() ?? [];
    }

    public async Task<TurnoVigilante> RegistrarEntradaAsync()
    {
        var request = new TurnoVigilanteCreateRequest
        {
            FechaHoraEntrada = DateTime.Now
        };

        var peticion = new HttpRequestMessage(HttpMethod.Post, "/api/bitacora-vigilante")
        {
            Content = JsonContent.Create(request)
        };
        AplicarToken(peticion);

        HttpResponseMessage respuesta = await EnviarAsync(peticion);

        if (!respuesta.IsSuccessStatusCode)
        {
            string detalle = await ExtraerDetalleErrorAsync(respuesta);
            throw new Exception(detalle ?? "No se pudo registrar la entrada.");
        }

        var turno = await respuesta.Content.ReadFromJsonAsync<TurnoVigilante>();
        return turno ?? throw new Exception("El servidor no devolvió el turno registrado.");
    }

    public async Task<TurnoVigilante> RegistrarSalidaAsync(int idTurno)
    {
        var request = new TurnoVigilanteUpdateRequest
        {
            FechaHoraSalida = DateTime.Now
        };

        var peticion = new HttpRequestMessage(HttpMethod.Put, $"/api/bitacora-vigilante/{idTurno}")
        {
            Content = JsonContent.Create(request)
        };
        AplicarToken(peticion);

        HttpResponseMessage respuesta = await EnviarAsync(peticion);

        if (!respuesta.IsSuccessStatusCode)
        {
            string detalle = await ExtraerDetalleErrorAsync(respuesta);
            throw new Exception(detalle ?? "No se pudo registrar la salida.");
        }

        var turno = await respuesta.Content.ReadFromJsonAsync<TurnoVigilante>();
        return turno ?? throw new Exception("El servidor no devolvió el turno actualizado.");
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
