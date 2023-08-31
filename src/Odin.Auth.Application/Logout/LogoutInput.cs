using MediatR;

namespace Odin.Auth.Application.Logout
{
    public class LogoutInput : IRequest<LogoutOutput>
    {
        public string Username { get; set; }
        public string AccessToken { get; set; }
    }
}
