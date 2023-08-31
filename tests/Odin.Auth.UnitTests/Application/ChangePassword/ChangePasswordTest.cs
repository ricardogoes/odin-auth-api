using Amazon.CognitoIdentityProvider.Model;
using FluentAssertions;
using Moq;
using Odin.Auth.Application.Common;
using Odin.Auth.Infra.Cognito;
using App = Odin.Auth.Application.ChangePassword;

namespace Odin.Auth.UnitTests.Application.ChangePassword
{
    [Collection(nameof(ChangePasswordTestFixtureCollection))]
    public class ChangePasswordTest
    {
        private readonly ChangePasswordTestFixture _fixture;
        private readonly AmazonCognitoIdentityRepositoryMock _awsIdentityRepository;
        private readonly Mock<ICommonService> _commonServiceMock;

        public ChangePasswordTest(ChangePasswordTestFixture fixture)
        {
            _fixture = fixture;
            _awsIdentityRepository = _fixture.GetAwsIdentityRepository();
            _commonServiceMock = new();
        }

        [Fact(DisplayName = "Handle() should change password with valid data")]
        [Trait("Application", "ChangePassword / ChangePassword")]
        public async Task ShouldChangePassword()
        {
            var expectedResult = new AuthenticationResultType
            {
                AccessToken = "access-token",
                ExpiresIn = 3600,
                IdToken = "id-token",
                RefreshToken = "refresh-token",
                TokenType = "Bearer"
            };

            _commonServiceMock.Setup(s => s.AuthenticateUserAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(expectedResult));

            var app = new App.ChangePassword(_commonServiceMock.Object, _awsIdentityRepository);
            var response = await app.Handle(new App.ChangePasswordInput
            {
                Username = "unit.testing",
                NewPassword = "new_password",
                CurrentPassword = "current_password"
            }, CancellationToken.None);

            response.Username.Should().Be("unit.testing");
        }
    }
}
