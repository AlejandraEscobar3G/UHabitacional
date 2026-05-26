using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using UHabitacional.MVC.Models;

namespace UHabitacional.MVC.Services;

/// <summary>
/// Cliente tipado para consumir la API de UHabitacional.
/// Adjunta automáticamente el JWT almacenado en Session y maneja errores.
/// </summary>
public interface IApiClient
{
    Task<T?> GetAsync<T>(string url, CancellationToken ct = default);
    Task<List<T>> GetListAsync<T>(string url, CancellationToken ct = default);
    Task<T?> PostAsync<T>(string url, object body, CancellationToken ct = default);
    Task<T?> PutAsync<T>(string url, object body, CancellationToken ct = default);
    Task<T?> PatchAsync<T>(string url, object body, CancellationToken ct = default);
    Task<bool> DeleteAsync(string url, CancellationToken ct = default);
    Task<HttpResponseMessage> SendRawAsync(HttpMethod method, string url, object? body, CancellationToken ct = default);
}

public class ApiClient : IApiClient
{
    private readonly HttpClient _http;
    private readonly IHttpContextAccessor _ctx;
    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public ApiClient(HttpClient http, IHttpContextAccessor ctx)
    {
        _http = http;
        _ctx = ctx;
    }

    private void AttachToken()
    {
        var token = _ctx.HttpContext?.Session.GetString("JwtToken");
        if (!string.IsNullOrEmpty(token))
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        else
            _http.DefaultRequestHeaders.Authorization = null;
    }

    public async Task<T?> GetAsync<T>(string url, CancellationToken ct = default)
    {
        AttachToken();
        var response = await _http.GetAsync(url, ct);
        await EnsureSuccessAsync(response);
        return await response.Content.ReadFromJsonAsync<T>(JsonOpts, ct);
    }

    public async Task<List<T>> GetListAsync<T>(string url, CancellationToken ct = default)
    {
        var result = await GetAsync<List<T>>(url, ct);
        return result ?? new List<T>();
    }

    public async Task<T?> PostAsync<T>(string url, object body, CancellationToken ct = default)
    {
        AttachToken();
        var response = await _http.PostAsync(url, BuildJsonContent(body), ct);
        await EnsureSuccessAsync(response);
        if (response.Content.Headers.ContentLength == 0) return default;
        return await response.Content.ReadFromJsonAsync<T>(JsonOpts, ct);
    }

    public async Task<T?> PutAsync<T>(string url, object body, CancellationToken ct = default)
    {
        AttachToken();
        var response = await _http.PutAsync(url, BuildJsonContent(body), ct);
        await EnsureSuccessAsync(response);
        if (response.Content.Headers.ContentLength == 0) return default;
        return await response.Content.ReadFromJsonAsync<T>(JsonOpts, ct);
    }

    public async Task<T?> PatchAsync<T>(string url, object body, CancellationToken ct = default)
    {
        AttachToken();
        var request = new HttpRequestMessage(HttpMethod.Patch, url)
        {
            Content = BuildJsonContent(body)
        };
        var response = await _http.SendAsync(request, ct);
        await EnsureSuccessAsync(response);
        if (response.Content.Headers.ContentLength == 0) return default;
        return await response.Content.ReadFromJsonAsync<T>(JsonOpts, ct);
    }

    public async Task<bool> DeleteAsync(string url, CancellationToken ct = default)
    {
        AttachToken();
        var response = await _http.DeleteAsync(url, ct);
        await EnsureSuccessAsync(response);
        return true;
    }

    public async Task<HttpResponseMessage> SendRawAsync(HttpMethod method, string url, object? body, CancellationToken ct = default)
    {
        AttachToken();
        var request = new HttpRequestMessage(method, url);
        if (body != null) request.Content = BuildJsonContent(body);
        return await _http.SendAsync(request, ct);
    }

    /// <summary>
    /// Construye el cuerpo JSON usando el tipo en tiempo de ejecución del objeto
    /// (evita serializar como `object` y obtener `{}`).
    /// </summary>
    private static HttpContent BuildJsonContent(object body)
    {
        var json = JsonSerializer.Serialize(body, body.GetType(), JsonOpts);
        return new StringContent(json, Encoding.UTF8, "application/json");
    }

    private static async Task EnsureSuccessAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode) return;

        string detail = string.Empty;
        try
        {
            var raw = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrWhiteSpace(raw))
            {
                try
                {
                    var err = JsonSerializer.Deserialize<ApiErrorDto>(raw, JsonOpts);
                    detail = err?.Detail ?? err?.Title ?? raw;
                }
                catch
                {
                    detail = raw;
                }
            }
        }
        catch { }

        throw new ApiException(response.StatusCode, detail);
    }
}

public class ApiException : Exception
{
    public HttpStatusCode StatusCode { get; }

    public ApiException(HttpStatusCode status, string message)
        : base(string.IsNullOrWhiteSpace(message) ? $"Error de la API ({(int)status})" : message)
    {
        StatusCode = status;
    }
}
