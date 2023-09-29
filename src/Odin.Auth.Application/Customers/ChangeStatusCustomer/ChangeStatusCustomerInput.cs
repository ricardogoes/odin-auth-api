using MediatR;
using Odin.Auth.Domain.Enums;

namespace Odin.Auth.Application.Customers.ChangeStatusCustomer
{
    public class ChangeStatusCustomerInput : IRequest<CustomerOutput>
    {
        public Guid Id { get; private set; }
        public ChangeStatusAction? Action { get; private set; }
        public string LoggedUsername { get; private set; }

        public ChangeStatusCustomerInput(Guid id, ChangeStatusAction? action, string loggedUsername)
        {
            Id = id;
            Action = action;
            LoggedUsername = loggedUsername;
        }
    }
}
