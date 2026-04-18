using UHabitacionalAPI.Domain.Enums;

namespace UHabitacionalAPI.Presentation.Dtos
{
    public class TipoUsuarioFilterRequest
    {
        public string? Descripcion { get; set; }
        public EstatusTipoUsuario? Estatus { get; set; }
    }
}
