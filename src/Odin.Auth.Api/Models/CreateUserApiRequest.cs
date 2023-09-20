namespace Odin.Auth.Api.Models
{
    public class CreateUserApiRequest
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public bool PasswordIsTemporary { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public List<string> Groups { get; private set; }

        public CreateUserApiRequest(string username, string password, bool passwordIsTemporary, string firstName, string lastName, string email, List<string> groups)
        {
            Username = username;
            Password = password;
            PasswordIsTemporary = passwordIsTemporary;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Groups = groups;
        }
    }
}
