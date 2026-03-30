using UHabitacionalAPI.Domain.Enums;

namespace UHabitacionalAPI.Presentation.Dtos
{
    public class EdificioFilterRequest
    {
        public EstatusEdificio? Estatus { get; set; }
        public int? NumeroPisos { get; set; }
        public int? TotalDeptos { get; set; }
    }
}
