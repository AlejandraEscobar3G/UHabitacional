namespace UHabitacional_Web.Models
{
    public class Inquilino
    {
        public int Id { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }


        // Relacion con Usuario
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        // Relacion con Departamento
        public int DepartamentoId { get; set; }
        public Departamento Departamento { get; set; }

        // Campos de auditoria
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifyAt { get; set; }
        public int? ModifyBy { get; set; }
    }
}
