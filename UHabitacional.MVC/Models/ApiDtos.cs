using System.ComponentModel.DataAnnotations;

namespace UHabitacional.MVC.Models;

// =====================================================================
// Auth
// =====================================================================
public class LoginDto
{
    [Required(ErrorMessage = "El correo es obligatorio")]
    [EmailAddress(ErrorMessage = "Formato de correo inválido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}

public class LoginResponseDto
{
    public int IdUsuario { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string TipoUsuario { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public DateTime Expira { get; set; }
}

// =====================================================================
// Identificación
// =====================================================================
public class IdentificacionDto
{
    public int IdIdentificacion { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool Activo { get; set; }
}

public class IdentificacionCreateDto
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(250)]
    public string? Descripcion { get; set; }
}

// =====================================================================
// Tipo de Usuario (Perfiles)
// =====================================================================
public class TipoUsuarioDto
{
    public int IdTipoUsuario { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool Activo { get; set; }
}

public class TipoUsuarioCreateDto
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [MaxLength(50)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(250)]
    public string? Descripcion { get; set; }
}

// =====================================================================
// Edificio
// =====================================================================
public class EdificioDto
{
    public int IdEdificio { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public int NumeroPisos { get; set; }
    public int TotalDeptos { get; set; }
}

public class EdificioCreateDto
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(250)]
    public string? Descripcion { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe tener al menos 1 piso")]
    public int NumeroPisos { get; set; } = 1;

    [Range(1, int.MaxValue, ErrorMessage = "Debe tener al menos 1 depto")]
    public int TotalDeptos { get; set; } = 1;
}

// =====================================================================
// Departamento
// =====================================================================
public class DepartamentoDto
{
    public int IdDepartamento { get; set; }
    public int IdEdificio { get; set; }
    public string? NombreEdificio { get; set; }
    public string NumeroDepartamento { get; set; } = string.Empty;
    public int Piso { get; set; }
}

public class DepartamentoCreateDto
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona un edificio")]
    public int IdEdificio { get; set; }

    [Required(ErrorMessage = "El número de departamento es obligatorio")]
    [MaxLength(20)]
    public string NumeroDepartamento { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "El piso debe ser mayor a 0")]
    public int Piso { get; set; } = 1;
}

// =====================================================================
// Usuario (incluye vigilantes, admins, inquilinos)
// =====================================================================
public class UsuarioDto
{
    public int IdUsuario { get; set; }
    public int IdTipoUsuario { get; set; }
    public string? TipoUsuario { get; set; }
    public int IdIdentificacion { get; set; }
    public string? Identificacion { get; set; }
    public string NumeroIdentificacion { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public bool Activo { get; set; }
}

public class UsuarioCreateDto
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona un tipo de usuario")]
    public int IdTipoUsuario { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona una identificación")]
    public int IdIdentificacion { get; set; }

    [Required(ErrorMessage = "El número de identificación es obligatorio")]
    [MaxLength(50)]
    public string NumeroIdentificacion { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "Los apellidos son obligatorios")]
    [MaxLength(150)]
    public string Apellidos { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo es obligatorio")]
    [EmailAddress]
    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    [MinLength(6, ErrorMessage = "Mínimo 6 caracteres")]
    [MaxLength(100)]
    public string Password { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? Telefono { get; set; }
}

public class UsuarioUpdateDto
{
    [Required]
    public int IdTipoUsuario { get; set; }

    [Required]
    public int IdIdentificacion { get; set; }

    [Required, MaxLength(50)]
    public string NumeroIdentificacion { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required, MaxLength(150)]
    public string Apellidos { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Password { get; set; }

    [MaxLength(20)]
    public string? Telefono { get; set; }
}

// =====================================================================
// Inquilino
// =====================================================================
public class InquilinoDto
{
    public int IdInquilino { get; set; }
    public int IdUsuario { get; set; }
    public string? NombreUsuario { get; set; }
    public string? EmailUsuario { get; set; }
    public int IdDepartamento { get; set; }
    public string? NumeroDepartamento { get; set; }
    public string? NombreEdificio { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public bool EstaActivo { get; set; }
}

public class InquilinoCreateDto
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona un usuario")]
    public int IdUsuario { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona un departamento")]
    public int IdDepartamento { get; set; }

    public DateTime? FechaInicio { get; set; }
}

public class InquilinoUpdateDto
{
    [Required]
    [Range(1, int.MaxValue)]
    public int IdDepartamento { get; set; }

    public DateTime? FechaInicio { get; set; }
}

public class InquilinoFechaFinDto
{
    public DateTime? FechaFin { get; set; }
}

// =====================================================================
// Bitácora del Vigilante (checador)
// =====================================================================
public class BitacoraVigilanteDto
{
    public int IdBitacoraVigilante { get; set; }
    public int IdUsuario { get; set; }
    public string? NombreVigilante { get; set; }
    public DateTime FechaHoraEntrada { get; set; }
    public DateTime? FechaHoraSalida { get; set; }
    public string? Observaciones { get; set; }
}

public class BitacoraVigilanteCreateDto
{
    public DateTime? FechaHoraEntrada { get; set; }
    [MaxLength(500)] public string? Observaciones { get; set; }
}

public class BitacoraVigilanteUpdateDto
{
    public DateTime? FechaHoraSalida { get; set; }
    [MaxLength(500)] public string? Observaciones { get; set; }
}

// =====================================================================
// Bitácora del Visitante
// =====================================================================
public class BitacoraVisitanteDto
{
    public int IdBitacoraVisitante { get; set; }
    public int IdInquilino { get; set; }
    public string? NombreInquilino { get; set; }
    public string NombreVisitante { get; set; } = string.Empty;
    public int IdIdentificacion { get; set; }
    public string? Identificacion { get; set; }
    public string NumeroIdentificacion { get; set; } = string.Empty;
    public string CodigoVisita { get; set; } = string.Empty;
    public DateTime? FechaHoraLlegada { get; set; }
    public DateTime? FechaHoraSalida { get; set; }
    public string? Observaciones { get; set; }
    public DateTime FechaCreacion { get; set; }
}

public class BitacoraVisitanteCreateDto
{
    [Required(ErrorMessage = "El nombre del visitante es obligatorio")]
    [MaxLength(150)]
    public string NombreVisitante { get; set; } = string.Empty;

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona el tipo de identificación")]
    public int IdIdentificacion { get; set; }

    [Required(ErrorMessage = "El número de identificación es obligatorio")]
    [MaxLength(50)]
    public string NumeroIdentificacion { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Observaciones { get; set; }
}

public class BitacoraVisitanteUpdateDto
{
    [Required, MaxLength(150)] public string NombreVisitante { get; set; } = string.Empty;
    [Required] public int IdIdentificacion { get; set; }
    [Required, MaxLength(50)] public string NumeroIdentificacion { get; set; } = string.Empty;
    [MaxLength(500)] public string? Observaciones { get; set; }
}

public class BitacoraVisitanteRegistroDto
{
    public bool EsLlegada { get; set; }
}

// =====================================================================
// API problem details (errores)
// =====================================================================
public class ApiErrorDto
{
    public string? Title { get; set; }
    public string? Detail { get; set; }
    public int? Status { get; set; }
    public Dictionary<string, string[]>? Errors { get; set; }
}
