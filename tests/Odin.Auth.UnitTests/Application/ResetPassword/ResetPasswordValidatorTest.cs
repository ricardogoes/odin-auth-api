using FluentAssertions;
using FluentValidation;
using App = Odin.Auth.Application.ResetPassword;

namespace Odin.Auth.UnitTests.Application.ResetPassword
{
    [Collection(nameof(ResetPasswordTestFixtureCollection))]
    public class ResetPasswordInputValidatorTest
    {
        private readonly ResetPasswordTestFixture _fixture;

        public ResetPasswordInputValidatorTest(ResetPasswordTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "Validate() should not validate when user id is empty")]
        [Trait("Application", "ResetPassword / ResetPasswordInputValidator")]
        public void DontValidateWhenEmptyUserId()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = new App.ResetPasswordInput
            {
                UserId = "",
                Username = _fixture.Faker.Person.UserName,
                NewPassword = "new.password",
                ConfirmationCode = "confirmation.code"               
            };

            var validator = new App.ResetPasswordInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'User Id' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when username is empty")]
        [Trait("Application", "ResetPassword / ResetPasswordInputValidator")]
        public void DontValidateWhenEmptyUsername()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = new App.ResetPasswordInput
            {
                UserId = _fixture.Faker.Person.UserName,
                Username = "",
                NewPassword = "new.password",
                ConfirmationCode = "confirmation.code"
            };

            var validator = new App.ResetPasswordInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Username' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when new password is empty")]
        [Trait("Application", "ResetPassword / ResetPasswordInputValidator")]
        public void DontValidateWhenEmptyNewPassword()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = new App.ResetPasswordInput
            {
                UserId = _fixture.Faker.Person.UserName,
                Username = _fixture.Faker.Person.UserName,
                NewPassword = "",
                ConfirmationCode = "confirmation.code"
            };

            var validator = new App.ResetPasswordInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'New Password' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when confirmatio code is empty")]
        [Trait("Application", "ResetPassword / ResetPasswordInputValidator")]
        public void DontValidateWhenEmptyConfirmationCode()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = new App.ResetPasswordInput
            {
                UserId = _fixture.Faker.Person.UserName,
                Username = _fixture.Faker.Person.UserName,
                NewPassword = "new.password",
                ConfirmationCode = ""
            };

            var validator = new App.ResetPasswordInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Confirmation Code' must not be empty.");
        }
    }
}
