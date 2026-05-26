namespace UHabitacional.MVC.Services;

/// <summary>
/// Acceso al usuario logueado a partir de la sesión.
/// </summary>
public interface ICurrentUser
{
    bool IsAuthenticated { get; }
    int IdUsuario { get; }
    string Email { get; }
    string Nombre { get; }
    string TipoUsuario { get; }
    string Iniciales { get; }
    bool EsAdministrador { get; }
    bool EsInquilino { get; }
    bool EsVigilante { get; }
    void SetSession(int id, string email, string nombre, string tipoUsuario, string token, DateTime expira);
    void Clear();
}

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _ctx;

    public CurrentUser(IHttpContextAccessor ctx) => _ctx = ctx;

    private ISession? S => _ctx.HttpContext?.Session;

    public bool IsAuthenticated => !string.IsNullOrEmpty(S?.GetString("JwtToken"));
    public int IdUsuario => S?.GetInt32("IdUsuario") ?? 0;
    public string Email => S?.GetString("Email") ?? string.Empty;
    public string Nombre => S?.GetString("Nombre") ?? string.Empty;
    public string TipoUsuario => S?.GetString("TipoUsuario") ?? string.Empty;

    public string Iniciales
    {
        get
        {
            var n = Nombre;
            if (string.IsNullOrWhiteSpace(n)) return "U";
            var parts = n.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1) return parts[0][..1].ToUpper();
            return (parts[0][..1] + parts[1][..1]).ToUpper();
        }
    }

    public bool EsAdministrador => TipoUsuario.Equals("Administrador", StringComparison.OrdinalIgnoreCase);
    public bool EsInquilino => TipoUsuario.Equals("Inquilino", StringComparison.OrdinalIgnoreCase);
    public bool EsVigilante => TipoUsuario.Equals("Vigilante", StringComparison.OrdinalIgnoreCase);

    public void SetSession(int id, string email, string nombre, string tipoUsuario, string token, DateTime expira)
    {
        if (S == null) return;
        S.SetInt32("IdUsuario", id);
        S.SetString("Email", email);
        S.SetString("Nombre", nombre);
        S.SetString("TipoUsuario", tipoUsuario);
        S.SetString("JwtToken", token);
        S.SetString("TokenExpira", expira.ToString("o"));
    }

    public void Clear() => S?.Clear();
}
