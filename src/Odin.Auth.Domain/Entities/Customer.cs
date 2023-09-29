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

        public void Create(string loggedUsername)
        {
            CreatedAt = DateTime.UtcNow;
            CreatedBy = loggedUsername;
            LastUpdatedAt = DateTime.UtcNow;
            LastUpdatedBy = loggedUsername;

            Validate();
        }

        public void Update(string newName, string? newDocument, string loggedUsername)
        {
            Name = newName;
            Document = newDocument ?? Document;
            LastUpdatedAt = DateTime.UtcNow;
            LastUpdatedBy = loggedUsername;

            Validate();
        }

        public void SetAuditLog(DateTime createdAt, string createdBy, DateTime lastUpdatedAt, string lastUpdatedBy)
        {
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            LastUpdatedAt = lastUpdatedAt;
            LastUpdatedBy = lastUpdatedBy;

            Validate();
        }

        public void ChangeAddress(Address newAddress, string loggedUsername)
        {
            Address = newAddress;

            LastUpdatedAt = DateTime.UtcNow;
            LastUpdatedBy = loggedUsername;

            Validate();
        }

        public void Activate(string loggedUsername)
        {
            IsActive = true;
            LastUpdatedAt = DateTime.UtcNow;
            LastUpdatedBy = loggedUsername;

            Validate();
        }

        public void Deactivate(string loggedUsername)
        {
            IsActive = false;
            LastUpdatedAt = DateTime.UtcNow;
            LastUpdatedBy = loggedUsername;

            Validate();
        }

        private void Validate()
        {
            DomainValidation.NotNullOrEmpty(Name, nameof(Name));
            CpfCnpjValidation.CpfCnpj(Document, nameof(Document));
        }
    }
}
