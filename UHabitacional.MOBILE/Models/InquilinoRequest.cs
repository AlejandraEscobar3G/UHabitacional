namespace UHabitacional.MOBILE.Models;

public class InquilinoCreateRequest
{
    public int IdUsuario { get; set; }
    public int IdDepartamento { get; set; }
    public DateTime? FechaInicio { get; set; }
}

public class InquilinoUpdateRequest
{
    public int IdDepartamento { get; set; }
    public DateTime? FechaInicio { get; set; }
}

public class InquilinoFechaFinRequest
{
    public DateTime? FechaFin { get; set; }
}
