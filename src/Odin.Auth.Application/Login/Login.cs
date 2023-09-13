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

                var tokens = new TokenResponse(result.IdToken, result.AccessToken, result.ExpiresIn, result.RefreshToken);
                return new LoginOutput(request.Username, tokens);
            }
            catch (UserNotConfirmedException)
            {
                var listUsersResponse = await _commonService.FindUsersByEmailAddressAsync(request.Username, cancellationToken);

                if (listUsersResponse != null && listUsersResponse.HttpStatusCode == HttpStatusCode.OK)
                {
                    var users = listUsersResponse.Users;
                    var filtered_user = users.FirstOrDefault(x => x.Attributes.Any(x => x.Name == "email" && x.Value == request.Username || x.Name == "preferred_username" && x.Value == request.Username))!;

                    var resendCodeResponse = await _awsIdentityRepository.ResendConfirmationCodeAsync(new ResendConfirmationCodeRequest
                    {
                        ClientId = _appSettings.AWSCognitoSettings.AppClientId,
                        Username = filtered_user.Username
                    });

                    if (resendCodeResponse.HttpStatusCode == HttpStatusCode.OK)
                    {
                        return new LoginOutput(
                            filtered_user.Username, 
                            $"Confirmation Code sent to {resendCodeResponse.CodeDeliveryDetails.Destination} via {resendCodeResponse.CodeDeliveryDetails.DeliveryMedium.Value}"
                        );
                    }
                    else
                    {
                        return new LoginOutput
                        (
                            filtered_user.Username,
                            $"Resend Confirmation Code Response: {resendCodeResponse.HttpStatusCode}"
                        );
                    }
                }
                else
                {
                    return new LoginOutput(string.Empty, "No Users found.");
                }
            }
            catch (UserNotFoundException)
            {
                return new LoginOutput(string.Empty, "User not found" );
            }
            catch (NotAuthorizedException)
            {
                return new LoginOutput(string.Empty, "Incorrect username or password");
            }
        }
    }
}
