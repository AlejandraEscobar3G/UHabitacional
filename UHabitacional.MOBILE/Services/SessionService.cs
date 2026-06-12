using UHabitacional.MOBILE.Models;

namespace UHabitacional.MOBILE.Services;

/*
 * Servicio para almacenamiento de sesión
 */
public static class SessionService
{
    public static string Token { get; private set; } = string.Empty;
    public static string Nombre { get; private set; } = string.Empty;
    public static string TipoUsuario { get; private set; } = string.Empty;
    public static int IdUsuario { get; private set; }

    public static void GuardarSesion(LoginResponse respuesta)
    {
        Token = respuesta.Token;
        Nombre = respuesta.Nombre;
        TipoUsuario = respuesta.TipoUsuario;
        IdUsuario = respuesta.IdUsuario;
    }

    public static void Limpiar()
    {
        Token = string.Empty;
        Nombre = string.Empty;
        TipoUsuario = string.Empty;
        IdUsuario = 0;
    }
}
