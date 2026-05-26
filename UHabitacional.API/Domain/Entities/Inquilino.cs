using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UHabitacional.API.Domain.Entities;

/// <summary>
/// Representa un inquilino asociado a un usuario y a un departamento.
/// Un inquilino se considera "Activo" cuando FechaFin es null.
/// </summary>
[Table("Inquilino")]
public class Inquilino
{
    [Key]
    public int IdInquilino { get; set; }

    [Required]
    public int IdUsuario { get; set; }

    [Required]
    public int IdDepartamento { get; set; }

    public DateTime FechaInicio { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Fecha de fin del periodo del inquilino. Si es null, el inquilino se considera Activo.
    /// Solo el Administrador puede actualizar este campo.
    /// </summary>
    public DateTime? FechaFin { get; set; }

    /// <summary>
    /// Indicador para borrado lógico.
    /// </summary>
    public bool Activo { get; set; } = true;

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public DateTime? FechaModificacion { get; set; }

    // Relaciones
    [ForeignKey(nameof(IdUsuario))]
    public virtual Usuario? Usuario { get; set; }

    [ForeignKey(nameof(IdDepartamento))]
    public virtual Departamento? Departamento { get; set; }

    public virtual ICollection<BitacoraVisitante> BitacorasVisitante { get; set; } = new List<BitacoraVisitante>();
}
