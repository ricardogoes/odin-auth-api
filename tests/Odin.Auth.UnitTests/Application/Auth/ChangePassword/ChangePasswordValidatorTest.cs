using FluentAssertions;
using FluentValidation;
using Odin.Auth.Application.Auth.ChangePassword;

namespace Odin.Auth.UnitTests.Application.Auth.ChangePassword
{
    [Collection(nameof(ChangePasswordTestFixtureCollection))]
    public class ChangePasswordInputValidatorTest
    {
        private readonly ChangePasswordTestFixture _fixture;

        public ChangePasswordInputValidatorTest(ChangePasswordTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "Validate() should not validate when userId is empty")]
        [Trait("Application", "ChangePassword / ChangePasswordInputValidator")]
        public void DontValidateWhenEmptyUserId()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetInputWithEmptyUserId();

            var validator = new ChangePasswordInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'User Id' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when new password is empty")]
        [Trait("Application", "ChangePassword / ChangePasswordInputValidator")]
        public void DontValidateWhenEmptyNewPassword()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetInputWithEmptyNewPassword();

            var validator = new ChangePasswordInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'New Password' must not be empty.");
        }  
    }
}

