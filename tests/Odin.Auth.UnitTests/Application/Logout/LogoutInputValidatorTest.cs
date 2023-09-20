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

        [Fact(DisplayName = "Validate() should not validate when userId is empty")]
        [Trait("Application", "Logout / LogoutInputValidator")]
        public void DontValidateWhenEmptyUserId()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = new LogoutInput(Guid.Empty);

            var validator = new LogoutInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'User Id' must not be empty.");
        }
    }
}
