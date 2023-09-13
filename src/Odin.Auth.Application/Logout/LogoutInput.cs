using MediatR;

namespace Odin.Auth.Application.Logout
{
    public class LogoutInput : IRequest<LogoutOutput>
    {        
        public string Username { get; private set; }
        public string AccessToken { get; private set; }
                
        public LogoutInput(string username, string accessToken)
        {
            Username = username;
            AccessToken = accessToken;
        }
    }
}
