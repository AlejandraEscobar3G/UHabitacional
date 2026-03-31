using UHabitacionalAPI.Domain.Enums;

namespace UHabitacionalAPI.Presentation.Dtos
{
    public class IdentificacionResponse
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public EstatusIdentificacion Estatus { get; set; }
    }
}
