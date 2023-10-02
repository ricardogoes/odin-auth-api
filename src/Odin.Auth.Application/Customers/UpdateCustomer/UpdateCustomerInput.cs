using MediatR;

namespace Odin.Auth.Application.Customers.UpdateCustomer
{
    public class UpdateCustomerInput : IRequest<CustomerOutput>
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Document { get; private set; }

        public UpdateCustomerInput(Guid id, string name, string document)
        {
            Id = id;
            Name = name;
            Document = document;
        }

        public void ChangeId(Guid id)
        {
            Id = id;
        }

        public void ChangeDocument(string document)
        {
            Document = document;
        }
    }
}
