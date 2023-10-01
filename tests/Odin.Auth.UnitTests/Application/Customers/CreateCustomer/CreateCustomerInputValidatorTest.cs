using FluentAssertions;
using FluentValidation;
using Odin.Auth.Application.Customers.CreateCustomer;

namespace Odin.Auth.UnitTests.Application.Customers.CreateCustomer
{
    [Collection(nameof(CreateCustomerTestFixtureCollection))]
    public class CreateCustomerInputValidatorTest
    {
        private readonly CreateCustomerTestFixture _fixture;

        public CreateCustomerInputValidatorTest(CreateCustomerTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "Validate() should not validate when Name is empty")]
        [Trait("Application", "Customers / CreateCustomerInputValidator")]
        public void DontValidateWhenEmptyName()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetCreateCustomerInputWithEmptyName();
            var validator = new CreateCustomerInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Name' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when Document is empty")]
        [Trait("Application", "Customers / CreateCustomerInputValidator")]
        public void DontValidateWhenEmptyDocument()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetCreateCustomerInputWithEmptyDocument();
            var validator = new CreateCustomerInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Document' must be a valid CPF or CNPJ");
        }

        [Fact(DisplayName = "Validate() should validate with valid data")]
        [Trait("Application", "Customers / CreateCustomerInputValidator")]
        public void ValidateWhenValid()
        {
            var input = _fixture.GetValidCreateCustomerInputWithoutAddress();
            var validator = new CreateCustomerInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeTrue();
            validateResult.Errors.Should().HaveCount(0);
        }
    }
}
