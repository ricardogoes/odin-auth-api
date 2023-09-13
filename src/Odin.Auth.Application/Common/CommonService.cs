using Amazon.CognitoIdentityProvider.Model;
using Odin.Auth.Infra.Cognito;

namespace Odin.Auth.Application.Common
{
    public class CommonService : ICommonService
    {
        private readonly AppSettings _appSettings;
        private readonly IAmazonCognitoIdentityRepository _awsIdentityRepository;

        public CommonService(AppSettings appSettings, IAmazonCognitoIdentityRepository awsIdentityRepository)
        {
            _appSettings = appSettings;
            _awsIdentityRepository = awsIdentityRepository;
        }

        public async Task<AuthenticationResultType> AuthenticateUserAsync(string username, string password, CancellationToken cancellationToken)
        {

            var authRequest = new InitiateAuthRequest()
            {
                AuthFlow = "USER_PASSWORD_AUTH",
                ClientId = _appSettings.AWSCognitoSettings.AppClientId,
                AuthParameters =
                {
                    { "USERNAME", username },
                    { "PASSWORD", password }
                }

            };

            var authResponse = await _awsIdentityRepository.InitiateAuthAsync(authRequest);
            var result = authResponse.AuthenticationResult;

            return result;
        }

        public async Task<UserProfileResponse> GetUserByUsernameAsync(string username, CancellationToken cancellationToken)
        {
            var userResponse = new AdminGetUserResponse();

            try
            {
                userResponse = await _awsIdentityRepository.AdminGetUserAsync(new AdminGetUserRequest
                {
                    Username = username,
                    UserPoolId = _appSettings.AWSCognitoSettings.UserPoolId
                });
            }
            catch (UserNotFoundException)
            {
                throw;
            }

            var attributes = userResponse.UserAttributes;

            return new UserProfileResponse
            (
                username,
                attributes.First(x => x.Name == "given_name").Value ?? string.Empty,
                attributes.First(x => x.Name == "family_name").Value ?? string.Empty,
                attributes.First(x => x.Name == "email").Value ?? string.Empty,
                attributes.First(x => x.Name == "preferred_username").Value ?? string.Empty
            );
        }

        public async Task<ListUsersResponse?> FindUsersByEmailAddressAsync(string emailAddress, CancellationToken cancellationToken)
        {
            var listUsersRequest = new ListUsersRequest
            {
                UserPoolId = _appSettings.AWSCognitoSettings.UserPoolId,
                Filter = $"email=\"{emailAddress}\""
            };

            return await _awsIdentityRepository.ListUsersAsync(listUsersRequest);
        }
    }
}
