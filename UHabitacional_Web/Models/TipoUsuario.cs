namespace UHabitacional_Web.Models
{
    public class TipoUsuario
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }

        // Campos de auditoria
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifyAt { get; set; }
        public int? ModifyBy { get; set; }

        // Relación UNO a MUCHOS con Usuario
        public List<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
