namespace Elearninig.Base.Application.GlobalExceptions
{
    public class NotImplementedException : Exception
    {
        public NotImplementedException() :base() { }
        public NotImplementedException(string msg) :base(msg) { }

        public NotImplementedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public NotImplementedException(string name, object key)
            : base($"Entity \"{name}\" ({key}) was not found.")
        {
        }

    }
}
