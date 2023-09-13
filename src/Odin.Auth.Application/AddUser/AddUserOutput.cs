namespace Odin.Auth.Application.AddUser
{
    public class AddUserOutput
    {
        public string Username { get; private set; }
        public string EmailAddress { get; private set; }

        public AddUserOutput(string username, string emailAddress)
        {
            Username = username;
            EmailAddress = emailAddress;
        }
    }
}
