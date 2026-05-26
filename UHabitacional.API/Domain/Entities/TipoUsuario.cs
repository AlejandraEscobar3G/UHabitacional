using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UHabitacional.API.Domain.Entities;

/// <summary>
/// Catálogo de perfiles de usuario que existen en la unidad habitacional
/// (Administrador, Vigilante, Inquilino).
/// </summary>
[Table("TipoUsuario")]
public class TipoUsuario
{
    [Key]
    public int IdTipoUsuario { get; set; }

    [Required]
    [MaxLength(50)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(250)]
    public string? Descripcion { get; set; }

    public bool Activo { get; set; } = true;

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public DateTime? FechaModificacion { get; set; }

    // Relaciones
    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
