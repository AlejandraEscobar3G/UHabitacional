using System.ComponentModel.DataAnnotations;

namespace UHabitacional.API.Application.DTOs;

public record DepartamentoDto(
    int IdDepartamento,
    int IdEdificio,
    string? NombreEdificio,
    string NumeroDepartamento,
    int Piso);

public class DepartamentoCreateDto
{
    [Required]
    public int IdEdificio { get; set; }

    [Required, MaxLength(20)]
    public string NumeroDepartamento { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int Piso { get; set; }
}

public class DepartamentoUpdateDto
{
    [Required]
    public int IdEdificio { get; set; }

    [Required, MaxLength(20)]
    public string NumeroDepartamento { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int Piso { get; set; }
}
