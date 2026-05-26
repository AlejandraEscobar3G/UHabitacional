using System.ComponentModel.DataAnnotations;

namespace UHabitacional.API.Application.DTOs;

// ============== BitacoraVigilante ==============

public record BitacoraVigilanteDto(
    int IdBitacoraVigilante,
    int IdUsuario,
    string? NombreVigilante,
    DateTime FechaHoraEntrada,
    DateTime? FechaHoraSalida,
    string? Observaciones);

public class BitacoraVigilanteCreateDto
{
    public DateTime? FechaHoraEntrada { get; set; }

    [MaxLength(500)]
    public string? Observaciones { get; set; }
}

public class BitacoraVigilanteUpdateDto
{
    public DateTime? FechaHoraSalida { get; set; }

    [MaxLength(500)]
    public string? Observaciones { get; set; }
}

// ============== BitacoraVisitante ==============

public record BitacoraVisitanteDto(
    int IdBitacoraVisitante,
    int IdInquilino,
    string? NombreInquilino,
    string NombreVisitante,
    int IdIdentificacion,
    string? Identificacion,
    string NumeroIdentificacion,
    string CodigoVisita,
    DateTime? FechaHoraLlegada,
    DateTime? FechaHoraSalida,
    string? Observaciones,
    DateTime FechaCreacion);

public class BitacoraVisitanteCreateDto
{
    [Required, MaxLength(150)]
    public string NombreVisitante { get; set; } = string.Empty;

    [Required]
    public int IdIdentificacion { get; set; }

    [Required, MaxLength(50)]
    public string NumeroIdentificacion { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Observaciones { get; set; }
}

/// <summary>
/// Update genérico que solo permite cambiar Observaciones y datos generales del visitante.
/// </summary>
public class BitacoraVisitanteUpdateDto
{
    [Required, MaxLength(150)]
    public string NombreVisitante { get; set; } = string.Empty;

    [Required]
    public int IdIdentificacion { get; set; }

    [Required, MaxLength(50)]
    public string NumeroIdentificacion { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Observaciones { get; set; }
}

/// <summary>
/// DTO específico para que el Vigilante registre la entrada/salida del visitante.
/// </summary>
public class BitacoraVisitanteRegistroDto
{
    /// <summary>
    /// Si es true: registra entrada (FechaHoraLlegada). Si es false: registra salida (FechaHoraSalida).
    /// </summary>
    public bool EsLlegada { get; set; }
}
