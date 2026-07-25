namespace UHabitacional.MOBILE.Models;

public class VisitanteCreateRequest
{
    public string NombreVisitante { get; set; } = string.Empty;
    public int IdIdentificacion { get; set; }
    public string NumeroIdentificacion { get; set; } = string.Empty;
    public string? Observaciones { get; set; }
}

public class VisitanteUpdateRequest
{
    public string NombreVisitante { get; set; } = string.Empty;
    public int IdIdentificacion { get; set; }
    public string NumeroIdentificacion { get; set; } = string.Empty;
    public string? Observaciones { get; set; }
}
