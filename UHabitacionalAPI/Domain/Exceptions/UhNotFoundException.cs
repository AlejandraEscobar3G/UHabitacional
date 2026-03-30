using UHabitacionalAPI.Domain.Enums;

namespace UHabitacionalAPI.Domain.Exceptions
{
    public class UhNotFoundException : DomainException
    {
        public UhNotFoundException(DomainEntity entity, string message, Exception? innerException = null)
            : base(entity, DomainOperation.READ, message, innerException)
        {}
    }
}
