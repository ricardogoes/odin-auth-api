using MediatR;

namespace Odin.Auth.Application.Customers.ChangeAddressCustomer
{
    public class ChangeAddressCustomerInput : IRequest<CustomerOutput>
    {
        public Guid CustomerId { get; private set; }
        public string StreetName { get; private set; }
        public int StreetNumber { get; private set; }
        public string? Complement { get; private set; }
        public string Neighborhood { get; private set; }
        public string ZipCode { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }

        public string LoggedUsername { get; private set; }

        public ChangeAddressCustomerInput(Guid customerId, string streetName, int streetNumber, string neighborhood,
            string zipCode, string city, string state, string loggedUsername, string? complement = null)
        {
            CustomerId = customerId;
            StreetName = streetName;
            StreetNumber = streetNumber;
            Complement = complement;
            Neighborhood = neighborhood;
            ZipCode = zipCode;
            City = city;
            State = state;

            LoggedUsername = loggedUsername;
        }

        public void ChangeCustomerId(Guid customerId)
        {
            CustomerId = customerId;
        }
    }
}
