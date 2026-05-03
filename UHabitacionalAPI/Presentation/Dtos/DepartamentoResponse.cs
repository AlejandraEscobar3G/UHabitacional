namespace UHabitacionalAPI.Presentation.Dtos
{
    public class DepartamentoResponse
    {
        public int Id { get; set; }
        public string NumeroInt { get; set; }
        public int Piso { get; set; }
        public EdificioResponse Edificio { get; set; }
    }
}
