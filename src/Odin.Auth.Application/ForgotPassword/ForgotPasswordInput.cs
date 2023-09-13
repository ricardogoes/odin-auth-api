using MediatR;

namespace Odin.Auth.Application.ForgotPassword
{
    public class ForgotPasswordInput : IRequest<ForgotPasswordOutput>
    {      

        public string Username { get; private set; }

        public ForgotPasswordInput(string username)
        {
            Username = username;
        }
    }
}
