namespace Odin.Auth.Application.ChangeStatusUser
{
    public class ChangeStatusUserOutput
    {
        public string Username { get; private set; }

        public ChangeStatusUserOutput(string username)
        {
            Username = username;
        }
    }
}
