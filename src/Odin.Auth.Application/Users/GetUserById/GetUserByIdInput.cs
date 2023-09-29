using MediatR;
using Odin.Auth.Domain.SeedWork;

namespace Odin.Auth.Application.Users.GetUserById
{
    public class GetUserByIdInput : Tenant, IRequest<UserOutput>
    {
        public Guid UserId { get; private set; }

        public GetUserByIdInput(Guid tenantId, Guid userId)
            : base(tenantId)
        {
            UserId = userId;
        }
    }
}
