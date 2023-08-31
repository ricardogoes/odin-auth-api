using FluentAssertions;
using Odin.Auth.Application.Logout;
using Odin.Auth.Infra.Cognito;
using App = Odin.Auth.Application.Logout;

namespace Odin.Auth.UnitTests.Application.Logout
{
    [Collection(nameof(LogoutTestFixtureCollection))]
    public class LogoutTest
    {
        private readonly LogoutTestFixture _fixture;
        private readonly AmazonCognitoIdentityRepositoryMock _awsIdentityRepository;

        public LogoutTest(LogoutTestFixture fixture)
        {
            _fixture = fixture;
            _awsIdentityRepository = _fixture.GetAwsIdentityRepository();
        }

        [Fact(DisplayName = "Handle() should return OK with valid data")]
        [Trait("Application", "Logout / Logout")]
        public async Task TryLogOutAsync_Ok()
        {
            var app = new App.Logout(_awsIdentityRepository);
            var response = await app.Handle(new LogoutInput
            {
                Username = "unit.testing",
                AccessToken = "access-token"
            }, CancellationToken.None);

            response.Username.Should().Be("unit.testing");
            response.Message.Should().Be("User 'unit.testing' logged out successfully");
        }
    }
}
