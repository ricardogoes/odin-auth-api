using FluentAssertions;
using FluentValidation;
using Odin.Auth.Application.Login;

namespace Odin.Auth.UnitTests.Application.Login
{
    [Collection(nameof(LoginTestFixtureCollection))]
    public class LoginInputValidatorTest
    {
        private readonly LoginTestFixture _fixture;

        public LoginInputValidatorTest(LoginTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "Validate() should not validate when username is empty")]
        [Trait("Application", "Login / LoginInputValidator")]
        public void DontValidateWhenEmptyFirstName()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = new LoginInput("", "unit.testing");

            var validator = new LoginInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Username' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when password is empty")]
        [Trait("Application", "Login / LoginInputValidator")]
        public void DontValidateWhenEmptyLastName()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = new LoginInput(_fixture.Faker.Person.UserName, "");

            var validator = new LoginInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Password' must not be empty.");
        }
    }
}
