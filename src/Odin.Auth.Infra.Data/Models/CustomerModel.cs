namespace Odin.Auth.Infra.Data.EF.Models
{
    public class CustomerModel : BaseModel
    {
        public string Name { get; private set; }
        public string Document { get; private set; }
        public string? StreetName { get; private set; }
        public int? StreetNumber { get; private set; }
        public string? Complement { get; private set; }
        public string? Neighborhood { get; private set; }
        public string? ZipCode { get; private set; }
        public string? City { get; private set; }
        public string? State { get; private set; }
        public bool IsActive { get; private set; }
        
        public CustomerModel(Guid id, string name, string document, string? streetName, int? streetNumber, string? complement, string? neighborhood, string? zipCode, string? city, string? state,
            bool isActive, DateTime createdAt, string createdBy, DateTime lastUpdatedAt, string lastUpdatedBy)
            : base(id, createdAt, createdBy, lastUpdatedAt, lastUpdatedBy)
        {
            Name = name;
            Document = document;
            StreetName = streetName;
            StreetNumber = streetNumber;
            Complement = complement;
            Neighborhood = neighborhood;
            ZipCode = zipCode;
            City = city;
            State = state;
            IsActive = isActive;
        }

        public CustomerModel(Guid id, string name, string document, bool isActive,
            DateTime createdAt, string createdBy, DateTime lastUpdatedAt, string lastUpdatedBy)
            : base(id, createdAt, createdBy, lastUpdatedAt, lastUpdatedBy)
        {
            Id = id;
            Name = name;
            Document = document;
            IsActive = isActive;
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            LastUpdatedAt = lastUpdatedAt;
            LastUpdatedBy = lastUpdatedBy;
        }

        public void ChangeIsActive(bool isActive)
        {
            IsActive = isActive;
        }
    }
}
