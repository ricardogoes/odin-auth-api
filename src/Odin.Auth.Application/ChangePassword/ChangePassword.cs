using Amazon.CognitoIdentityProvider.Model;
using MediatR;
using Odin.Auth.Application.Common;
using Odin.Auth.Infra.Cognito;

namespace Odin.Auth.Application.ChangePassword
{
    public class ChangePassword : IRequestHandler<ChangePasswordInput, ChangePasswordOutput>
    {
        private readonly ICommonService _commonService;
        private readonly IAmazonCognitoIdentityRepository _awsIdentityRepository;

        public ChangePassword(ICommonService commonService, IAmazonCognitoIdentityRepository awsIdentityRepository)
        {
            _commonService = commonService;
            _awsIdentityRepository = awsIdentityRepository;
        }

        public async Task<ChangePasswordOutput> Handle(ChangePasswordInput request, CancellationToken cancellationToken)
        {
            // FetchTokens for User
            var tokenResponse = await _commonService.AuthenticateUserAsync(request.Username, request.CurrentPassword, cancellationToken);

            var changePasswordRequest = new ChangePasswordRequest
            {
                AccessToken = tokenResponse.AccessToken,
                PreviousPassword = request.CurrentPassword,
                ProposedPassword = request.NewPassword
            };

            await _awsIdentityRepository.ChangePasswordAsync(changePasswordRequest);

            return new ChangePasswordOutput(request.Username);
        }
    }
}
