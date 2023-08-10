using Amazon.CognitoIdentityProvider.Model;
using Odin.Auth.Domain.Interfaces.Cognito;
using Odin.Auth.Domain.Interfaces.Services;
using Odin.Auth.Domain.Models;
using Odin.Auth.Domain.Models.ResetPassword;
using Odin.Auth.Domain.Models.UserLogin;
using System.Net;

namespace Odin.Auth.Service.ExternalServices
{
    public class CognitoAuthService : ICognitoAuthService
    {
        private readonly AppSettings _appSettings;
        private readonly IAmazonCognitoIdentityService _awsIdentityService;

        public CognitoAuthService(AppSettings appSettings, IAmazonCognitoIdentityService awsIdentityService)
        {
            _appSettings = appSettings;
            _awsIdentityService = awsIdentityService;
        }

        public async Task<Domain.Models.ChangePassword.ChangePasswordResponse> TryChangePasswordAsync(Domain.Models.ChangePassword.ChangePasswordRequest request)
        {
            // FetchTokens for User
            var tokenResponse = await AuthenticateUserAsync(request.Username, request.CurrentPassword);

            var changePasswordRequest = new ChangePasswordRequest
            {
                AccessToken = tokenResponse.AccessToken,
                PreviousPassword = request.CurrentPassword,
                ProposedPassword = request.NewPassword
            };

            await _awsIdentityService.ChangePasswordAsync(changePasswordRequest);

            return new Domain.Models.ChangePassword.ChangePasswordResponse
            {
                Username = request.Username
            };
        }

        public async Task<AuthResponse> TryLoginAsync(UserAuthRequest request)
        {
            try
            {
                var result = await AuthenticateUserAsync(request.Username, request.Password);
                                
                return new AuthResponse
                {
                    Username = request.Username,
                    Tokens = new TokenResponse
                    {
                        IdToken = result.IdToken,
                        AccessToken = result.AccessToken,
                        ExpiresIn = result.ExpiresIn,
                        RefreshToken = result.RefreshToken
                    }
                };
            }
            catch (UserNotConfirmedException)
            {
                var listUsersResponse = await FindUsersByEmailAddressAsync(request.Username);

                if (listUsersResponse != null && listUsersResponse.HttpStatusCode == HttpStatusCode.OK)
                {
                    var users = listUsersResponse.Users;
                    var filtered_user = users.FirstOrDefault(x => x.Attributes.Any(x => (x.Name == "email" && x.Value == request.Username) || (x.Name == "preferred_username" && x.Value == request.Username)));

                    var resendCodeResponse = await _awsIdentityService.ResendConfirmationCodeAsync(new ResendConfirmationCodeRequest
                    {
                        ClientId = _appSettings.AWSCognitoSettings.AppClientId,
                        Username = filtered_user.Username
                    });

                    if (resendCodeResponse.HttpStatusCode == HttpStatusCode.OK)
                    {
                        return new AuthResponse
                        {
                            Username = filtered_user.Username,
                            Message = $"Confirmation Code sent to {resendCodeResponse.CodeDeliveryDetails.Destination} via {resendCodeResponse.CodeDeliveryDetails.DeliveryMedium.Value}",
                        };
                    }
                    else
                    {
                        return new AuthResponse
                        {
                            Username = filtered_user.Username,
                            Message = $"Resend Confirmation Code Response: {resendCodeResponse.HttpStatusCode}"
                        };
                    }
                }
                else
                {
                    return new AuthResponse
                    {
                        Username = string.Empty,
                        Message = "No Users found.",
                    };
                }
            }
            catch (UserNotFoundException)
            {
                return new AuthResponse
                {
                    Username = string.Empty,
                    Message = "User not found"
                };
            }
            catch (NotAuthorizedException)
            {
                return new AuthResponse
                {
                    Username = string.Empty,
                    Message = "Incorrect username or password"
                };
            }
        }
                
