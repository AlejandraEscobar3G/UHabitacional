using UHabitacionalAPI.Domain.Enums;

namespace UHabitacionalAPI.Presentation.Dtos
{
    public class IdentificacionFilterRequest
    {
        public string? Descripcion { get; set; }
        public EstatusIdentificacion? Estatus { get; set; }
    }
}
