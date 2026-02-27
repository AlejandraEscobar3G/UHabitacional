namespace UHabitacional_Web.Models
{
    public class BitacoraVisitante
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Correo { get; set; }
        public int Estatus { get; set; }
        public DateTime? FechaHoraLlegada { get; set; }
        public DateTime? FechaHoraSalida { get; set; }
        public int codigoVisita { get; set; }

        // Relacion con Identificacion
        public int IdentificacionId { get; set; }
        public Identificacion Identificacion { get; set; }

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
