namespace Odin.Auth.Application.UpdateProfile
{
    public class UpdateProfileOutput
    {        
        public string Username { get; private set; }

        public UpdateProfileOutput(string username)
        {
            Username = username;
        }
    }
}
