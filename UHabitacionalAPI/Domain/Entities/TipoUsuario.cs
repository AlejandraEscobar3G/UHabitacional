using UHabitacionalAPI.Domain.Enums;

namespace UHabitacionalAPI.Domain.Entities
{
    public class TipoUsuario
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public EstatusTipoUsuario Estatus { get; set; } = EstatusTipoUsuario.Activo;

        // Campos de auditoria
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifyAt { get; set; }
        public int? ModifyBy { get; set; }

        // Relación UNO a MUCHOS con Usuario
        public List<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
