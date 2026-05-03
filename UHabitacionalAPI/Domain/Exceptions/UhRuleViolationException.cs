namespace UHabitacionalAPI.Domain.Exceptions
{
    public class UhRuleViolationException : DomainException
    {
        public UhRuleViolationException(string message, Exception? innerException = null)
            : base(message, innerException)
        {}
    }
}
