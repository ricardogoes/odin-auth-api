namespace Odin.Auth.Application.ForgotPassword
{
    public class ForgotPasswordOutput
    {
        public string? UserId { get; private set; }
        public string? Username { get; private set; }
        public string Message { get; private set; }

        public ForgotPasswordOutput(string userId, string username, string message)
        {
            UserId = userId;
            Username = username;
            Message = message;
        }

        public ForgotPasswordOutput(string message)
        {
            Message = message;
        }
    }
}
