using Odin.Auth.Domain.SeedWork;
using Odin.Auth.Domain.Validations;

namespace Odin.Auth.Domain.ValuesObjects
{
    public class UserCredential : ValueObject
    {
        public string Type { get; private set; }
        public string Value { get; private set; }
        public bool Temporary { get; private set; }

        public UserCredential(string value, bool temporary)
        {
            Type = "password";
            Value = value;
            Temporary = temporary;

            Validate();
        }

        private void Validate()
        {
            DomainValidation.NotNullOrEmpty(Value, nameof(Value));
            DomainValidation.NotNullOrEmpty(Temporary, nameof(Temporary));
        }
    }
}
