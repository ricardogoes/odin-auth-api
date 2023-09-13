namespace Odin.Auth.Application.ChangePassword
{
    public class ChangePasswordOutput
    {        
        public string Username { get; private set; }

        public ChangePasswordOutput(string username)
        {
            Username = username;
        }
    }
}
