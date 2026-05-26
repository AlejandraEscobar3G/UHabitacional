using UHabitacional.MVC.Models;

namespace UHabitacional.MVC.ViewModels;

public class ChecadorViewModel
{
    public BitacoraVigilanteDto? TurnoAbierto { get; set; }
    public List<BitacoraVigilanteDto> Historial { get; set; } = new();
}
