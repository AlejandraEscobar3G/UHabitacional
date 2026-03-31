using UHabitacionalAPI.Domain.Enums;

namespace UHabitacionalAPI.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException(string message, Exception? innerException = null)
            : base(message, innerException)
        {}
    }
}
