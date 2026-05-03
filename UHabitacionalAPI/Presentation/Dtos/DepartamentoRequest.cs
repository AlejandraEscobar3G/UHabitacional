using System.ComponentModel.DataAnnotations;

namespace UHabitacionalAPI.Presentation.Dtos
{
    public class DepartamentoRequest
    {
        [Required(ErrorMessage = "Campo 'NumeroInt' es requerido.")]
        public string NumeroInt { get; set; }
        [Required(ErrorMessage = "Campo 'Piso' es requerido.")]
        public int Piso { get; set; }
        [Required(ErrorMessage = "Campo 'EdificioId' es requerido.")]
        public string EdificioId { get; set; }
    }
}
