namespace UHabitacional.MOBILE.Models;

public class EdificioRequest
{
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public int NumeroPisos { get; set; }
    public int TotalDeptos { get; set; }
}
