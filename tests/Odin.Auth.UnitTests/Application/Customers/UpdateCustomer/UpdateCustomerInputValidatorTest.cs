using FluentAssertions;
using FluentValidation;
using Odin.Auth.Application.Customers.UpdateCustomer;

namespace Odin.Auth.UnitTests.Application.Customers.UpdateCustomer
{
    [Collection(nameof(UpdateCustomerTestFixtureCollection))]
    public class UpdateCustomerInputValidatorTest
    {
        private readonly UpdateCustomerTestFixture _fixture;

        public UpdateCustomerInputValidatorTest(UpdateCustomerTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "Validate() should not validate when Id is empty")]
        [Trait("Application", "Customers / UpdateCustomerInputValidator")]
        public void DontValidateWhenEmptyId()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetValidUpdateCustomerInput(Guid.Empty);
            var validator = new UpdateCustomerInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Id' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when Name is empty")]
        [Trait("Application", "Customers / UpdateCustomerInputValidator")]
        public void DontValidateWhenEmptyName()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetUpdateCustomerInputWithEmptyName();
            var validator = new UpdateCustomerInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Name' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when Document is empty")]
        [Trait("Application", "Customers / UpdateCustomerInputValidator")]
        public void DontValidateWhenEmptyDocument()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetUpdateCustomerInputWithEmptyDocument();
            var validator = new UpdateCustomerInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Document' must be a valid CPF or CNPJ");
        }

        [Fact(DisplayName = "Validate() should validate with valid data")]
        [Trait("Application", "Customers / UpdateCustomerInputValidator")]
        public void ValidateWhenValid()
        {
            var input = _fixture.GetValidUpdateCustomerInput();
            var validator = new UpdateCustomerInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeTrue();
            validateResult.Errors.Should().HaveCount(0);
        }
    }
}
