using Amazon.CognitoIdentityProvider.Model;

namespace Odin.Auth.Domain.Interfaces.Cognito
{
    public interface IAmazonCognitoIdentityService
    {
        Task<AdminCreateUserResponse> AdminCreateUserAsync(AdminCreateUserRequest request);
        Task AdminUpdateUserAttributesAsync(AdminUpdateUserAttributesRequest request);
        Task<AdminInitiateAuthResponse> AdminInitiateAuthAsync(AdminInitiateAuthRequest request);
        Task AdminRespondToAuthChallengeAsync(AdminRespondToAuthChallengeRequest request);
        Task AdminEnableUserAsync(AdminEnableUserRequest request);
        Task AdminDisableUserAsync(AdminDisableUserRequest request);
        Task<AdminGetUserResponse> AdminGetUserAsync(AdminGetUserRequest request);

        Task<ChangePasswordResponse> ChangePasswordAsync(ChangePasswordRequest request);
        Task<ResendConfirmationCodeResponse> ResendConfirmationCodeAsync(ResendConfirmationCodeRequest request);
        Task<GlobalSignOutResponse> GlobalSignOutAsync(GlobalSignOutRequest request);
        Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request);
        Task<ConfirmForgotPasswordResponse> ConfirmForgotPasswordAsync(ConfirmForgotPasswordRequest request);
        Task<InitiateAuthResponse> InitiateAuthAsync(InitiateAuthRequest request);
        Task<ListUsersResponse> ListUsersAsync(ListUsersRequest request);

    }
}