        public async Task<bool> TryLogOutAsync(UserSignOutRequest model)
        {
            var request = new GlobalSignOutRequest { AccessToken = model.AccessToken };
            await _awsIdentityService.GlobalSignOutAsync(request);

            return true;
        }

        public async Task<Domain.Models.ForgotPassword.ForgotPasswordResponse> TryInitForgotPasswordAsync(Domain.Models.ForgotPassword.ForgotPasswordRequest request)
        {
            var listUsersResponse = await FindUsersByEmailAddressAsync(request.Username);

            if (listUsersResponse != null && listUsersResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                var users = listUsersResponse.Users;

                var filtered_user = users.FirstOrDefault(x => x.Attributes.Any(x => (x.Name == "email" && x.Value == request.Username) || (x.Name == "preferred_username" && x.Value == request.Username)));
                if (filtered_user != null)
                {
                    var forgotPasswordResponse = await _awsIdentityService.ForgotPasswordAsync(new ForgotPasswordRequest
                    {
                        ClientId = _appSettings.AWSCognitoSettings.AppClientId,
                        Username = filtered_user.Username
                    });

                    if (forgotPasswordResponse.HttpStatusCode == HttpStatusCode.OK)
                    {
                        return new Domain.Models.ForgotPassword.ForgotPasswordResponse
                        {
                            UserId = filtered_user.Username,
                            Username = request.Username,
                            Message = $"Confirmation Code sent to {forgotPasswordResponse.CodeDeliveryDetails.Destination} via {forgotPasswordResponse.CodeDeliveryDetails.DeliveryMedium.Value}"
                        };
                    }
                    else
                    {
                        return new Domain.Models.ForgotPassword.ForgotPasswordResponse
                        {
                            UserId = string.Empty,
                            Username = request.Username,
                            Message = $"ListUsers Response: {forgotPasswordResponse.HttpStatusCode}",
                        };
                    }
                }
                else
                {
                    return new Domain.Models.ForgotPassword.ForgotPasswordResponse
                    {
                        UserId = string.Empty,
                        Username = request.Username,
                        Message = $"No users with the given username found"
                    };
                }
            }
            else
            {
                return new Domain.Models.ForgotPassword.ForgotPasswordResponse
                {
                    UserId = string.Empty,
                    Username = string.Empty,
                    Message = $"Error trying to recover user data"
                };
            }
        }

        public async Task<bool> TryResetPasswordWithConfirmationCodeAsync(ResetPasswordRequest request)
        {
            var response = await _awsIdentityService.ConfirmForgotPasswordAsync(new ConfirmForgotPasswordRequest
            {
                ClientId = _appSettings.AWSCognitoSettings.AppClientId,
                Username = request.UserId,
                Password = request.NewPassword,
                ConfirmationCode = request.ConfirmationCode
            });

            if (response.HttpStatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Can't reset password");
            }

            return true;
        }

        private async Task<AuthenticationResultType> AuthenticateUserAsync(string emailAddress, string password)
        {
            var authRequest = new InitiateAuthRequest()
            {
                AuthFlow = "USER_PASSWORD_AUTH",
                ClientId = _appSettings.AWSCognitoSettings.AppClientId,
                AuthParameters =
                {   
                    { "USERNAME", emailAddress},
                    { "PASSWORD", password}
                }
                
            };

            var authResponse = await _awsIdentityService.InitiateAuthAsync(authRequest);
            var result = authResponse.AuthenticationResult;

            return result;
        }

        private async Task<ListUsersResponse> FindUsersByEmailAddressAsync(string emailAddress)
        {
            var listUsersRequest = new ListUsersRequest
            {
                UserPoolId = _appSettings.AWSCognitoSettings.UserPoolId,
                Filter = $"email=\"{emailAddress}\""
            };

            return await _awsIdentityService.ListUsersAsync(listUsersRequest);
        }
    }
}
