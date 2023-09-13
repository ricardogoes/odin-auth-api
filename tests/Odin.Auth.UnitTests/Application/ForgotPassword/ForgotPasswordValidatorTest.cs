using FluentAssertions;
using FluentValidation;
using App = Odin.Auth.Application.ForgotPassword;

namespace Odin.Auth.UnitTests.Application.ForgotPassword
{
    [Collection(nameof(ForgotPasswordTestFixtureCollection))]
    public class ForgotPasswordInputValidatorTest
    {
        private readonly ForgotPasswordTestFixture _fixture;

        public ForgotPasswordInputValidatorTest(ForgotPasswordTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "Validate() should not validate when username is empty")]
        [Trait("Application", "ForgotPassword / ForgotPasswordInputValidator")]
        public void DontValidateWhenEmptyFirstName()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = new App.ForgotPasswordInput("");

            var validator = new App.ForgotPasswordInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Username' must not be empty.");
        }
    }
}
