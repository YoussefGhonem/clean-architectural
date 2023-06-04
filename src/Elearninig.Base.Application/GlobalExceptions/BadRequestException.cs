namespace Elearninig.Base.Application.GlobalExceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException() :base() { }
        public BadRequestException(string msg) :base(msg) { }

        public BadRequestException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public BadRequestException(string name, object key)
            : base($"Entity \"{name}\" ({key}) was not found.")
        {
        }

    }
}
