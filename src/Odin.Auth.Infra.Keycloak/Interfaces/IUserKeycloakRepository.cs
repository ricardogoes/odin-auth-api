using Odin.Auth.Domain.Entities;
using Odin.Auth.Domain.ValuesObjects;

namespace Odin.Auth.Infra.Keycloak.Interfaces
{
    public interface IUserKeycloakRepository
    {
        Task CreateUserAsync(User user, CancellationToken cancellationToken);
        Task UpdateUserAsync(User user, CancellationToken cancellationToken);

        Task<User> FindByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<User>> FindUsersAsync(Guid tenantId, CancellationToken cancellationToken);
        Task<IEnumerable<UserGroup>> FindGroupsByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    }
}
