namespace UHabitacional.API.Application.DTOs;

/// <summary>
/// Representa una sesión consultable por el usuario.
/// </summary>
public record SesionDto(
    int IdSesion,
    int IdUsuario,
    string? NombreUsuario,
    string Jti,
    DateTime FechaInicio,
    DateTime FechaExpiracion,
    DateTime FechaUltimaActividad,
    DateTime? FechaCierre,
    string? DireccionIP,
    string? UserAgent,
    bool Activa,
    bool EsSesionActual);

public record LogoutResponseDto(
    int IdSesion,
    string Mensaje,
    DateTime FechaCierre);
