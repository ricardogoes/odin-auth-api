using MediatR;

namespace Odin.Auth.Application.Login
{
    public class LoginInput : IRequest<LoginOutput>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
