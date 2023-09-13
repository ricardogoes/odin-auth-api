using MediatR;

namespace Odin.Auth.Application.AddUser
{
    public class AddUserInput : IRequest<AddUserOutput>
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string EmailAddress { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }

        public AddUserInput(string firstName, string lastName, string emailAddress, string username, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
            Username = username;
            Password = password;
        }
    }
}
