using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UHabitacional.API.Domain.Entities;

/// <summary>
/// Catálogo de usuarios registrados en la unidad habitacional.
/// Tipos: Administrador, Vigilante, Inquilino.
/// </summary>
[Table("Usuario")]
public class Usuario
{
    [Key]
    public int IdUsuario { get; set; }

    [Required]
    public int IdTipoUsuario { get; set; }

    [Required]
    public int IdIdentificacion { get; set; }

    [Required]
    [MaxLength(50)]
    public string NumeroIdentificacion { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string Apellidos { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string PasswordHash { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? Telefono { get; set; }

    /// <summary>
    /// Indicador para borrado lógico.
    /// </summary>
    public bool Activo { get; set; } = true;

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public DateTime? FechaModificacion { get; set; }

    // Relaciones
    [ForeignKey(nameof(IdTipoUsuario))]
    public virtual TipoUsuario? TipoUsuario { get; set; }

    [ForeignKey(nameof(IdIdentificacion))]
    public virtual Identificacion? Identificacion { get; set; }

    public virtual Inquilino? Inquilino { get; set; }
    public virtual ICollection<BitacoraVigilante> BitacorasVigilante { get; set; } = new List<BitacoraVigilante>();
    public virtual ICollection<Sesion> Sesiones { get; set; } = new List<Sesion>();
}
