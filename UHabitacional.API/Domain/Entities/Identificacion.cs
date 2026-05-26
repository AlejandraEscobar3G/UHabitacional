using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UHabitacional.API.Domain.Entities;

/// <summary>
/// Catálogo de identificaciones válidas que un visitante puede presentar
/// para ingresar a la unidad habitacional.
/// </summary>
[Table("Identificacion")]
public class Identificacion
{
    [Key]
    public int IdIdentificacion { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(250)]
    public string? Descripcion { get; set; }

    /// <summary>
    /// Indicador para borrado lógico. true = activo, false = eliminado.
    /// </summary>
    public bool Activo { get; set; } = true;

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public DateTime? FechaModificacion { get; set; }

    // Relaciones
    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    public virtual ICollection<BitacoraVisitante> BitacorasVisitante { get; set; } = new List<BitacoraVisitante>();
}
