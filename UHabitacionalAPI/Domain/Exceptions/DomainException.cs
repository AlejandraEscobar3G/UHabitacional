using UHabitacionalAPI.Domain.Enums;

namespace UHabitacionalAPI.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public DomainEntity Entity { get; }
        public DomainOperation Operation { get; }

        public DomainException(DomainEntity entity, DomainOperation operation, string message, Exception? innerException = null)
            : base(message, innerException)
        {
            Entity = entity;
            Operation = operation;
        }
    }
}
