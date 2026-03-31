using UHabitacionalAPI.Domain.Enums;

namespace UHabitacionalAPI.Domain.Exceptions
{
    public class UhNotFoundException : DomainException
    {
        public UhNotFoundException(string message, Exception? innerException = null)
            : base(message, innerException)
        {}
    }
}
