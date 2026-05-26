using UHabitacional.MVC.Models;

namespace UHabitacional.MVC.ViewModels;

public class RegistroVisitanteViewModel
{
    public string Codigo { get; set; } = string.Empty;
    public BitacoraVisitanteDto? Visitante { get; set; }
    public string? Mensaje { get; set; }
    public bool EsError { get; set; }
}
