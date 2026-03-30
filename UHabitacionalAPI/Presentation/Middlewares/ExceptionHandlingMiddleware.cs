using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using UHabitacionalAPI.Domain.Enums;
using UHabitacionalAPI.Domain.Exceptions;

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
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = MapStatusCode(ex.InnerException ?? ex);

                var result = new
                {
                    error = ex.Message,
                    entity = ex.Entity,
                    operation = ex.Operation
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(result));
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = MapStatusCode(ex);

                var result = new
                {
                    error = ex.Message,
                    entity = DomainEntity.UNHANDLED_ENTITY,
                    operation = DomainOperation.UNHANDLED_OPERATION
                };

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
