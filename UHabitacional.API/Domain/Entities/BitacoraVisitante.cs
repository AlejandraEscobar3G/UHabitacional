using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UHabitacional.API.Domain.Entities;

/// <summary>
/// Tabla transaccional que registra entradas y salidas de personas que
/// no pertenecen a la unidad habitacional (visitantes).
/// </summary>
[Table("BitacoraVisitante")]
public class BitacoraVisitante
{
    [Key]
    public int IdBitacoraVisitante { get; set; }

    /// <summary>
    /// Inquilino que registra al visitante.
    /// </summary>
    [Required]
    public int IdInquilino { get; set; }

    [Required]
    [MaxLength(150)]
    public string NombreVisitante { get; set; } = string.Empty;

    [Required]
    public int IdIdentificacion { get; set; }

    [Required]
    [MaxLength(50)]
    public string NumeroIdentificacion { get; set; } = string.Empty;

    /// <summary>
    /// Código aleatorio de 6 caracteres (mayúscula + minúscula + número).
    /// Se genera al crear el registro.
    /// </summary>
    [Required]
    [MaxLength(6)]
    public string CodigoVisita { get; set; } = string.Empty;

    /// <summary>
    /// Solo se asigna mediante Update por parte de un vigilante.
    /// </summary>
    public DateTime? FechaHoraLlegada { get; set; }

    /// <summary>
    /// Solo se asigna mediante Update por parte de un vigilante.
    /// </summary>
    public DateTime? FechaHoraSalida { get; set; }

    [MaxLength(500)]
    public string? Observaciones { get; set; }

    /// <summary>
    /// Vigilante que registró la entrada (FechaHoraLlegada).
    /// </summary>
    public int? IdVigilanteEntrada { get; set; }

    /// <summary>
    /// Vigilante que registró la salida (FechaHoraSalida).
    /// </summary>
    public int? IdVigilanteSalida { get; set; }

    /// <summary>
    /// Indicador para borrado lógico.
    /// </summary>
    public bool Activo { get; set; } = true;

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public DateTime? FechaModificacion { get; set; }

    // Relaciones
    [ForeignKey(nameof(IdInquilino))]
    public virtual Inquilino? Inquilino { get; set; }

    [ForeignKey(nameof(IdIdentificacion))]
    public virtual Identificacion? Identificacion { get; set; }

    [ForeignKey(nameof(IdVigilanteEntrada))]
    public virtual Usuario? VigilanteEntrada { get; set; }

    [ForeignKey(nameof(IdVigilanteSalida))]
    public virtual Usuario? VigilanteSalida { get; set; }
}
