using FluentAssertions;
using FluentValidation;
using Odin.Auth.Application.ChangeStatusUser;

namespace Odin.Auth.UnitTests.Application.ChangeStatusUser
{
    [Collection(nameof(ChangeStatusUserTestFixtureCollection))]
    public class ChangeStatusUserInputValidatorTest
    {
        private readonly ChangeStatusUserTestFixture _fixture;

        public ChangeStatusUserInputValidatorTest(ChangeStatusUserTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "Validate() should not validate when username is empty")]
        [Trait("Application", "ChangeStatusUser / ChangeStatusUserInputValidator")]
        public void DontValidateWhenEmptyUsername()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = new ChangeStatusUserInput
            (
                username: "",
                action: ChangeStatusAction.ACTIVATE
            );

            var validator = new ChangeStatusUserInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Username' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when action is empty")]
        [Trait("Application", "ChangeStatusUser / ChangeStatusUserInputValidator")]
        public void DontValidateWhenEmptyAction()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = new ChangeStatusUserInput
            (
                username: _fixture.Faker.Person.UserName,
                action: null
            );

            var validator = new ChangeStatusUserInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Action' must not be empty.");
        }
    }
}
