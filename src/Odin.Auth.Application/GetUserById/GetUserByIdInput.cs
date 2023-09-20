using MediatR;
using Odin.Auth.Domain.Models;

namespace Odin.Auth.Application.GetUserById
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
