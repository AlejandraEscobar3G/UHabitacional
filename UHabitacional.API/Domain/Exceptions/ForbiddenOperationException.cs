namespace UHabitacional.API.Domain.Exceptions;

/// <summary>
/// Excepción lanzada cuando un usuario no tiene los permisos necesarios
/// para realizar una operación. Se mapea a HTTP 403.
/// </summary>
public class ForbiddenOperationException : Exception
{
    public ForbiddenOperationException(string message) : base(message) { }
}
