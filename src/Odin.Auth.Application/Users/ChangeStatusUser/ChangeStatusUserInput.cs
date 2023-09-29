using MediatR;
using Odin.Auth.Domain.Enums;
using Odin.Auth.Domain.SeedWork;

namespace Odin.Auth.Application.Users.ChangeStatusUser
{
    public class ChangeStatusUserInput : Tenant, IRequest<UserOutput>
    {
        public Guid UserId { get; private set; }
        public ChangeStatusAction? Action { get; private set; }
        public string LoggedUsername { get; private set; }

        public ChangeStatusUserInput(Guid tenantId, Guid id, ChangeStatusAction? action, string loggedUsername)
            : base(tenantId)
        {
            UserId = id;
            Action = action;
            LoggedUsername = loggedUsername;
        }
    }
}
