namespace UHabitacional.MOBILE.Models;

public class Visitante
{
    public int IdBitacoraVisitante { get; set; }
    public int IdInquilino { get; set; }
    public string? NombreInquilino { get; set; }
    public string NombreVisitante { get; set; } = string.Empty;
    public int IdIdentificacion { get; set; }
    public string? Identificacion { get; set; }
    public string NumeroIdentificacion { get; set; } = string.Empty;
    public string CodigoVisita { get; set; } = string.Empty;
    public DateTime? FechaHoraLlegada { get; set; }
    public DateTime? FechaHoraSalida { get; set; }
    public string? Observaciones { get; set; }
    public DateTime FechaCreacion { get; set; }

    public string EstadoTexto
    {
        get
        {
            if (FechaHoraSalida.HasValue) return "Salió";
            if (FechaHoraLlegada.HasValue) return "En visita";
            return "Pendiente";
        }
    }

    public string InfoIdentificacion => $"{Identificacion} · {NumeroIdentificacion}";

    public string FechaCreacionTexto => FechaCreacion.ToString("dd/MM/yyyy");

    public string Icono => "🚶";
}
