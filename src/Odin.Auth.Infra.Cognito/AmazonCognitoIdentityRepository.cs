using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Odin.Auth.Infra.Cognito.Models;

namespace Odin.Auth.Infra.Cognito
{
    public class AmazonCognitoIdentityRepository : IAmazonCognitoIdentityRepository
    {
        private readonly AmazonCognitoIdentityProviderClient _awsIdentityProvider;

        public AmazonCognitoIdentityRepository(CognitoSettings cognitoSettings)
        {
            _awsIdentityProvider = new AmazonCognitoIdentityProviderClient(
                cognitoSettings.AccessKeyId,
                cognitoSettings.AccessSecretKey, 
                RegionEndpoint.GetBySystemName(cognitoSettings.Region)
            );
        }

        public async Task<AdminCreateUserResponse> AdminCreateUserAsync(AdminCreateUserRequest request)
            => await _awsIdentityProvider.AdminCreateUserAsync(request);

        public async Task AdminDisableUserAsync(AdminDisableUserRequest request)
            => await _awsIdentityProvider.AdminDisableUserAsync(request);

        public async Task AdminEnableUserAsync(AdminEnableUserRequest request)
            => await _awsIdentityProvider.AdminEnableUserAsync(request);

        public async Task<AdminGetUserResponse> AdminGetUserAsync(AdminGetUserRequest request)
            => await _awsIdentityProvider.AdminGetUserAsync(request);

        public async Task<AdminInitiateAuthResponse> AdminInitiateAuthAsync(AdminInitiateAuthRequest request)
            => await _awsIdentityProvider.AdminInitiateAuthAsync(request);

        public async Task AdminRespondToAuthChallengeAsync(AdminRespondToAuthChallengeRequest request)
            => await _awsIdentityProvider.AdminRespondToAuthChallengeAsync(request);

        public async Task AdminUpdateUserAttributesAsync(AdminUpdateUserAttributesRequest request)
            => await _awsIdentityProvider.AdminUpdateUserAttributesAsync(request);

        public async Task<ChangePasswordResponse> ChangePasswordAsync(ChangePasswordRequest request)
            => await _awsIdentityProvider.ChangePasswordAsync(request);

        public async Task<ConfirmForgotPasswordResponse> ConfirmForgotPasswordAsync(ConfirmForgotPasswordRequest request)
            => await _awsIdentityProvider.ConfirmForgotPasswordAsync(request);

        public async Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request)
            => await _awsIdentityProvider.ForgotPasswordAsync(request);

        public async Task<GlobalSignOutResponse> GlobalSignOutAsync(GlobalSignOutRequest request)
            => await _awsIdentityProvider.GlobalSignOutAsync(request);

        public async Task<InitiateAuthResponse> InitiateAuthAsync(InitiateAuthRequest request)
            => await _awsIdentityProvider.InitiateAuthAsync(request);

        public async Task<ListUsersResponse?> ListUsersAsync(ListUsersRequest request)
            => await _awsIdentityProvider.ListUsersAsync(request);

        public async Task<ResendConfirmationCodeResponse> ResendConfirmationCodeAsync(ResendConfirmationCodeRequest request)
            => await _awsIdentityProvider.ResendConfirmationCodeAsync(request);
    }
}
