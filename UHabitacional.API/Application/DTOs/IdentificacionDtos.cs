using System.ComponentModel.DataAnnotations;

namespace UHabitacional.API.Application.DTOs;

public record IdentificacionDto(
    int IdIdentificacion,
    string Nombre,
    string? Descripcion,
    bool Activo);

public class IdentificacionCreateDto
{
    [Required, MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(250)]
    public string? Descripcion { get; set; }
}

public class IdentificacionUpdateDto
{
    [Required, MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(250)]
    public string? Descripcion { get; set; }
}
