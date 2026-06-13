namespace UHabitacional.MOBILE.Models;

public class Usuario
{
    public int IdUsuario { get; set; }
    public int IdTipoUsuario { get; set; }
    public string TipoUsuario { get; set; } = string.Empty;
    public int IdIdentificacion { get; set; }
    public string Identificacion { get; set; } = string.Empty;
    public string NumeroIdentificacion { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public bool Activo { get; set; }

    public string NombreCompleto => $"{Nombre} {Apellidos}".Trim();

    public string Contacto => Email;

    public string EstadoTexto => Activo ? "Activo" : "Inactivo";

    public string Icono => "👮";
}
