namespace UHabitacional.API.Domain.Exceptions;

/// <summary>
/// Excepción lanzada cuando no se encuentra una entidad en la base de datos.
/// Se mapea a HTTP 404.
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }

    public NotFoundException(string entityName, object key)
        : base($"No se encontró la entidad '{entityName}' con identificador '{key}'.") { }
}
