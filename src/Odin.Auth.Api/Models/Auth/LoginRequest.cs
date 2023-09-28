namespace Odin.Auth.Api.Models.Auth
{
    public class LoginRequest
    {
        public string Username { get; private set; }
        public string Password { get; private set; }

        public LoginRequest(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
