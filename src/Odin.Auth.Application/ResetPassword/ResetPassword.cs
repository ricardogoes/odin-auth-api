using Amazon.CognitoIdentityProvider.Model;
using MediatR;
using Odin.Auth.Application.Common;
using Odin.Auth.Infra.Cognito;
using System.Net;

namespace Odin.Auth.Application.ResetPassword
{
    public class ResetPassword : IRequestHandler<ResetPasswordInput, ResetPasswordOutput>
    {
        private readonly AppSettings _appSettings;
        private readonly IAmazonCognitoIdentityRepository _awsIdentityRepository;

        public ResetPassword(AppSettings appSettings, IAmazonCognitoIdentityRepository awsIdentityRepository)
        {
            _appSettings = appSettings;
            _awsIdentityRepository = awsIdentityRepository;
        }

        public async Task<ResetPasswordOutput> Handle(ResetPasswordInput request, CancellationToken cancellationToken)
        {
            var response = await _awsIdentityRepository.ConfirmForgotPasswordAsync(new ConfirmForgotPasswordRequest
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

            return new ResetPasswordOutput(request.Username);
        }
    }
}
