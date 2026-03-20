using System.ComponentModel.DataAnnotations;

namespace UHabitacional_Web.Models
{
    public enum EstatusEdificio
    {
        [Display(Name = "Demolido")] Demolido = 0,
        [Display(Name = "Activo")] Activo = 1,
        [Display(Name = "En construcción")] EnConstruccion = 2,
        [Display(Name = "En reparación")] EnReparacion = 3,
        [Display(Name = "Clausurado")] Clausurado = 4
    }
}
