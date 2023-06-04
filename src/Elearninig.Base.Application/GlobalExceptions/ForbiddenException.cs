namespace Elearninig.Base.Application.GlobalExceptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException() :base() { }
        public ForbiddenException(string msg) :base(msg) { }

        public ForbiddenException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ForbiddenException(string name, object key)
            : base($"Entity \"{name}\" ({key}) was not found.")
        {
        }

    }
}
