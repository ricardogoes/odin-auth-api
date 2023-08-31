using FluentAssertions;
using Newtonsoft.Json.Linq;
using Odin.Auth.Infra.Cognito;
using App = Odin.Auth.Application.Common;

namespace Odin.Auth.UnitTests.Application.Common
{
    [Collection(nameof(CommonServiceTestFixtureCollection))]
    public class CommonServiceTest
    {
        private readonly CommonServiceTestFixture _fixture;
        private readonly AmazonCognitoIdentityRepositoryMock _awsIdentityRepository;

        public CommonServiceTest(CommonServiceTestFixture fixture)
        {
            _fixture = fixture;
            _awsIdentityRepository = _fixture.GetAwsIdentityRepository();
        }


        [Fact(DisplayName = "AuthenticateUserAsync() should authenticate valid username and password")]
        [Trait("Application", "CommonService / CommonService")]
        public async Task AuthenticateUserAsync_ShouldAuthenticateValidUser()
        {
            var app = new App.CommonService(_fixture.AppSettings, _awsIdentityRepository);
            var output = await app.AuthenticateUserAsync("unit.testing", "password", CancellationToken.None);

            output.AccessToken.Should().Be("access-token");
            output.ExpiresIn.Should().Be(3600);
            output.IdToken.Should().Be("id-token");
            output.RefreshToken.Should().Be("refresh-token");
            output.TokenType.Should().Be("Bearer");
        }

        [Fact(DisplayName = "AuthenticateUserAsync() should throw an error when user not found")]
        [Trait("Application", "CommonService / CommonService")]
        public void AuthenticaUserAsync_ShouldThrowErrorWhenUserNotFound()
        {
            var app = new App.CommonService(_fixture.AppSettings, _awsIdentityRepository);
            var ex = Assert.Throws<AggregateException>(() => app.AuthenticateUserAsync("user.not.found", "password", CancellationToken.None).Result);

            ex.Message.Should().Contain("User not found");
        }

        [Fact(DisplayName = "AuthenticateUserAsync() should throw an error when user not confirmed")]
        [Trait("Application", "CommonService / CommonService")]
        public void AuthenticaUserAsync_ShouldThrowErrorWhenUserNotConfirmed()
        {
            var app = new App.CommonService(_fixture.AppSettings, _awsIdentityRepository);
            var ex = Assert.Throws<AggregateException>(() => app.AuthenticateUserAsync("user.not.confirmed", "password", CancellationToken.None).Result);

            ex.Message.Should().Contain("User not confirmed");
        }

        [Fact(DisplayName = "AuthenticateUserAsync() should throw an error when user not authorized")]
        [Trait("Application", "CommonService / CommonService")]
        public void AuthenticaUserAsync_ShouldThrowErrorWhenUserNotAuthorized()
        {
            var app = new App.CommonService(_fixture.AppSettings, _awsIdentityRepository);
            var ex = Assert.Throws<AggregateException>(() => app.AuthenticateUserAsync("user.not.authenticated", "password", CancellationToken.None).Result);

            ex.Message.Should().Contain("User not authenticated");
        }


        [Fact(DisplayName = "GetUserByUsernameAsync() should return OK with valid data")]
        [Trait("Application", "CommonService / CommonService")]
        public async Task GetUserByUsernameAsync_OK()
        {
            var app = new App.CommonService(_fixture.AppSettings, _awsIdentityRepository);
            var output = await app.GetUserByUsernameAsync("unit.testing", CancellationToken.None);

            output.Username.Should().Be("unit.testing");
            output.EmailAddress.Should().Be("unit.testing@email.com");
        }

        [Fact(DisplayName = "GetUserByUsernameAsync() should throw UserNotFoundException when user does not exist")]
        [Trait("Application", "CommonService / CommonService")]
        public void GetUserByUsernameAsync_throws_UserNotFoundException()
        {
            var app = new App.CommonService(_fixture.AppSettings, _awsIdentityRepository);
            var ex = Assert.Throws<AggregateException>(() => app.GetUserByUsernameAsync("user.not.found", CancellationToken.None).Result);

            ex.Message.Should().Contain("User not found");
        }
    }
}
