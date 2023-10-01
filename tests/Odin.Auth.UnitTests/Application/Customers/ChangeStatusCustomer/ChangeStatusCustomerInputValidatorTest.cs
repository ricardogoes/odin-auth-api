using FluentAssertions;
using FluentValidation;
using Odin.Auth.Application.Customers.ChangeStatusCustomer;

namespace Odin.Auth.UnitTests.Application.Customers.ChangeStatusCustomer
{
    [Collection(nameof(ChangeStatusCustomerTestFixtureCollection))]
    public class ChangeStatusCustomerInputValidatorTest
    {
        private readonly ChangeStatusCustomerTestFixture _fixture;

        public ChangeStatusCustomerInputValidatorTest(ChangeStatusCustomerTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "Validate() should not validate when Id is empty")]
        [Trait("Application", "Customers / ChangeStatusCustomerInputValidator")]
        public void DontValidateWhenEmptyId()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetValidChangeStatusCustomerInputToActivate(Guid.Empty);
            var validator = new ChangeStatusCustomerInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Id' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when Action is empty")]
        [Trait("Application", "Customers / ChangeStatusCustomerInputValidator")]
        public void DontValidateWhenEmptyAction()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetChangeStatusCustomerInputWithEmptyAction();
            var validator = new ChangeStatusCustomerInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Action' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should validate with valid data")]
        [Trait("Application", "Customers / ChangeStatusCustomerInputValidator")]
        public void ValidateWhenValid()
        {
            var input = _fixture.GetValidChangeStatusCustomerInputToActivate();
            var validator = new ChangeStatusCustomerInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeTrue();
            validateResult.Errors.Should().HaveCount(0);
        }
    }
}
