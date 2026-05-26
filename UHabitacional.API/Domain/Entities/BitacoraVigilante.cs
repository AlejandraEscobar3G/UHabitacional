using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UHabitacional.API.Domain.Entities;

/// <summary>
/// Tabla transaccional que registra las entradas y salidas
/// del personal de vigilancia.
/// </summary>
[Table("BitacoraVigilante")]
public class BitacoraVigilante
{
    [Key]
    public int IdBitacoraVigilante { get; set; }

    [Required]
    public int IdUsuario { get; set; }

    [Required]
    public DateTime FechaHoraEntrada { get; set; }

    public DateTime? FechaHoraSalida { get; set; }

    [MaxLength(500)]
    public string? Observaciones { get; set; }

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public DateTime? FechaModificacion { get; set; }

    // Relaciones
    [ForeignKey(nameof(IdUsuario))]
    public virtual Usuario? Usuario { get; set; }
}
