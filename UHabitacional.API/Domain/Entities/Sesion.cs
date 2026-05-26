using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UHabitacional.API.Domain.Entities;

/// <summary>
/// Representa una sesión activa de un usuario. Se persiste al hacer login
/// y se invalida en logout. El middleware de validación de sesión consulta
/// esta tabla en cada request autenticada para verificar que el JWT esté
/// asociado a una sesión vigente.
/// </summary>
[Table("Sesion")]
public class Sesion
{
    [Key]
    public int IdSesion { get; set; }

    [Required]
    public int IdUsuario { get; set; }

    /// <summary>
    /// JWT ID (claim 'jti'). Identificador único del token; se almacena en
    /// lugar del JWT completo para evitar exponer el token en BD.
    /// </summary>
    [Required]
    [MaxLength(64)]
    public string Jti { get; set; } = string.Empty;

    /// <summary>
    /// Fecha y hora de inicio de la sesión (login).
    /// </summary>
    public DateTime FechaInicio { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Fecha y hora en que el JWT expira (claim 'exp').
    /// </summary>
    public DateTime FechaExpiracion { get; set; }

    /// <summary>
    /// Última vez que el usuario interactuó con la API en esta sesión.
    /// Se actualiza por el middleware en cada request válida.
    /// </summary>
    public DateTime FechaUltimaActividad { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Fecha y hora del cierre explícito (logout). Null si la sesión sigue abierta.
    /// </summary>
    public DateTime? FechaCierre { get; set; }

    [MaxLength(45)]
    public string? DireccionIP { get; set; }

    [MaxLength(500)]
    public string? UserAgent { get; set; }

    /// <summary>
    /// true mientras la sesión esté vigente (no se ha hecho logout ni expirada).
    /// </summary>
    public bool Activa { get; set; } = true;

    // Relaciones
    [ForeignKey(nameof(IdUsuario))]
    public virtual Usuario? Usuario { get; set; }
}
