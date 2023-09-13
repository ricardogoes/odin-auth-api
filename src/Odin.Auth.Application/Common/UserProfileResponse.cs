namespace Odin.Auth.Application.Common
{
    public class UserProfileResponse
    {        

        public string Username { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string EmailAddress { get; private set; }
        public string PreferredUsername { get; private set; }

        public UserProfileResponse(string username, string firstName, string lastName, string emailAddress, string preferredUsername)
        {
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
            PreferredUsername = preferredUsername;
        }
    }
}
