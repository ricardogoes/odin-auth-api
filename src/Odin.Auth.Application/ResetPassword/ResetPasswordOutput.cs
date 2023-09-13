namespace Odin.Auth.Application.ResetPassword
{
    public class ResetPasswordOutput
    {
        public string Username { get; private set; }

        public ResetPasswordOutput(string username)
        {
            Username = username;
        }        
    }
}
