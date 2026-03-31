using UHabitacionalAPI.Domain.Enums;

namespace UHabitacionalAPI.Presentation.Dtos
{
    public class IdentificacionRequest
    {
        public string Descripcion { get; set; } = string.Empty;
        public EstatusIdentificacion Estatus { get; set; }
    }
}
