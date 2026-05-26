using System.ComponentModel.DataAnnotations;

namespace UHabitacional.MVC.ViewModels;

public class InquilinoFormViewModel
{
    public int IdInquilino { get; set; }
    public int IdUsuario { get; set; }
    public int IdTipoUsuario { get; set; }

    [Required, Range(1, int.MaxValue, ErrorMessage = "Selecciona el tipo de identificación")]
    public int IdIdentificacion { get; set; }

    [Required(ErrorMessage = "El número de identificación es obligatorio"), MaxLength(50)]
    public string NumeroIdentificacion { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es obligatorio"), MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "Los apellidos son obligatorios"), MaxLength(150)]
    public string Apellidos { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? Telefono { get; set; }

    [MinLength(6, ErrorMessage = "Mínimo 6 caracteres")]
    public string? Password { get; set; }

    [Required, Range(1, int.MaxValue, ErrorMessage = "Selecciona un departamento")]
    public int IdDepartamento { get; set; }

    [DataType(DataType.Date)]
    public DateTime? FechaInicio { get; set; }

    [DataType(DataType.Date)]
    public DateTime? FechaFin { get; set; }
}
