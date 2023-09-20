using Odin.Auth.Domain.Entities;
using Odin.Auth.Domain.Models;
using Odin.Auth.Domain.ValuesObjects;

namespace Odin.Auth.Domain.Interfaces
{
    public interface IKeycloakRepository
    {   
        Task CreateUserAsync(User user, CancellationToken cancellationToken);
        Task UpdateUserAsync(User user, CancellationToken cancellationToken);

        Task<User> FindByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<User>> FindUsersAsync(CancellationToken cancellationToken);
        Task<IEnumerable<UserGroup>> FindGroupsByUserIdAsync(Guid userId, CancellationToken cancellationToken);

        Task<KeycloakAuthResponse> AuthAsync(string username, string password, CancellationToken cancellationToken);
        Task ChangePasswordAsync(User user, CancellationToken cancellationToken);
        Task LogoutAsync(Guid userId, CancellationToken cancellationToken);
    }
}
