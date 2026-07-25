namespace UHabitacional.MOBILE.Models;

public class TurnoVigilante
{
    public int IdBitacoraVigilante { get; set; }
    public int IdUsuario { get; set; }
    public string? NombreVigilante { get; set; }
    public DateTime FechaHoraEntrada { get; set; }
    public DateTime? FechaHoraSalida { get; set; }
    public string? Observaciones { get; set; }

    public bool EstaAbierto => !FechaHoraSalida.HasValue;

    public bool TieneObservaciones => !string.IsNullOrWhiteSpace(Observaciones);

    public string FechaTexto => FechaHoraEntrada.ToString("dd MMM yyyy");

    public string EntradaTexto => FechaHoraEntrada.ToString("HH:mm");

    public string SalidaTexto => FechaHoraSalida?.ToString("HH:mm") ?? "—";

    public string DuracionTexto
    {
        get
        {
            if (!FechaHoraSalida.HasValue)
            {
                return "Abierto";
            }

            TimeSpan dur = FechaHoraSalida.Value - FechaHoraEntrada;
            return $"{(int)dur.TotalHours}h {dur.Minutes:D2}m";
        }
    }
}

public class TurnoVigilanteCreateRequest
{
    public DateTime? FechaHoraEntrada { get; set; }
    public string? Observaciones { get; set; }
}

public class TurnoVigilanteUpdateRequest
{
    public DateTime? FechaHoraSalida { get; set; }
    public string? Observaciones { get; set; }
}
