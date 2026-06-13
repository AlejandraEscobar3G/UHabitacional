namespace UHabitacional.MOBILE.Models;

public class Perfil
{
    public int IdTipoUsuario { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool Activo { get; set; }

    public string EstadoTexto => Activo ? "Activo" : "Inactivo";

    public string Icono => Nombre.Trim().ToLowerInvariant() switch
    {
        "administrador" => "🛡️",
        "vigilante" => "👮",
        "inquilino" => "🧑",
        _ => "👤"
    };
}
