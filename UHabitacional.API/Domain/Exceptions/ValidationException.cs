namespace UHabitacional.API.Domain.Exceptions;

/// <summary>
/// Excepción lanzada cuando los datos no cumplen con la validación de modelo.
/// Se mapea a HTTP 400.
/// </summary>
public class ValidationException : Exception
{
    public IDictionary<string, string[]> Errors { get; }

    public ValidationException(string message) : base(message)
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(IDictionary<string, string[]> errors)
        : base("Se han producido uno o más errores de validación.")
    {
        Errors = errors;
    }
}
