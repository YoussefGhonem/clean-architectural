namespace Elearninig.Base.Application.GlobalExceptions
{
    public class KeyNotFoundException : Exception
    {
        public KeyNotFoundException() :base() { }
        public KeyNotFoundException(string msg) :base(msg) { }

        public KeyNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public KeyNotFoundException(string name, object key)
            : base($"Entity \"{name}\" ({key}) was not found.")
        {
        }

    }
}
