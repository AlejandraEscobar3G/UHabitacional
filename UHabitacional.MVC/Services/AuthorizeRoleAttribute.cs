using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace UHabitacional.MVC.Services;

/// <summary>
/// Filtro simple para exigir que haya sesión activa y, opcionalmente, un rol específico.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class AuthorizeRoleAttribute : ActionFilterAttribute
{
    public string? Roles { get; }

    public AuthorizeRoleAttribute(string? roles = null) => Roles = roles;

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var current = context.HttpContext.RequestServices.GetService(typeof(ICurrentUser)) as ICurrentUser;

        if (current == null || !current.IsAuthenticated)
        {
            context.Result = new RedirectToActionResult("Login", "Auth", null);
            return;
        }

        if (!string.IsNullOrWhiteSpace(Roles))
        {
            var permitidos = Roles.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (!permitidos.Any(r => string.Equals(r, current.TipoUsuario, StringComparison.OrdinalIgnoreCase)))
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Auth", null);
                return;
            }
        }
    }
}
