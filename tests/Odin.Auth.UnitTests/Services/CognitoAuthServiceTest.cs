using FluentAssertions;
using Odin.Auth.Domain.Interfaces.Services;
using Odin.Auth.Domain.Models.ChangePassword;
using Odin.Auth.Domain.Models.UpdateProfile;
using Odin.Auth.Domain.Models.UserLogin;
using Odin.Auth.Service.ExternalServices;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace Odin.Auth.UnitTests.Services
{
    public class CognitoAuthServiceTest : TestBed<TestFixture>
    {
        public CognitoAuthServiceTest(ITestOutputHelper testOutputHelper, TestFixture fixture)
            : base(testOutputHelper, fixture)
        {
        }

        [Fact(DisplayName = "TryChangePasswordAsync() should return OK with valid data")]
        [Trait("Category", "Cognito Auth Service Tests")]
        public async Task TryChangePasswordAsync_Ok()
        {
            var cognitoAuthService = _fixture.GetService<ICognitoAuthService>(_testOutputHelper);
            var response = await cognitoAuthService.TryChangePasswordAsync(new ChangePasswordRequest
            {
                Username = "unit.testing",
                NewPassword = "new_password",
                CurrentPassword = "current_password"
            });

            response.Username.Should().Be("unit.testing");
        }

        [Fact(DisplayName = "TryLoginAsync() should return OK with valid data")]
        [Trait("Category", "Cognito Auth Service Tests")]
        public async Task TryLoginAsync_Ok()
        {
            var cognitoAuthService = _fixture.GetService<ICognitoAuthService>(_testOutputHelper);
            var response = await cognitoAuthService.TryLoginAsync(new UserAuthRequest
            {
                Username = "unit.testing",
                Password = "password"
            });

            response.Username.Should().Be("unit.testing");
        }

        [Fact(DisplayName = "TryLoginAsync() should return not authenticated when invalid user or password")]
        [Trait("Category", "Cognito Auth Service Tests")]
        public async Task TryLoginAsync_UserNotAuthenticated()
        {
            var cognitoAuthService = _fixture.GetService<ICognitoAuthService>(_testOutputHelper);
            var response = await cognitoAuthService.TryLoginAsync(new UserAuthRequest
            {
                Username = "user.not.authenticated",
                Password = "password"
            });

            response.Username.Should().BeEmpty();
            response.Message.Should().Be("Incorrect username or password");
        }

        [Fact(DisplayName = "TryLoginAsync() should return not found when user not found")]
        [Trait("Category", "Cognito Auth Service Tests")]
        public async Task TryLoginAsync_UserNotFound()
        {
            var cognitoAuthService = _fixture.GetService<ICognitoAuthService>(_testOutputHelper);
            var response = await cognitoAuthService.TryLoginAsync(new UserAuthRequest
            {
                Username = "user.not.found",
                Password = "password"
            });

            response.Username.Should().BeEmpty();
            response.Message.Should().Be("User not found");
        }

        [Fact(DisplayName = "TryLoginAsync() should resend confirmation code when user not confirmed")]
        [Trait("Category", "Cognito Auth Service Tests")]
        public async Task TryLoginAsync_ResendConfirmationCode()
        {
            var cognitoAuthService = _fixture.GetService<ICognitoAuthService>(_testOutputHelper);
            var response = await cognitoAuthService.TryLoginAsync(new UserAuthRequest
            {
                Username = "user.not.confirmed",
                Password = "password"
            });

            response.Username.Should().Be("user.not.confirmed");
            response.Message.Should().Contain("Confirmation Code sent");
        }

        [Fact(DisplayName = "TryLoginAsync() should not resend confirmation code when an error ocurred")]
        [Trait("Category", "Cognito Auth Service Tests")]
        public async Task TryLoginAsync_NotResendConfirmationCodeWhenError()
        {
            var cognitoAuthService = _fixture.GetService<ICognitoAuthService>(_testOutputHelper);
            var response = await cognitoAuthService.TryLoginAsync(new UserAuthRequest
            {
                Username = "user.not.confirmed.not-sent",
                Password = "password"
            });

            response.Username.Should().Be("user.not.confirmed.not-sent");
            response.Message.Should().Contain("Resend Confirmation Code Response");
        }

        [Fact(DisplayName = "TryLogOutAsync() should return OK with valid data")]
        [Trait("Category", "Cognito Auth Service Tests")]
        public async Task TryLogOutAsync_Ok()
        {
            var cognitoAuthService = _fixture.GetService<ICognitoAuthService>(_testOutputHelper);
            var response = await cognitoAuthService.TryLogOutAsync(new UserSignOutRequest
            {
                Username = "unit.testing",
                AccessToken = "access-token"
            });

            response.Should().BeTrue();
        }

        [Fact(DisplayName = "TryInitForgotPasswordAsync() should return OK with valid data")]
        [Trait("Category", "Cognito Auth Service Tests")]
        public async Task TryInitForgotPasswordAsync_Ok()
        {
            var cognitoAuthService = _fixture.GetService<ICognitoAuthService>(_testOutputHelper);
            var response = await cognitoAuthService.TryInitForgotPasswordAsync(new Domain.Models.ForgotPassword.ForgotPasswordRequest
            {
                Username = "unit.testing"
            });

            response.Username.Should().Be("unit.testing");
        }

        [Fact(DisplayName = "TryInitForgotPasswordAsync() should return not found when user does not exist")]
        [Trait("Category", "Cognito Auth Service Tests")]
        public async Task TryInitForgotPasswordAsync_UserNotFound()
        {
            var cognitoAuthService = _fixture.GetService<ICognitoAuthService>(_testOutputHelper);
            var response = await cognitoAuthService.TryInitForgotPasswordAsync(new Domain.Models.ForgotPassword.ForgotPasswordRequest
            {
                Username = "user.not.found"
            });

            response.Username.Should().BeEmpty();
            response.Message.Should().Be("Error trying to recover user data");
        }

        [Fact(DisplayName = "TryInitForgotPasswordAsync() should return not found when username does not exist")]
        [Trait("Category", "Cognito Auth Service Tests")]
        public async Task TryInitForgotPasswordAsync_UserFilteredNotFound()
        {
            var cognitoAuthService = _fixture.GetService<ICognitoAuthService>(_testOutputHelper);
            var response = await cognitoAuthService.TryInitForgotPasswordAsync(new Domain.Models.ForgotPassword.ForgotPasswordRequest
            {
                Username = "user.filtered.not.found"
            });

            response.Username.Should().Be("user.filtered.not.found");
            response.Message.Should().Be("No users with the given username found");
        }

        [Fact(DisplayName = "TryInitForgotPasswordAsync() should not send confirmation code when an error ocurred")]
        [Trait("Category", "Cognito Auth Service Tests")]
        public async Task TryInitForgotPasswordAsync_NotSendConfirmationCodeWhenError()
        {
            var cognitoAuthService = _fixture.GetService<ICognitoAuthService>(_testOutputHelper);
            var response = await cognitoAuthService.TryInitForgotPasswordAsync(new Domain.Models.ForgotPassword.ForgotPasswordRequest
            {
                Username = "user.with.error"
            });

            response.Username.Should().Be("user.with.error");
            response.Message.Should().Be("ListUsers Response: BadRequest");
        }

        [Fact(DisplayName = "TryResetPasswordWithConfirmationCodeAsync() should return OK with valid data")]
        [Trait("Category", "Cognito Auth Service Tests")]
        public async Task TTryResetPasswordWithConfirmationCodeAsync_Ok()
        {
            var cognitoAuthService = _fixture.GetService<ICognitoAuthService>(_testOutputHelper);
            var response = await cognitoAuthService.TryResetPasswordWithConfirmationCodeAsync(new Domain.Models.ResetPassword.ResetPasswordRequest
            {
                UserId = "unit.testing",
                Username = "unit.testing",
                ConfirmationCode = "OK",
                NewPassword = "123123"
            });

            response.Should().BeTrue();
        }

        [Fact(DisplayName = "TryResetPasswordWithConfirmationCodeAsync() should throw Exception when the API return an error")]
        [Trait("Category", "Cognito Auth Service Tests")]
        public void TryResetPasswordWithConfirmationCodeAsync_CantResetPassword()
        {
            var cognitoAuthService = _fixture.GetService<ICognitoAuthService>(_testOutputHelper);

            var ex = Assert.Throws<AggregateException>(() => cognitoAuthService.TryResetPasswordWithConfirmationCodeAsync(new Domain.Models.ResetPassword.ResetPasswordRequest
            {
                UserId = "user.with.error",
                Username = "unit.testing",
                ConfirmationCode = "OK",
                NewPassword = "123123"
            }).Result);

            ex.Message.Should().Contain("Can't reset password");
        }
    }
}
