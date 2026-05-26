using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UHabitacional.API.Domain.Entities;

/// <summary>
/// Catálogo de departamentos pertenecientes a los edificios.
/// Solo soporta operaciones CRU (Create / Read / Update).
/// </summary>
[Table("Departamento")]
public class Departamento
{
    [Key]
    public int IdDepartamento { get; set; }

    [Required]
    public int IdEdificio { get; set; }

    [Required]
    [MaxLength(20)]
    public string NumeroDepartamento { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int Piso { get; set; }

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public DateTime? FechaModificacion { get; set; }

    // Relaciones
    [ForeignKey(nameof(IdEdificio))]
    public virtual Edificio? Edificio { get; set; }

    public virtual ICollection<Inquilino> Inquilinos { get; set; } = new List<Inquilino>();
}
