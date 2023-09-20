using MediatR;

namespace Odin.Auth.Application.Logout
{
    public class LogoutInput : IRequest
    {
        public Guid UserId { get; set; }

        public LogoutInput(Guid userId)
        {
            UserId = userId;
        }
    }
}
