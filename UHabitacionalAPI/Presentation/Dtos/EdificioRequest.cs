using System.ComponentModel.DataAnnotations;
using UHabitacionalAPI.Domain.Enums;

namespace UHabitacionalAPI.Presentation.Dtos
{
    public class EdificioRequest
    {
        [Required(ErrorMessage = "Campo 'Calle' es requerido.")]
        public string Calle { get; set; }

        [Required(ErrorMessage = "Campo 'TotalDeptos' es requerido.")]
        [Range(1, 50, ErrorMessage = "Campo 'TotalDeptos' debe estar entre 1 y 50.")]
        public int TotalDeptos { get; set; }

        [Required(ErrorMessage = "Campo 'NumeroPisos' es requerido.")]
        [Range(1, 12, ErrorMessage = "Campo 'NumeroPisos' debe estar entre 1 y 12.")]
        public int NumeroPisos { get; set; }

        [Required(ErrorMessage = "Campo 'Estatus' es requerido.")]
        public EstatusEdificio Estatus { get; set; } = EstatusEdificio.Activo;
    }
}
