using MediatR;
using Odin.Auth.Domain.SeedWork;

namespace Odin.Auth.Application.Auth.ChangePassword
{
    public class ChangePasswordInput : Tenant, IRequest
    {
        public Guid UserId { get; private set; }
        public string NewPassword { get; private set; }
        public bool Temporary { get; private set; }

        public ChangePasswordInput(Guid tenantId, Guid userId, string newPassword, bool temporary)
            : base(tenantId)
        {
            UserId = userId;
            NewPassword = newPassword;
            Temporary = temporary;
        }
    }
}
