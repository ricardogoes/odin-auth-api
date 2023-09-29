using Odin.Auth.Domain.ValueObjects;

namespace Odin.Baseline.Api.Models.Customers
{
    public class CreateCustomerApiRequest
    {
        public string Name { get; private set; }
        public string Document { get; private set; }

        public Address? Address { get; private set; }

        public CreateCustomerApiRequest(string name, string document, Address? address)
        {
            Name = name;
            Document = document;
            Address = address;
        }
    }
}
