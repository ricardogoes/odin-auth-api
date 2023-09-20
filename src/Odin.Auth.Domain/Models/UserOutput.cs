using Odin.Auth.Domain.Entities;
using Odin.Auth.Domain.ValuesObjects;

namespace Odin.Auth.Domain.Models
{
    public class UserOutput
    {
        public Guid Id { get; private set; }
        public string Username { get; private set; }
        public bool Enabled { get; private set; }
        public bool EmailVerified { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public Dictionary<string, string> Attributes { get; private set; }
        public List<UserCredential> Credentials { get; private set; }
        public List<UserGroup> Groups { get; set; }

        public UserOutput(Guid id, string username, bool enabled, bool emailVerified, string firstName, string lastName, string email,
            Dictionary<string, string> attributes, List<UserCredential> credentials, List<UserGroup> groups)
        {
            Id = id;
            Username = username;
            Enabled = enabled;
            EmailVerified = emailVerified;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Attributes = attributes;
            Credentials = credentials;
            Groups = groups;
        }

        public static UserOutput FromUser(User user)
        {
            return new UserOutput
            (
                user.Id,
                user.Username,
                user.Enabled,
                user.EmailVerified,
                user.FirstName,
                user.LastName,
                user.Email,
                user.Attributes,
                user.Credentials,
                user.Groups
            );
        }

        public static IEnumerable<UserOutput> FromUser(IEnumerable<User> users)
            => users.Select(FromUser);
    }
}
