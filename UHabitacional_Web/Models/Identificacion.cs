namespace UHabitacional_Web.Models
{
    public class Identificacion
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }

        // Campos de auditoria
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifyAt { get; set; }
        public int? ModifyBy { get; set; }

        // Relacion UNO a MUCHOS con BitacoraVisitante
        public List<BitacoraVisitante> BitacoraVisitantes { get; set; } = new List<BitacoraVisitante>();
    }
}
