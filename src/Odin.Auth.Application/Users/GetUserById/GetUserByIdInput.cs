using MediatR;
using Odin.Auth.Domain.SeedWork;

namespace Odin.Auth.Application.Users.GetUserById
{
    public class GetUserByIdInput : IRequest<UserOutput>
    {
        public Guid UserId { get; private set; }

        public GetUserByIdInput(Guid userId)
        {
            UserId = userId;
        }
    }
}
