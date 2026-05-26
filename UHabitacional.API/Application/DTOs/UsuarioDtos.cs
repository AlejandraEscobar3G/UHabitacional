using System.ComponentModel.DataAnnotations;

namespace UHabitacional.API.Application.DTOs;

public record UsuarioDto(
    int IdUsuario,
    int IdTipoUsuario,
    string? TipoUsuario,
    int IdIdentificacion,
    string? Identificacion,
    string NumeroIdentificacion,
    string Nombre,
    string Apellidos,
    string Email,
    string? Telefono,
    bool Activo);

public class UsuarioCreateDto
{
    [Required]
    public int IdTipoUsuario { get; set; }

    [Required]
    public int IdIdentificacion { get; set; }

    [Required, MaxLength(50)]
    public string NumeroIdentificacion { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required, MaxLength(150)]
    public string Apellidos { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(6), MaxLength(100)]
    public string Password { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? Telefono { get; set; }
}

public class UsuarioUpdateDto
{
    [Required]
    public int IdTipoUsuario { get; set; }

    [Required]
    public int IdIdentificacion { get; set; }

    [Required, MaxLength(50)]
    public string NumeroIdentificacion { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required, MaxLength(150)]
    public string Apellidos { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Password { get; set; }

    [MaxLength(20)]
    public string? Telefono { get; set; }
}

public class LoginDto
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}

public record LoginResponseDto(
    int IdUsuario,
    string Email,
    string Nombre,
    string TipoUsuario,
    string Token,
    DateTime Expira);
