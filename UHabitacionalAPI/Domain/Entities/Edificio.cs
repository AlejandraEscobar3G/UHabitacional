using UHabitacionalAPI.Domain.Enums;

namespace UHabitacionalAPI.Domain.Entities
{
    public class Edificio
    {
        public string Id { get; set; }
        public string Calle { get; set; }
        public int TotalDeptos { get; set; }
        public int NumeroPisos { get; set; }
        public EstatusEdificio Estatus { get; set; } = EstatusEdificio.Activo;

        // Campos de auditoria
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifyAt { get; set; }
        public int? ModifyBy { get; set; }

        // Relacion UNO a MUCHOS con Departamento
        public List<Departamento> Departamentos { get; set; } = new List<Departamento>();
    }
}
