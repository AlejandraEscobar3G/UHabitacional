namespace UHabitacional_Web.Models
{
    public class Departamento
    {
        public int Id { get; set; }
        public string NumeroInt { get; set; }
        public int Piso { get; set; }

        // Relacion con edificio
        public string EdificioId { get; set; }
        public Edificio Edificio { get; set; }


        // Campos de auditoria
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifyAt { get; set; }
        public int? ModifyBy { get; set; }

        // Relacion UNO a MUCHOS con BitacoraVisitante
        public List<BitacoraVisitante> BitacoraVisitantes { get; set; } = new List<BitacoraVisitante>();

        // Relacion UNO a MUCHOS con Inquilino
        public List<Inquilino> Inquilinos { get; set; } = new List<Inquilino>();

    }
}
