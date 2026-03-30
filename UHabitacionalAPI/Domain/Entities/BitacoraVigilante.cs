namespace UHabitacionalAPI.Domain.Entities
{
    public class BitacoraVigilante
    {
        public int Id { get; set; }

        // Relacion con Usuario
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        // Campos de auditoria
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifyAt { get; set; }
        public int? ModifyBy { get; set; }
    }
}
