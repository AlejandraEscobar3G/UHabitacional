namespace UHabitacional.MOBILE.Models;

public class Departamento
{
    public int IdDepartamento { get; set; }
    public int IdEdificio { get; set; }
    public string? NombreEdificio { get; set; }
    public string NumeroDepartamento { get; set; } = string.Empty;
    public int Piso { get; set; }
}
