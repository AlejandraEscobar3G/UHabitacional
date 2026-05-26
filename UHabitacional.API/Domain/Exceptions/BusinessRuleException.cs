namespace UHabitacional.API.Domain.Exceptions;

/// <summary>
/// Excepción lanzada cuando se viola una regla de negocio.
/// Se mapea a HTTP 400 / 409.
/// </summary>
public class BusinessRuleException : Exception
{
    public BusinessRuleException(string message) : base(message) { }
}
