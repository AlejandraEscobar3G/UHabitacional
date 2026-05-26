using System.ComponentModel.DataAnnotations;

namespace UHabitacional.API.Application.DTOs;

public record TipoUsuarioDto(
    int IdTipoUsuario,
    string Nombre,
    string? Descripcion,
    bool Activo);

public class TipoUsuarioCreateDto
{
    [Required, MaxLength(50)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(250)]
    public string? Descripcion { get; set; }
}

public class TipoUsuarioUpdateDto
{
    [Required, MaxLength(50)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(250)]
    public string? Descripcion { get; set; }
}
