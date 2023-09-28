using Odin.Auth.Domain.Entities;
using Odin.Auth.Domain.ValuesObjects;

namespace Odin.Auth.Application.Users
{
    public class UserOutput
    {
        public Guid Id { get; private set; }
        public string Username { get; private set; }
        public bool IsActive { get; private set; }
        public bool EmailVerified { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public List<UserGroup> Groups { get; set; }
        public DateTime CreatedAt { get; private set; }
        public string CreatedBy { get; private set; }
        public DateTime LastUpdatedAt { get; private set; }
        public string LastUpdatedBy { get; private set; }

        public UserOutput(Guid id, string username, bool isActive, bool emailVerified, string firstName, string lastName, string email,
            List<UserGroup> groups, DateTime createdAt, string createdBy, DateTime lastUpdatedAt, string lastUpdatedBy)
        {
            Id = id;
            Username = username;
            IsActive = isActive;
            EmailVerified = emailVerified;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Groups = groups;
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            LastUpdatedAt = lastUpdatedAt;
            LastUpdatedBy = lastUpdatedBy;
        }

        public static UserOutput FromUser(User user)
        {
            return new UserOutput
            (
                user.Id,
                user.Username,
                user.IsActive,
                user.EmailVerified,
                user.FirstName,
                user.LastName,
                user.Email,
                user.Groups,
                user.CreatedAt ?? default,
                user.CreatedBy ?? "",
                user.LastUpdatedAt ?? default,
                user.LastUpdatedBy ?? ""
            );
        }

        public static IEnumerable<UserOutput> FromUser(IEnumerable<User> users)
            => users.Select(FromUser);
    }
}
