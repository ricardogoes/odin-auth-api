namespace Odin.Auth.Application.Logout
{
    public class LogoutOutput
    {
        public string Username { get; private set; }
        public string? Message { get; private set; }
                
        public LogoutOutput(string username, string message)
        {
            Username = username;
            Message = message;
        }

        public LogoutOutput(string username)
        {
            Username = username;
        }
    }
}
