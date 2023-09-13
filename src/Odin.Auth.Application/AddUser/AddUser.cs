using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using MediatR;
using Odin.Auth.Application.Common;
using Odin.Auth.Infra.Cognito;

namespace Odin.Auth.Application.AddUser
{
    public class AddUser : IRequestHandler<AddUserInput, AddUserOutput>
    {
        private readonly AppSettings _appSettings;
        private readonly IAmazonCognitoIdentityRepository _awsIdentityRepository;

        public AddUser(AppSettings appSettings, IAmazonCognitoIdentityRepository awsIdentityRepository)
        {
            _appSettings = appSettings;
            _awsIdentityRepository = awsIdentityRepository;
        }

        public async Task<AddUserOutput> Handle(AddUserInput request, CancellationToken cancellationToken)
        {
            var adminCreateUserRequest = new AdminCreateUserRequest
            {
                Username = request.Username,
                TemporaryPassword = request.Password,
                UserPoolId = _appSettings.AWSCognitoSettings.UserPoolId
            };

            adminCreateUserRequest.UserAttributes.Add(new AttributeType
            {
                Value = request.FirstName,
                Name = "given_name"
            });
            adminCreateUserRequest.UserAttributes.Add(new AttributeType
            {
                Value = request.LastName,
                Name = "family_name"
            });
            adminCreateUserRequest.UserAttributes.Add(new AttributeType
            {
                Value = request.Username,
                Name = "preferred_username"
            });
            adminCreateUserRequest.UserAttributes.Add(new AttributeType
            {
                Name = "email",
                Value = request.EmailAddress
            });

            await _awsIdentityRepository
                .AdminCreateUserAsync(adminCreateUserRequest)
                .ConfigureAwait(false);

            var adminUpdateUserAttributesRequest = new AdminUpdateUserAttributesRequest
            {
                Username = request.Username,
                UserPoolId = _appSettings.AWSCognitoSettings.UserPoolId,
                UserAttributes = new List<AttributeType>
                    {
                        new AttributeType()
                        {
                            Name = "email_verified",
                            Value = "true"
                        }
                    }
            };

            await _awsIdentityRepository
                .AdminUpdateUserAttributesAsync(adminUpdateUserAttributesRequest)
            .ConfigureAwait(false);

            var adminInitiateAuthRequest = new AdminInitiateAuthRequest
            {
                UserPoolId = _appSettings.AWSCognitoSettings.UserPoolId,
                ClientId = _appSettings.AWSCognitoSettings.AppClientId,
                AuthFlow = AuthFlowType.ADMIN_NO_SRP_AUTH,
                AuthParameters = new Dictionary<string, string>
                {
                    { "USERNAME", request.Username },
                    { "PASSWORD", request.Password }
                }
            };

            var adminInitiateAuthResponse = await _awsIdentityRepository
                .AdminInitiateAuthAsync(adminInitiateAuthRequest)
                .ConfigureAwait(false);

            var adminRespondToAuthChallengeRequest = new AdminRespondToAuthChallengeRequest
            {
                ChallengeName = ChallengeNameType.NEW_PASSWORD_REQUIRED,
                ClientId = _appSettings.AWSCognitoSettings.AppClientId,
                UserPoolId = _appSettings.AWSCognitoSettings.UserPoolId,
                ChallengeResponses = new Dictionary<string, string>
                    {
                        { "USERNAME", request.Username },
                        { "NEW_PASSWORD", request.Password }
                    },
                Session = adminInitiateAuthResponse.Session
            };

            await _awsIdentityRepository
                .AdminRespondToAuthChallengeAsync(adminRespondToAuthChallengeRequest)
                .ConfigureAwait(false);

            return new AddUserOutput(request.Username, request.EmailAddress);
        }
    }
}
