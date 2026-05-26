using System.Net;
using System.Text.Json;
using UHabitacional.API.Domain.Exceptions;

namespace UHabitacional.API.Middleware;

/// <summary>
/// Middleware global para manejar excepciones del dominio y devolver respuestas
/// estandarizadas al cliente.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var (statusCode, title) = ex switch
        {
            NotFoundException        => ((int)HttpStatusCode.NotFound, "Recurso no encontrado"),
            ForbiddenOperationException => ((int)HttpStatusCode.Forbidden, "Operación no permitida"),
            BusinessRuleException    => ((int)HttpStatusCode.BadRequest, "Regla de negocio violada"),
            ValidationException      => ((int)HttpStatusCode.BadRequest, "Datos inválidos"),
            UnauthorizedAccessException => ((int)HttpStatusCode.Unauthorized, "No autenticado"),
            _                        => ((int)HttpStatusCode.InternalServerError, "Error inesperado")
        };

        if (statusCode == (int)HttpStatusCode.InternalServerError)
            _logger.LogError(ex, "Error no controlado en la API");
        else
            _logger.LogWarning(ex, "Excepción controlada: {Type}", ex.GetType().Name);

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";

        object payload;
        if (ex is ValidationException ve && ve.Errors.Count > 0)
        {
            payload = new
            {
                title,
                status = statusCode,
                detail = ex.Message,
                errors = ve.Errors
            };
        }
        else
        {
            payload = new
            {
                title,
                status = statusCode,
                detail = statusCode == (int)HttpStatusCode.InternalServerError
                    ? "Ocurrió un error inesperado, contacte al administrador."
                    : ex.Message
            };
        }

        var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}
