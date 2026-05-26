using System.ComponentModel.DataAnnotations;

namespace UHabitacional.API.Application.DTOs;

public record EdificioDto(
    int IdEdificio,
    string Nombre,
    string? Descripcion,
    int NumeroPisos,
    int TotalDeptos);

public class EdificioCreateDto
{
    [Required, MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(250)]
    public string? Descripcion { get; set; }

    [Range(1, int.MaxValue)]
    public int NumeroPisos { get; set; }

    [Range(1, int.MaxValue)]
    public int TotalDeptos { get; set; }
}

public class EdificioUpdateDto
{
    [Required, MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(250)]
    public string? Descripcion { get; set; }

    [Range(1, int.MaxValue)]
    public int NumeroPisos { get; set; }

    [Range(1, int.MaxValue)]
    public int TotalDeptos { get; set; }
}
