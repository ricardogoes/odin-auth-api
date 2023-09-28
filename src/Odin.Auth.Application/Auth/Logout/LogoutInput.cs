using MediatR;

namespace Odin.Auth.Application.Auth.Logout
{
    public class LogoutInput : IRequest
    {
        public Guid UserId { get; private set; }

        public LogoutInput(Guid userId)
        {
            UserId = userId;
        }
    }
}
