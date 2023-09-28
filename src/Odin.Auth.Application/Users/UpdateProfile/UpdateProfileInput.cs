using MediatR;
using Odin.Auth.Domain.SeedWork;

namespace Odin.Auth.Application.Users.UpdateProfile
{
    public class UpdateProfileInput : Tenant, IRequest<UserOutput>
    {
        public Guid UserId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public List<string> Groups { get; private set; }

        public string LoggedUsername { get; private set; }

        public UpdateProfileInput(Guid tenantId, Guid userId, string firstName, string lastName, string email, List<string> groups, string loggedUsername)
            : base(tenantId)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Groups = groups;
            LoggedUsername = loggedUsername;
        }
    }
}
