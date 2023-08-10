using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Odin.Auth.Domain.Interfaces.Cognito;
using Odin.Auth.Domain.Interfaces.Services;
using Odin.Auth.Domain.Models;
using Odin.Auth.Domain.Models.UpdateProfile;
using Odin.Auth.Domain.Models.UserLogin;
using Odin.Auth.Domain.Models.UserProfile;

namespace Odin.Auth.Service.ExternalServices
{
    public class CognitoUserService : ICognitoUserService
    {
        private readonly AppSettings _appSettings;
        private readonly IAmazonCognitoIdentityService _awsIdentityService;

        public CognitoUserService(AppSettings appSettings, IAmazonCognitoIdentityService awsIdentityService)
        {
            _appSettings = appSettings;
            _awsIdentityService = awsIdentityService;
        }

        public async Task<InsertUserResponse> InsertUserAsync(InsertUserRequest request)
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

            var r = await _awsIdentityService
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

            await  _awsIdentityService
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

            var adminInitiateAuthResponse = await _awsIdentityService
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

            await _awsIdentityService
                .AdminRespondToAuthChallengeAsync(adminRespondToAuthChallengeRequest)
                .ConfigureAwait(false);

            return new InsertUserResponse
            {
                Username = request.Username,
                EmailAddress = request.EmailAddress
            };
        }

        public async Task<UpdateProfileResponse> UpdateUserAttributesAsync(UpdateProfileRequest request)
        {
            try
            {
                await GetUserByUsernameAsync(request.Username);
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

            await _awsIdentityService.AdminUpdateUserAttributesAsync(userAttributesRequest);

            return new UpdateProfileResponse
            {
                Username = request.Username
            };
        }

        public async Task<UpdateProfileResponse> EnableUserAsync(string username)
        {
            try
            {
                await GetUserByUsernameAsync(username);
            }
            catch (UserNotFoundException)
            {
                throw;
            }            

            var userAttributesRequest = new AdminEnableUserRequest
            {
                UserPoolId = _appSettings.AWSCognitoSettings.UserPoolId,
                Username = username
            };

            await _awsIdentityService.AdminEnableUserAsync(userAttributesRequest);

            return new UpdateProfileResponse
            {
                Username = username
            };
        }

        public async Task<UpdateProfileResponse> DisableUserAsync(string username)
        {

            try
            {
                await GetUserByUsernameAsync(username);
            }
            catch (UserNotFoundException)
            {
                throw;
            }

            var userAttributesRequest = new AdminDisableUserRequest
            {
                UserPoolId = _appSettings.AWSCognitoSettings.UserPoolId,
                Username = username
            };

            await _awsIdentityService.AdminDisableUserAsync(userAttributesRequest);

            return new UpdateProfileResponse
            {
                Username = username
            };
        }

        public async Task<UserProfileResponse> GetUserByUsernameAsync(string username)
        {
            var userResponse = new AdminGetUserResponse();

            try
            {
                userResponse = await _awsIdentityService.AdminGetUserAsync(new AdminGetUserRequest
                {
                    Username = username,
                    UserPoolId = _appSettings.AWSCognitoSettings.UserPoolId
                });
            }
            catch (UserNotFoundException)
            {
                throw;
            }   

            var attributes = userResponse.UserAttributes;

            return new UserProfileResponse
            {
                Username = username,
                FirstName = attributes.First(x => x.Name == "given_name").Value ?? string.Empty,
                LastName = attributes.First(x => x.Name == "family_name").Value ?? string.Empty,
                PreferredUsername = attributes.First(x => x.Name == "preferred_username").Value ?? string.Empty,
                EmailAddress = attributes.First(x => x.Name == "email").Value ?? string.Empty
            };
        }
    }
}


