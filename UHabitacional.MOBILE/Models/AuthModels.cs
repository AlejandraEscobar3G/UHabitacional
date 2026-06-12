namespace UHabitacional.MOBILE.Models;

public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginResponse
{
    public int IdUsuario { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string TipoUsuario { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public DateTime Expira { get; set; }
}
