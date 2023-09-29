using Odin.Auth.Domain.SeedWork;
using Odin.Auth.Domain.Validations;
using System.Text.Json.Serialization;

namespace Odin.Auth.Domain.ValueObjects
{
    public class Address : ValueObject
    {

        public string? StreetName { get; private set; }
        public int? StreetNumber { get; private set; }
        public string? Complement { get; private set; }
        public string? Neighborhood { get; private set; }
        public string? ZipCode { get; private set; }
        public string? City { get; private set; }
        public string? State { get; private set; }

        public Address()
        { }

        [JsonConstructor]
        public Address(string streetName, int? streetNumber, string? complement, string neighborhood, string zipCode, string city, string state)
        {
            StreetName = streetName;
            StreetNumber = streetNumber;
            Complement = complement;
            Neighborhood = neighborhood;
            ZipCode = zipCode;
            City = city;
            State = state;

            Validate();
        }

        public void Validate()
        {
            DomainValidation.NotNullOrEmpty(StreetName, nameof(StreetName));
            DomainValidation.NotNullOrEmpty(StreetNumber, nameof(StreetNumber));
            DomainValidation.NotNullOrEmpty(Neighborhood, nameof(Neighborhood));
            DomainValidation.NotNullOrEmpty(ZipCode, nameof(ZipCode));
            DomainValidation.NotNullOrEmpty(City, nameof(City));
            DomainValidation.NotNullOrEmpty(State, nameof(State));
        }
    }
}
