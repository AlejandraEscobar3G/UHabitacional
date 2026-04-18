using UHabitacionalAPI.Domain.Enums;

namespace UHabitacionalAPI.Presentation.Dtos
{
    public class TipoUsuarioResponse
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public EstatusTipoUsuario Estatus { get; set; }
    }
}
