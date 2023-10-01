using Odin.Auth.Domain.Entities;
using Odin.Auth.Domain.ValuesObjects;

namespace Odin.Auth.Infra.Keycloak.Interfaces
{
    public interface IUserKeycloakRepository
    {
        Task<User> CreateUserAsync(User user, CancellationToken cancellationToken);
        Task<User> UpdateUserAsync(User user, CancellationToken cancellationToken);

        Task<User> FindByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<User>> FindUsersAsync(CancellationToken cancellationToken);
        Task<IEnumerable<UserGroup>> FindGroupsByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    }
}
