using Amazon.CognitoIdentityProvider.Model;
using MediatR;
using Odin.Auth.Application.Common;
using Odin.Auth.Infra.Cognito;

namespace Odin.Auth.Application.ChangeStatusUser
{
    public class ChangeStatusUser : IRequestHandler<ChangeStatusUserInput, ChangeStatusUserOutput>
    {
        private readonly AppSettings _appSettings;
        private readonly IAmazonCognitoIdentityRepository _awsIdentityRepository;
        private readonly ICommonService _commonService;

        public ChangeStatusUser(AppSettings appSettings, IAmazonCognitoIdentityRepository awsIdentityRepository, ICommonService commonService)
        {
            _appSettings = appSettings;
            _commonService = commonService;
            _awsIdentityRepository = awsIdentityRepository;
        }

        public async Task<ChangeStatusUserOutput> Handle(ChangeStatusUserInput input, CancellationToken cancellationToken)
        {
            try
            {
                await _commonService.GetUserByUsernameAsync(input.Username, cancellationToken);
            }
            catch (UserNotFoundException)
            {
                throw;
            }

            ChangeStatusUserOutput? output = input.Action switch
            {
                ChangeStatusAction.ACTIVATE => await EnableUserAsync(input.Username, cancellationToken),
                ChangeStatusAction.DEACTIVATE => await DisableUserAsync(input.Username, cancellationToken),
                _ => throw new ArgumentException("Invalid Action", nameof(input)),
            };
            return output;
        }

        private async Task<ChangeStatusUserOutput> EnableUserAsync(string username, CancellationToken cancellationToken)
        {            

            var userAttributesRequest = new AdminEnableUserRequest
            {
                UserPoolId = _appSettings.AWSCognitoSettings.UserPoolId,
                Username = username
            };

            await _awsIdentityRepository.AdminEnableUserAsync(userAttributesRequest);

            return new ChangeStatusUserOutput(username);
        }

        private async Task<ChangeStatusUserOutput> DisableUserAsync(string username, CancellationToken cancellationToken)
        { 
            var userAttributesRequest = new AdminDisableUserRequest
            {
                UserPoolId = _appSettings.AWSCognitoSettings.UserPoolId,
                Username = username
            };

            await _awsIdentityRepository.AdminDisableUserAsync(userAttributesRequest);

            return new ChangeStatusUserOutput(username);
        }
    }
}
