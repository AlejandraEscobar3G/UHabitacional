using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Net.NetworkInformation;
using System.Text.Json;
using UHabitacionalAPI.Domain.Enums;
using UHabitacionalAPI.Domain.Exceptions;
using UHabitacionalAPI.Presentation.Dtos;

namespace UHabitacionalAPI.Presentation.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DomainException ex)
            {
                int status = MapStatusCode(ex.InnerException ?? ex);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = status;

                ApiResponse<string> result = ApiResponse<string>.Fail(status, ex.Message, new List<string> { ex.Message });

                await context.Response.WriteAsync(JsonSerializer.Serialize(result));
            }
            catch (Exception ex)
            {
                int status = MapStatusCode(ex);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = status;

                ApiResponse<string> result = ApiResponse<string>.Fail(status, ex.Message, new List<string> { ex.Message });

                await context.Response.WriteAsync(JsonSerializer.Serialize(result));
            }
        }

        private static int MapStatusCode(Exception ex)
        {
            return ex switch
            {
                UhNotFoundException => StatusCodes.Status404NotFound,
                NotImplementedException => StatusCodes.Status501NotImplemented,
                ArgumentException => StatusCodes.Status400BadRequest,
                DbUpdateException => StatusCodes.Status409Conflict,
                TimeoutException => StatusCodes.Status503ServiceUnavailable,
                _ => StatusCodes.Status500InternalServerError
            };
        }
    }
}
