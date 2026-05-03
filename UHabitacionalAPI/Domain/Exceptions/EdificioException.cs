namespace UHabitacionalAPI.Domain.Exceptions
{
    public class EdificioException : Exception
    {
        public EdificioException() { }

        public EdificioException(string message) : base(message) { }

        public EdificioException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
