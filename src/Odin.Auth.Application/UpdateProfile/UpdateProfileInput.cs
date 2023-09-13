using MediatR;

namespace Odin.Auth.Application.UpdateProfile
{
    public class UpdateProfileInput : IRequest<UpdateProfileOutput>
    {
        public string Username { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string EmailAddress { get; private set; }

        public UpdateProfileInput(string username, string firstName, string lastName, string emailAddress)
        {
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
        }
    }
}
