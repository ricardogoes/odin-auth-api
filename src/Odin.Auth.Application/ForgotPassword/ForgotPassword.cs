using Amazon.CognitoIdentityProvider.Model;
using MediatR;
using Odin.Auth.Application.Common;
using Odin.Auth.Infra.Cognito;
using System.Net;

namespace Odin.Auth.Application.ForgotPassword
{
    public class ForgotPassword : IRequestHandler<ForgotPasswordInput, ForgotPasswordOutput>
    {
        private readonly AppSettings _appSettings;
        private readonly ICommonService _commonService;
        private readonly IAmazonCognitoIdentityRepository _awsIdentityRepository;

        public ForgotPassword(AppSettings appSettings, ICommonService commonService, IAmazonCognitoIdentityRepository awsIdentityRepository)
        {
            _appSettings = appSettings;
            _commonService = commonService;
            _awsIdentityRepository = awsIdentityRepository;
        }

        public async Task<ForgotPasswordOutput> Handle(ForgotPasswordInput request, CancellationToken cancellationToken)
        {
            var listUsersResponse = await _commonService.FindUsersByEmailAddressAsync(request.Username, cancellationToken);

            if (listUsersResponse != null && listUsersResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                var users = listUsersResponse.Users;

                var filtered_user = users.FirstOrDefault(x => x.Attributes.Any(x => x.Name == "email" && x.Value == request.Username || x.Name == "preferred_username" && x.Value == request.Username));
                if (filtered_user != null)
                {
                    var forgotPasswordResponse = await _awsIdentityRepository.ForgotPasswordAsync(new ForgotPasswordRequest
                    {
                        ClientId = _appSettings.AWSCognitoSettings.AppClientId,
                        Username = filtered_user.Username
                    });

                    if (forgotPasswordResponse.HttpStatusCode == HttpStatusCode.OK)
                    {
                        return new ForgotPasswordOutput
                        {
                            UserId = filtered_user.Username,
                            Username = request.Username,
                            Message = $"Confirmation Code sent to {forgotPasswordResponse.CodeDeliveryDetails.Destination} via {forgotPasswordResponse.CodeDeliveryDetails.DeliveryMedium.Value}"
                        };
                    }
                    else
                    {
                        return new ForgotPasswordOutput
                        {
                            UserId = string.Empty,
                            Username = request.Username,
                            Message = $"ListUsers Response: {forgotPasswordResponse.HttpStatusCode}",
                        };
                    }
                }
                else
                {
                    return new ForgotPasswordOutput
                    {
                        UserId = string.Empty,
                        Username = request.Username,
                        Message = $"No users with the given username found"
                    };
                }
            }
            else
            {
                return new ForgotPasswordOutput
                {
                    UserId = string.Empty,
                    Username = string.Empty,
                    Message = $"Error trying to recover user data"
                };
            }
        }
    }
}
