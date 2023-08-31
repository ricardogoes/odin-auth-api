using Amazon.CognitoIdentityProvider.Model;

namespace Odin.Auth.Application.Common
{
    public interface ICommonService
    {
        Task<AuthenticationResultType> AuthenticateUserAsync(string username, string password, CancellationToken cancellationToken);
        Task<UserProfileResponse> GetUserByUsernameAsync(string username, CancellationToken cancellationToken);
        Task<ListUsersResponse> FindUsersByEmailAddressAsync(string emailAddress, CancellationToken cancellationToken);
    }
}
