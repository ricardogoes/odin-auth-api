using MediatR;

namespace Odin.Auth.Application.Login
{
    public class LoginInput : IRequest<LoginOutput>
    {
        public string Username { get; private set; }
        public string Password { get; private set; }

        public LoginInput(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
