using Amazon.CognitoIdentityProvider.Model;
using MediatR;
using Odin.Auth.Application.Common;
using Odin.Auth.Infra.Cognito;
using System.Net;

namespace Odin.Auth.Application.Login
{
    public class Login : IRequestHandler<LoginInput, LoginOutput>
    {
        private readonly AppSettings _appSettings;
        private readonly ICommonService _commonService;
        private readonly IAmazonCognitoIdentityRepository _awsIdentityRepository;

        public Login(AppSettings appSettings, ICommonService commonService, IAmazonCognitoIdentityRepository awsIdentityRepository)
        {
            _appSettings = appSettings;
            _commonService = commonService;
            _awsIdentityRepository = awsIdentityRepository;
        } 

        public async Task<LoginOutput> Handle(LoginInput request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _commonService.AuthenticateUserAsync(request.Username, request.Password, cancellationToken);

                return new LoginOutput
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
                var listUsersResponse = await _commonService.FindUsersByEmailAddressAsync(request.Username, cancellationToken);

                if (listUsersResponse != null && listUsersResponse.HttpStatusCode == HttpStatusCode.OK)
                {
                    var users = listUsersResponse.Users;
                    var filtered_user = users.FirstOrDefault(x => x.Attributes.Any(x => x.Name == "email" && x.Value == request.Username || x.Name == "preferred_username" && x.Value == request.Username));

                    var resendCodeResponse = await _awsIdentityRepository.ResendConfirmationCodeAsync(new ResendConfirmationCodeRequest
                    {
                        ClientId = _appSettings.AWSCognitoSettings.AppClientId,
                        Username = filtered_user.Username
                    });

                    if (resendCodeResponse.HttpStatusCode == HttpStatusCode.OK)
                    {
                        return new LoginOutput
                        {
                            Username = filtered_user.Username,
                            Message = $"Confirmation Code sent to {resendCodeResponse.CodeDeliveryDetails.Destination} via {resendCodeResponse.CodeDeliveryDetails.DeliveryMedium.Value}",
                        };
                    }
                    else
                    {
                        return new LoginOutput
                        {
                            Username = filtered_user.Username,
                            Message = $"Resend Confirmation Code Response: {resendCodeResponse.HttpStatusCode}"
                        };
                    }
                }
                else
                {
                    return new LoginOutput
                    {
                        Username = string.Empty,
                        Message = "No Users found.",
                    };
                }
            }
            catch (UserNotFoundException)
            {
                return new LoginOutput
                {
                    Username = string.Empty,
                    Message = "User not found"
                };
            }
            catch (NotAuthorizedException)
            {
                return new LoginOutput
                {
                    Username = string.Empty,
                    Message = "Incorrect username or password"
                };
            }
        }
    }
}
