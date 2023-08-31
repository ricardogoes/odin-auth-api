using FluentAssertions;
using FluentValidation;
using Odin.Auth.Application.Logout;

namespace Odin.Auth.UnitTests.Application.Logout
{
    [Collection(nameof(LogoutTestFixtureCollection))]
    public class LogoutInputValidatorTest
    {
        private readonly LogoutTestFixture _fixture;

        public LogoutInputValidatorTest(LogoutTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "Validate() should not validate when username is empty")]
        [Trait("Application", "Logout / LogoutInputValidator")]
        public void DontValidateWhenEmptyFirstName()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = new LogoutInput
            {
                Username = "",
                AccessToken = "unit.testing.token"
            };

            var validator = new LogoutInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Username' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when access token is empty")]
        [Trait("Application", "Logout / LogoutInputValidator")]
        public void DontValidateWhenEmptyLastName()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = new LogoutInput
            {
                Username = _fixture.Faker.Person.UserName,
                AccessToken = ""
            };

            var validator = new LogoutInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Access Token' must not be empty.");
        }
    }
}
