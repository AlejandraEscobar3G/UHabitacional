using UHabitacionalAPI.Domain.Enums;

namespace UHabitacionalAPI.Presentation.Dtos
{
    public class EdificioResponse
    {
        public string Id { get; set; }
        public string Calle { get; set; }
        public int TotalDeptos { get; set; }
        public int NumeroPisos { get; set; }
        public EstatusEdificio Estatus { get; set; } = EstatusEdificio.Activo;
    }
}
