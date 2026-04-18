using System.ComponentModel.DataAnnotations;
using UHabitacionalAPI.Domain.Enums;

namespace UHabitacionalAPI.Presentation.Dtos
{
    public class TipoUsuarioRequest
    {
        [Required(ErrorMessage = "Campo 'Descripcion' es requerido.")]
        public string Descripcion { get; set; } = string.Empty;
        [Required(ErrorMessage = "Campo 'Estatus' es requerido.")]
        public EstatusTipoUsuario Estatus { get; set; }
    }
}
