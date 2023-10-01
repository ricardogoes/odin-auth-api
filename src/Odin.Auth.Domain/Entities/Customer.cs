using Odin.Auth.Domain.SeedWork;
using Odin.Auth.Domain.Validations;
using Odin.Auth.Domain.ValueObjects;

namespace Odin.Auth.Domain.Entities
{
    public class Customer : Entity
    {
        public string Name { get; private set; }
        public string Document { get; private set; }
        public Address? Address { get; private set; }
        public bool IsActive { get; private set; }

        public Customer(Guid id, string name, string document, bool isActive = true)
            : base(id)
        {
            Name = name;
            Document = document;
            IsActive = isActive;

            Validate();
        }

        public Customer(string name, string document, bool isActive = true)
            : base()
        {
            Name = name;
            Document = document;
            IsActive = isActive;

            Validate();
        }
               

        public void Update(string newName, string? newDocument)
        {
            Name = newName;
            Document = newDocument ?? Document;

            Validate();
        }        

        public void ChangeAddress(Address newAddress)
        {
            Address = newAddress;
            Validate();
        }

        public void Activate()
        {
            IsActive = true;
            Validate();
        }

        public void Deactivate()
        {
            IsActive = false;
            Validate();
        }

        private void Validate()
        {
            DomainValidation.NotNullOrEmpty(Name, nameof(Name));
            CpfCnpjValidation.CpfCnpj(Document, nameof(Document));
        }
    }
}
