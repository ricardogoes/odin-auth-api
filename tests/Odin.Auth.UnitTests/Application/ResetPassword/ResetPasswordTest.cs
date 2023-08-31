using FluentAssertions;
using Moq;
using Odin.Auth.Application.Common;
using Odin.Auth.Infra.Cognito;
using App = Odin.Auth.Application.ResetPassword;

namespace Odin.Auth.UnitTests.Application.ResetPassword
{
    [Collection(nameof(ResetPasswordTestFixtureCollection))]
    public class ResetPasswordTest
    {
        private readonly ResetPasswordTestFixture _fixture;
        private readonly AmazonCognitoIdentityRepositoryMock _awsIdentityRepository;

        public ResetPasswordTest(ResetPasswordTestFixture fixture)
        {
            _fixture = fixture;
            _awsIdentityRepository = _fixture.GetAwsIdentityRepository();
        }

        [Fact(DisplayName = "Handle() should return OK with valid data")]
        [Trait("Application", "ResetPassword / ResetPassword")]
        public async Task TTryResetPasswordWithConfirmationCodeAsync_Ok()
        {
            var app = new App.ResetPassword(_fixture.AppSettings, _awsIdentityRepository);
            var response = await app.Handle(new App.ResetPasswordInput
            {
                UserId = "unit.testing",
                Username = "unit.testing",
                ConfirmationCode = "OK",
                NewPassword = "123123"
            }, CancellationToken.None);

            response.Username. Should().Be("unit.testing");
        }

        [Fact(DisplayName = "Handle() should throw Exception when the API return an error")]
        [Trait("Application", "ResetPassword / ResetPassword")]
        public void TryResetPasswordWithConfirmationCodeAsync_CantResetPassword()
        {
            var app = new App.ResetPassword(_fixture.AppSettings, _awsIdentityRepository);
            var ex = Assert.Throws<AggregateException>(() => app.Handle(new App.ResetPasswordInput
            {
                UserId = "user.with.error",
                Username = "unit.testing",
                ConfirmationCode = "OK",
                NewPassword = "123123"
            }, CancellationToken.None).Result);

            ex.Message.Should().Contain("Can't reset password");
        }
    }
}
