using Odin.Auth.Domain.SeedWork;
using Odin.Auth.Domain.Validations;
using System.Text.Json.Serialization;

namespace Odin.Auth.Domain.ValuesObjects
{
    public class UserGroup : ValueObject
    {
        public Guid? Id { get; private set; }
        public string Name { get; private set; }
        public string? Path { get; private set; }

        [JsonConstructor]
        public UserGroup(Guid? id, string name, string path)
        {
            Id = id ?? Guid.NewGuid();
            Name = name;
            Path = path;

            Validate();
        }

        public UserGroup(string name)
        {
            Name = name;
            Validate();
        }

        private void Validate()
        {
            DomainValidation.NotNullOrEmpty(Name, nameof(Name));
        }
    }
}
