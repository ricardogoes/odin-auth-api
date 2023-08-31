using Amazon.CognitoIdentityProvider.Model;
using FluentAssertions;
using Moq;
using Odin.Auth.Application.Common;
using Odin.Auth.Application.Login;
using Odin.Auth.Infra.Cognito;
using Xunit.Abstractions;
using App = Odin.Auth.Application.Login;

namespace Odin.Auth.UnitTests.Application.Login
{
    [Collection(nameof(LoginTestFixtureCollection))]
    public class LoginTest
    {
        private readonly LoginTestFixture _fixture;
        private readonly AmazonCognitoIdentityRepositoryMock _awsIdentityRepository;
        private readonly Mock<ICommonService> _commonServiceMock;

        public LoginTest(LoginTestFixture fixture)
        {
            _fixture = fixture;
            _awsIdentityRepository = _fixture.GetAwsIdentityRepository();
            _commonServiceMock = new();
        }

        [Fact(DisplayName = "Handle() should return OK with valid data")]
        [Trait("Application", "Login / Login")]
        public async Task TryLoginAsync_Ok()
        {
            _commonServiceMock.Setup(s => s.AuthenticateUserAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(new AuthenticationResultType
                {
                    AccessToken = "access-token",
                    ExpiresIn = 3600,
                    IdToken = "id-token",
                    RefreshToken = "refresh-token",
                    TokenType = "Bearer"
                }));
            
            var app = new App.Login(_fixture.AppSettings, _commonServiceMock.Object, _awsIdentityRepository);
            var response = await app.Handle(new App.LoginInput
            {
                Username = "unit.testing",
                Password = "password"
            }, CancellationToken.None);

            response.Username.Should().Be("unit.testing");
        }

        [Fact(DisplayName = "Handle() should return not authenticated when invalid user or password")]
        [Trait("Application", "Login / Login")]
        public async Task TryLoginAsync_UserNotAuthenticated()
        {
            _commonServiceMock.Setup(s => s.AuthenticateUserAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Throws(new NotAuthorizedException("Incorrect username or password")); 
            
            var app = new App.Login(_fixture.AppSettings, _commonServiceMock.Object, _awsIdentityRepository);
            var response = await app.Handle(new LoginInput
            {
                Username = "user.not.authenticated",
                Password = "password"
            }, CancellationToken.None);

            response.Username.Should().BeEmpty();
            response.Message.Should().Be("Incorrect username or password");
        }

        [Fact(DisplayName = "Handle() should return not found when user not found")]
        [Trait("Application", "Login / Login")]
        public async Task TryLoginAsync_UserNotFound()
        {
            _commonServiceMock.Setup(s => s.AuthenticateUserAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Throws(new UserNotFoundException("User not found"));

            var app = new App.Login(_fixture.AppSettings, _commonServiceMock.Object, _awsIdentityRepository);
            var response = await app.Handle(new LoginInput
            {
                Username = "user.not.found",
                Password = "password"
            }, CancellationToken.None);

            response.Username.Should().BeEmpty();
            response.Message.Should().Be("User not found");
        }

        [Fact(DisplayName = "Handle() should resend confirmation code when user not confirmed")]
        [Trait("Application", "Login / Login")]
        public async Task TryLoginAsync_ResendConfirmationCode()
        {
            _commonServiceMock.Setup(s => s.AuthenticateUserAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Throws(new UserNotConfirmedException(""));

            _commonServiceMock.Setup(s => s.FindUsersByEmailAddressAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(new ListUsersResponse
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Users = new List<UserType>
                    {
                        new UserType
                        {
                            Username = "user.not.confirmed",
                            Attributes = new List<AttributeType>
                            {
                                new AttributeType {Name = "preferred_username", Value = "user.not.confirmed"},
                                new AttributeType {Name = "email", Value = "user.not.confirmed@email.com"}
                            }
                        }
                    }
                })); 
            
            var app = new App.Login(_fixture.AppSettings, _commonServiceMock.Object, _awsIdentityRepository);
            var response = await app.Handle(new LoginInput
            {
                Username = "user.not.confirmed",
                Password = "password"
            }, CancellationToken.None);

            response.Username.Should().Be("user.not.confirmed");
            response.Message.Should().Contain("Confirmation Code sent");
        }

        [Fact(DisplayName = "Handle() should not resend confirmation code when an error ocurred")]
        [Trait("Application", "Login / Login")]
        public async Task TryLoginAsync_NotResendConfirmationCodeWhenError()
        {
            _commonServiceMock.Setup(s => s.AuthenticateUserAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Throws(new UserNotConfirmedException(""));

            _commonServiceMock.Setup(s => s.FindUsersByEmailAddressAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(new ListUsersResponse
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Users = new List<UserType>
                    {
                        new UserType
                        {
                            Username = "user.not.confirmed.not-sent",
                            Attributes = new List<AttributeType>
                            {
                                new AttributeType {Name = "preferred_username", Value = "user.not.confirmed.not-sent"},
                                new AttributeType {Name = "email", Value = "user.not.confirmed.not-sent@email.com"}
                            }
                        }
                    }
                }));

            var app = new App.Login(_fixture.AppSettings, _commonServiceMock.Object, _awsIdentityRepository);
            var response = await app.Handle(new LoginInput
            {
                Username = "user.not.confirmed.not-sent",
                Password = "password"
            }, CancellationToken.None);

            response.Username.Should().Be("user.not.confirmed.not-sent");
            response.Message.Should().Contain("Resend Confirmation Code Response");
        }
    }
}
