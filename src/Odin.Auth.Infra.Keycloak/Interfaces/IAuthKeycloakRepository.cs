using Odin.Auth.Domain.Entities;
using Odin.Auth.Infra.Keycloak.Models;

namespace Odin.Auth.Infra.Keycloak.Interfaces
{
    public interface IAuthKeycloakRepository
    {        
        Task<KeycloakAuthResponse> AuthAsync(string username, string password, CancellationToken cancellationToken);
        Task ChangePasswordAsync(User user, CancellationToken cancellationToken);
        Task LogoutAsync(Guid userId, CancellationToken cancellationToken);
    }
}
