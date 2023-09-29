using MediatR;

namespace Odin.Auth.Application.Customers.GetCustomerById
{
    public class GetCustomerByIdInput : IRequest<CustomerOutput>
    {
        public Guid Id { get; set; }
    }
}
