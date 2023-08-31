using Amazon.CognitoIdentityProvider.Model;
using MediatR;
using Odin.Auth.Application.Common;
using Odin.Auth.Infra.Cognito;

namespace Odin.Auth.Application.UpdateProfile
{
    public class UpdateProfile : IRequestHandler<UpdateProfileInput, UpdateProfileOutput>
    {
        private readonly AppSettings _appSettings;
        private readonly IAmazonCognitoIdentityRepository _awsIdentityRepository;
        private readonly ICommonService _commonService;

        public UpdateProfile(AppSettings appSettings, IAmazonCognitoIdentityRepository awsIdentityRepository, ICommonService commonService)
        {
            _appSettings = appSettings;
            _awsIdentityRepository = awsIdentityRepository;
            _commonService = commonService;
        }

        public async Task<UpdateProfileOutput> Handle(UpdateProfileInput request, CancellationToken cancellationToken)
        {
            try
            {
                await _commonService.GetUserByUsernameAsync(request.Username, cancellationToken);
            }
            catch (UserNotFoundException)
            {
                throw;
            }

            var userAttributesRequest = new AdminUpdateUserAttributesRequest
            {
                UserPoolId = _appSettings.AWSCognitoSettings.UserPoolId,
                Username = request.Username
            };

            userAttributesRequest.UserAttributes.Add(new AttributeType
            {
                Value = request.FirstName,
                Name = "given_name"
            });

            userAttributesRequest.UserAttributes.Add(new AttributeType
            {
                Value = request.LastName,
                Name = "family_name"
            });

            userAttributesRequest.UserAttributes.Add(new AttributeType
            {
                Value = request.EmailAddress,
                Name = "email"
            });

            await _awsIdentityRepository.AdminUpdateUserAttributesAsync(userAttributesRequest);

            return new UpdateProfileOutput
            {
                Username = request.Username
            };
        }
    }
}
