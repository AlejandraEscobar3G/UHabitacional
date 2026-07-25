using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using UHabitacional.MOBILE.Models;

namespace UHabitacional.MOBILE.Services;

public class InquilinosService
{
    private const string BaseUrl = "http://localhost:5000";
    private const int TipoUsuarioInquilino = 3;

    private static readonly HttpClient _http = new()
    {
        BaseAddress = new Uri(BaseUrl),
        Timeout = TimeSpan.FromSeconds(30)
    };

    public async Task<List<Inquilino>> ObtenerInquilinosAsync()
    {
        var peticion = new HttpRequestMessage(HttpMethod.Get, "/api/Inquilinos");
        AplicarToken(peticion);

        HttpResponseMessage respuesta = await EnviarAsync(peticion);

        if (!respuesta.IsSuccessStatusCode)
        {
            throw new Exception("No se pudieron obtener los inquilinos.");
        }

        var inquilinos = await respuesta.Content.ReadFromJsonAsync<List<Inquilino>>();
        return inquilinos ?? [];
    }

    public async Task<Inquilino> ObtenerInquilinoPorIdAsync(int id)
    {
        var peticion = new HttpRequestMessage(HttpMethod.Get, $"/api/Inquilinos/{id}");
        AplicarToken(peticion);

        HttpResponseMessage respuesta = await EnviarAsync(peticion);

        if (!respuesta.IsSuccessStatusCode)
        {
            throw new Exception("No se pudo obtener el inquilino.");
        }

        var inquilino = await respuesta.Content.ReadFromJsonAsync<Inquilino>();
        return inquilino ?? throw new Exception("No se encontró el inquilino.");
    }

    public async Task<List<Usuario>> ObtenerUsuariosInquilinoAsync()
    {
        var peticion = new HttpRequestMessage(HttpMethod.Get, "/api/Usuarios");
        AplicarToken(peticion);

        HttpResponseMessage respuesta = await EnviarAsync(peticion);

        if (!respuesta.IsSuccessStatusCode)
        {
            throw new Exception("No se pudo obtener la lista de usuarios.");
        }

        var usuarios = await respuesta.Content.ReadFromJsonAsync<List<Usuario>>();
        return usuarios?
            .Where(u => u.IdTipoUsuario == TipoUsuarioInquilino && u.Activo)
            .ToList() ?? [];
    }

    public async Task<Inquilino> CrearInquilinoAsync(InquilinoCreateRequest request)
    {
        var peticion = new HttpRequestMessage(HttpMethod.Post, "/api/Inquilinos")
        {
            Content = JsonContent.Create(request)
        };
        AplicarToken(peticion);

        HttpResponseMessage respuesta = await EnviarAsync(peticion);

        if (!respuesta.IsSuccessStatusCode)
        {
            string detalle = await ExtraerDetalleErrorAsync(respuesta);
            throw new Exception(detalle ?? "No se pudo crear el inquilino.");
        }

        var inquilino = await respuesta.Content.ReadFromJsonAsync<Inquilino>();
        return inquilino ?? throw new Exception("El servidor no devolvió el inquilino creado.");
    }

    public async Task<Inquilino> ActualizarInquilinoAsync(int id, InquilinoUpdateRequest request)
    {
        var peticion = new HttpRequestMessage(HttpMethod.Put, $"/api/Inquilinos/{id}")
        {
            Content = JsonContent.Create(request)
        };
        AplicarToken(peticion);

        HttpResponseMessage respuesta = await EnviarAsync(peticion);

        if (!respuesta.IsSuccessStatusCode)
        {
            string detalle = await ExtraerDetalleErrorAsync(respuesta);
            throw new Exception(detalle ?? "No se pudo actualizar el inquilino.");
        }

        var inquilino = await respuesta.Content.ReadFromJsonAsync<Inquilino>();
        return inquilino ?? throw new Exception("El servidor no devolvió el inquilino actualizado.");
    }

    public async Task ActualizarFechaFinAsync(int id, DateTime? fechaFin)
    {
        var peticion = new HttpRequestMessage(HttpMethod.Patch, $"/api/Inquilinos/{id}/fecha-fin")
        {
            Content = JsonContent.Create(new InquilinoFechaFinRequest { FechaFin = fechaFin })
        };
        AplicarToken(peticion);

        HttpResponseMessage respuesta = await EnviarAsync(peticion);

        if (!respuesta.IsSuccessStatusCode)
        {
            string detalle = await ExtraerDetalleErrorAsync(respuesta);
            throw new Exception(detalle ?? "No se pudo actualizar la fecha de salida.");
        }
    }

    public async Task EliminarInquilinoAsync(int id)
    {
        var peticion = new HttpRequestMessage(HttpMethod.Delete, $"/api/Inquilinos/{id}");
        AplicarToken(peticion);

        HttpResponseMessage respuesta = await EnviarAsync(peticion);

        if (!respuesta.IsSuccessStatusCode)
        {
            string detalle = await ExtraerDetalleErrorAsync(respuesta);
            throw new Exception(detalle ?? "No se pudo eliminar el inquilino.");
        }
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
