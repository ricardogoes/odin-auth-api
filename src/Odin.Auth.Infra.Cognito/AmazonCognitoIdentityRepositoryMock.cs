using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;

namespace Odin.Auth.Infra.Cognito
{
    public class AmazonCognitoIdentityRepositoryMock : IAmazonCognitoIdentityRepository
    {
        public AmazonCognitoIdentityRepositoryMock()
        {
        }

        public async Task<AdminCreateUserResponse> AdminCreateUserAsync(AdminCreateUserRequest request)
        {
            return await Task.FromResult(new AdminCreateUserResponse
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                User = new UserType
                {
                    Username = "unit.testing"
                }
            });
        }

        public async Task AdminDisableUserAsync(AdminDisableUserRequest request)
        {
            await Task.CompletedTask;
        }

        public async Task AdminEnableUserAsync(AdminEnableUserRequest request)
        {
            await Task.CompletedTask;
        }

        public async Task<AdminGetUserResponse> AdminGetUserAsync(AdminGetUserRequest request)
        {
            if (request.Username.Equals("user.not.found"))
                throw new UserNotFoundException("User not found");

            return await Task.FromResult(new AdminGetUserResponse
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Username = "unit.testing",
                UserAttributes = new List<AttributeType> 
                { 
                    new AttributeType { Name = "given_name", Value = "Unit" },
                    new AttributeType { Name = "family_name", Value = "Testing" },
                    new AttributeType { Name = "email", Value = "unit.testing@email.com" },
                    new AttributeType { Name = "preferred_username", Value = "unit.testing" }
                }
            });
        }

        public async Task<AdminInitiateAuthResponse> AdminInitiateAuthAsync(AdminInitiateAuthRequest request)
        {
            return await Task.FromResult(new AdminInitiateAuthResponse
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Session = "session-123123123123"
            });
        }

        public async Task AdminRespondToAuthChallengeAsync(AdminRespondToAuthChallengeRequest request)
        {
            await Task.CompletedTask;
        }

        public async Task AdminUpdateUserAttributesAsync(AdminUpdateUserAttributesRequest request)
        {
            await Task.CompletedTask;
        }

        public async Task<ChangePasswordResponse> ChangePasswordAsync(ChangePasswordRequest request)
        {
            return await Task.FromResult(new ChangePasswordResponse
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK
            });
        }

        public async Task<ConfirmForgotPasswordResponse> ConfirmForgotPasswordAsync(ConfirmForgotPasswordRequest request)
        {
            if (request.Username == "user.with.error")
                return await Task.FromResult(new ConfirmForgotPasswordResponse
                {
                    HttpStatusCode = System.Net.HttpStatusCode.BadRequest
                });
            else
                return await Task.FromResult(new ConfirmForgotPasswordResponse
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK                
                });
        }

        public async Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            if(request.Username == "user.with.error")
                return await Task.FromResult(new ForgotPasswordResponse
                {
                    HttpStatusCode = System.Net.HttpStatusCode.BadRequest
                });
            else
                return await Task.FromResult(new ForgotPasswordResponse
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    CodeDeliveryDetails = new CodeDeliveryDetailsType
                    {
                        Destination = "destination",
                        DeliveryMedium = new DeliveryMediumType("DeliveryMedium")
                    }
                });
        }

        public async Task<GlobalSignOutResponse> GlobalSignOutAsync(GlobalSignOutRequest request)
        {
            return await Task.FromResult(new GlobalSignOutResponse
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK
            });
        }

        public async Task<InitiateAuthResponse> InitiateAuthAsync(InitiateAuthRequest request)
        {
            if (request.AuthParameters["USERNAME"].Contains("user.not.found"))
                throw new UserNotFoundException("User not found");
            else if(request.AuthParameters["USERNAME"].Contains("user.not.confirmed"))
                throw new UserNotConfirmedException("User not confirmed");
            else if (request.AuthParameters["USERNAME"].Contains("user.not.authenticated"))
                throw new NotAuthorizedException("User not authenticated");
            else
                return await Task.FromResult(new InitiateAuthResponse
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK, 
                    AuthenticationResult = new AuthenticationResultType
                    {
                        AccessToken = "access-token",
                        ExpiresIn = 3600,
                        IdToken = "id-token",
                        RefreshToken = "refresh-token",
                        TokenType = "Bearer"
                    }
                });
        }

        public async Task<ListUsersResponse?> ListUsersAsync(ListUsersRequest request)
        {
            if (request.Filter.Equals("user.not.confirmed") || request.Filter.Contains("user.not.found"))
                return null;

            return await Task.FromResult(new ListUsersResponse
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Users = new List<UserType>
                {
                    new UserType 
                    { 
                        Username = "unit.testing",
                        Attributes = new List<AttributeType>
                        {
                            new AttributeType {Name = "preferred_username", Value = "unit.testing"},
                            new AttributeType {Name = "email", Value = "unit.testing@email.com"}
                        }
                    },
                    new UserType 
                    { 
                        Username = "user.not.confirmed" ,
                        Attributes = new List<AttributeType>
                        {
                            new AttributeType {Name = "preferred_username", Value = "user.not.confirmed"},
                            new AttributeType {Name = "email", Value = "user.not.confirmed@email.com"}
                        }
                    },
                    new UserType 
                    { 
                        Username = "user.not.confirmed.not-sent" ,
                        Attributes = new List<AttributeType>
                        {
                            new AttributeType {Name = "preferred_username", Value = "user.not.confirmed.not-sent"},
                            new AttributeType {Name = "email", Value = "user.not.confirmed.not-sent@email.com"}
                        }
                    },
                    new UserType
                    {
                        Username = "user.with.error" ,
                        Attributes = new List<AttributeType>
                        {
                            new AttributeType {Name = "preferred_username", Value = "user.with.error"},
                            new AttributeType {Name = "email", Value = "user.with.error@email.com"}
                        }
                    }
                }
            });
        }

        public async Task<ResendConfirmationCodeResponse> ResendConfirmationCodeAsync(ResendConfirmationCodeRequest request)
        {
            if(request.Username == "user.not.confirmed.not.found")
                return await Task.FromResult(new ResendConfirmationCodeResponse
                {
                    HttpStatusCode = System.Net.HttpStatusCode.BadRequest
                });
            else if (request.Username == "user.not.confirmed.not-sent")
                return await Task.FromResult(new ResendConfirmationCodeResponse
                {
                    HttpStatusCode = System.Net.HttpStatusCode.BadRequest
                });
            else
                return await Task.FromResult(new ResendConfirmationCodeResponse
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    CodeDeliveryDetails = new CodeDeliveryDetailsType
                    {
                        Destination = "destination",
                        DeliveryMedium = new DeliveryMediumType("DeliveryMedium")
                    }
                });
        }
    }
}
