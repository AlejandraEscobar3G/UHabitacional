using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UHabitacional.API.Domain.Entities;

/// <summary>
/// Catálogo de edificios que existen en la unidad habitacional.
/// Solo soporta operaciones CRU (Create / Read / Update).
/// </summary>
[Table("Edificio")]
public class Edificio
{
    [Key]
    public int IdEdificio { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(250)]
    public string? Descripcion { get; set; }

    /// <summary>
    /// Número total de pisos del edificio. Usado para validar el campo Piso de Departamento.
    /// </summary>
    [Range(1, int.MaxValue)]
    public int NumeroPisos { get; set; }

    /// <summary>
    /// Número total de departamentos del edificio. Usado para validar
    /// que no se excedan al crear departamentos.
    /// </summary>
    [Range(1, int.MaxValue)]
    public int TotalDeptos { get; set; }

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public DateTime? FechaModificacion { get; set; }

    // Relaciones
    public virtual ICollection<Departamento> Departamentos { get; set; } = new List<Departamento>();
}
