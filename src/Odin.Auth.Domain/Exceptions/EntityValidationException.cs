namespace Odin.Auth.Domain.Exceptions
{
    public class EntityValidationException : Exception
    {
        public IDictionary<string, string[]>? Errors { get; private set; }

        public EntityValidationException(string message, IDictionary<string, string[]>? errors = null)
            : base(message)
            => Errors = errors;
    }
}
