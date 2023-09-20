namespace Odin.Auth.Api.Models
{
    public class UpdateProfileApiRequest
    {
        public Guid UserId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public List<string> Groups { get; private set; }

        public UpdateProfileApiRequest(Guid userId, string firstName, string lastName, string email, List<string> groups)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Groups = groups;
        }

        public void ChangeUserId(Guid userId)
        {
            UserId = userId;
        }
    }
}

