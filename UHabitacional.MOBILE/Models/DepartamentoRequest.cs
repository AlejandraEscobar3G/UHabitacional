namespace UHabitacional.MOBILE.Models;

public class DepartamentoCreateRequest
{
    public int IdEdificio { get; set; }
    public string NumeroDepartamento { get; set; } = string.Empty;
    public int Piso { get; set; }
}

public class DepartamentoUpdateRequest
{
    public int IdEdificio { get; set; }
    public string NumeroDepartamento { get; set; } = string.Empty;
    public int Piso { get; set; }
}
