using FluentAssertions;
using FluentValidation;
using Odin.Auth.Application.Users.ChangeStatusUser;
using Odin.Auth.Domain.Enums;

namespace Odin.Auth.UnitTests.Application.Users.ChangeStatusUser
{
    [Collection(nameof(ChangeStatusUserTestFixtureCollection))]
    public class ChangeStatusUserInputValidatorTest
    {
        private readonly ChangeStatusUserTestFixture _fixture;

        public ChangeStatusUserInputValidatorTest(ChangeStatusUserTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "Validate() should not validate when userId is empty")]
        [Trait("Application", "ChangeStatusUser / ChangeStatusUserInputValidator")]
        public void DontValidateWhenEmptyUserId()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = new ChangeStatusUserInput
            (
                tenantId: Guid.NewGuid(),
                id: Guid.Empty,
                action: ChangeStatusAction.ACTIVATE,
                loggedUsername: "admin"
            );

            var validator = new ChangeStatusUserInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'User Id' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when action is empty")]
        [Trait("Application", "ChangeStatusUser / ChangeStatusUserInputValidator")]
        public void DontValidateWhenEmptyAction()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = new ChangeStatusUserInput
            (
                tenantId: Guid.NewGuid(),
                id: Guid.NewGuid(),
                action: null,
                loggedUsername: "admin"
            );

            var validator = new ChangeStatusUserInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Action' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when tenantId is empty")]
        [Trait("Application", "ChangeStatusUser / ChangeStatusUserInputValidator")]
        public void DontValidateWhenEmptyTenantId()
        {
            var input = new ChangeStatusUserInput
            (
                tenantId: Guid.Empty,
                id: Guid.NewGuid(),
                action: ChangeStatusAction.ACTIVATE,
                loggedUsername: "admin"
            );

            var validator = new ChangeStatusUserInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Tenant Id' must not be empty.");
        }
    }
}
