namespace UHabitacional.MOBILE.Models;

public class Edificio
{
    public int IdEdificio { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public int NumeroPisos { get; set; }
    public int TotalDeptos { get; set; }

    public string DeptosTexto => $"{TotalDeptos} deptos";

    public string NivelesTexto => $"{NumeroPisos} niveles";

    public string Icono => "🏢";
}
