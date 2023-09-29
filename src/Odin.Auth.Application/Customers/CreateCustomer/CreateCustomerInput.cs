using MediatR;
using Odin.Auth.Domain.ValueObjects;

namespace Odin.Auth.Application.Customers.CreateCustomer
{
    public class CreateCustomerInput : IRequest<CustomerOutput>
    {
        public string Name { get; private set; }
        public string Document { get; private set; }
        public Address? Address { get; private set; }
        public string LoggedUsername { get; private set; }

        public CreateCustomerInput(string name, string document, Address? address, string loggedUsername)
        {
            Name = name;
            Document = document;
            Address = address;
            LoggedUsername = loggedUsername;
        }

        public void ChangeDocument(string document)
        {
            Document = document;
        }

        public void ChangeLoggedUsername(string username)
        {
            LoggedUsername = username;
        }
    }
}
