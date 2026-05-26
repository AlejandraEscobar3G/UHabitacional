using System.ComponentModel.DataAnnotations;

namespace UHabitacional.API.Application.DTOs;

public record InquilinoDto(
    int IdInquilino,
    int IdUsuario,
    string? NombreUsuario,
    string? EmailUsuario,
    int IdDepartamento,
    string? NumeroDepartamento,
    string? NombreEdificio,
    DateTime FechaInicio,
    DateTime? FechaFin,
    bool EstaActivo);

public class InquilinoCreateDto
{
    [Required]
    public int IdUsuario { get; set; }

    [Required]
    public int IdDepartamento { get; set; }

    public DateTime? FechaInicio { get; set; }
}

public class InquilinoUpdateDto
{
    [Required]
    public int IdDepartamento { get; set; }

    public DateTime? FechaInicio { get; set; }
}

/// <summary>
/// DTO específico para que el Administrador actualice FechaFin.
/// </summary>
public class InquilinoFechaFinDto
{
    public DateTime? FechaFin { get; set; }
}
