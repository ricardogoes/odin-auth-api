namespace Odin.Auth.Domain.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException()
            : base()
        {
        }

        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public static void ThrowIfNull(object? @object, string exceptionMessage)
        {
            if (@object == null)
                throw new NotFoundException(exceptionMessage);
        }
    }
}
