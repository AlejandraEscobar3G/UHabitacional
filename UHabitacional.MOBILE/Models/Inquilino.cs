namespace UHabitacional.MOBILE.Models;

public class Inquilino
{
    public int IdInquilino { get; set; }
    public int IdUsuario { get; set; }
    public string? NombreUsuario { get; set; }
    public string? EmailUsuario { get; set; }
    public int IdDepartamento { get; set; }
    public string? NumeroDepartamento { get; set; }
    public string? NombreEdificio { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public bool EstaActivo { get; set; }

    public string NombreCompleto => NombreUsuario ?? "Sin nombre";

    public string InfoDepartamento => $"Depto. {NumeroDepartamento} · {NombreEdificio}";

    public string FechaInicioTexto => FechaInicio.ToString("dd/MM/yyyy");

    public string EstadoTexto => EstaActivo ? "Activo" : "Inactivo";

    public string Icono => "🏠";
}
