using UHabitacionalAPI.Domain.Enums;

namespace UHabitacionalAPI.Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Correo { get; set; }
        public EstatusUsuario Estatus { get; set; }

        // Relacion con TipoUsuario
        public int TipoUsuarioId { get; set; }
        public TipoUsuario TipoUsuario { get; set; }

        // Campos de auditoria
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifyAt { get; set; }
        public int? ModifyBy { get; set; }

        // Relacion UNO a MUCHOS con BitacoraVigilante
        public List<BitacoraVigilante> BitacoraVigilantes { get; set; } = new List<BitacoraVigilante>();

        // Relacion UNO a UNO con Inquilino
        public Inquilino Inquilino { get; set; }
    }
}
