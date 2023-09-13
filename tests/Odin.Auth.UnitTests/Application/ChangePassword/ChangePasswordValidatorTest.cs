using FluentAssertions;
using FluentValidation;
using App = Odin.Auth.Application.ChangePassword;

namespace Odin.Auth.UnitTests.Application.ChangePassword
{
    [Collection(nameof(ChangePasswordTestFixtureCollection))]
    public class ChangePasswordInputValidatorTest
    {
        private readonly ChangePasswordTestFixture _fixture;

        public ChangePasswordInputValidatorTest(ChangePasswordTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "Validate() should not validate when username is empty")]
        [Trait("Application", "ChangePassword / ChangePasswordInputValidator")]
        public void DontValidateWhenEmptyFirstName()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = new App.ChangePasswordInput
            (
                username: "",
                currentPassword: "unit.testing",
                newPassword: "new.password"
            );

            var validator = new App.ChangePasswordInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Username' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when current password is empty")]
        [Trait("Application", "ChangePassword / ChangePasswordInputValidator")]
        public void DontValidateWhenEmptyLastName()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = new App.ChangePasswordInput
            (
                username: "unit.testing",
                currentPassword: "",
                newPassword: "new.password"
            );

            var validator = new App.ChangePasswordInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Current Password' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when new password is empty")]
        [Trait("Application", "ChangePassword / ChangePasswordInputValidator")]
        public void DontValidateWhenEmptyEmailAddress()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = new App.ChangePasswordInput
            (
                username: "unit.testing",
                currentPassword: "password",
                newPassword: ""
            );

            var validator = new App.ChangePasswordInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'New Password' must not be empty.");
        }
    }
}
