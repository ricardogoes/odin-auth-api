using FluentAssertions;
using FluentValidation;
using Odin.Auth.Application.Customers.ChangeAddressCustomer;

namespace Odin.Auth.UnitTests.Application.Customers.ChangeAddressCustomer
{
    [Collection(nameof(ChangeAddressCustomerTestFixtureCollection))]
    public class ChangeAddressCustomerInputValidatorTest
    {
        private readonly ChangeAddressCustomerTestFixture _fixture;

        public ChangeAddressCustomerInputValidatorTest(ChangeAddressCustomerTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "Validate() should not validate when CustomerId is empty")]
        [Trait("Application", "Customers / ChangeAddressCustomerInputValidator")]
        public void DontValidateWhenEmptyId()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetInputAddressWithCustomerIdEmpty();
            var validator = new ChangeAddressCustomerInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Customer Id' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when street name is empty")]
        [Trait("Application", "Customers / ChangeAddressCustomerInputValidator")]
        public void DontValidateWhenEmptyStreetName()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetInputAddressWithStreetNameEmpty();
            var validator = new ChangeAddressCustomerInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Street Name' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when street number is invalid")]
        [Trait("Application", "Customers / ChangeAddressCustomerInputValidator")]
        public void DontValidateWhenEmptyStreetNumber()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetInputAddressWithStreetNumberEmpty();
            var validator = new ChangeAddressCustomerInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Street Number' must be greater than '0'.");
        }

        [Fact(DisplayName = "Validate() should not validate when neighborhood is empty")]
        [Trait("Application", "Customers / ChangeAddressCustomerInputValidator")]
        public void DontValidateWhenEmptyNeighborhood()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetInputAddressWithNeighborhoodEmpty();
            var validator = new ChangeAddressCustomerInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Neighborhood' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when zip code is empty")]
        [Trait("Application", "Customers / ChangeAddressCustomerInputValidator")]
        public void DontValidateWhenEmptyZipCode()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetInputAddressWithZipCodeEmpty();
            var validator = new ChangeAddressCustomerInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Zip Code' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when city is empty")]
        [Trait("Application", "Customers / ChangeAddressCustomerInputValidator")]
        public void DontValidateWhenEmptyCity()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetInputAddressWithCityEmpty();
            var validator = new ChangeAddressCustomerInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'City' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when state is empty")]
        [Trait("Application", "Customers / ChangeAddressCustomerInputValidator")]
        public void DontValidateWhenEmptyState()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetInputAddressWithStateEmpty();
            var validator = new ChangeAddressCustomerInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'State' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should validate with valid data")]
        [Trait("Application", "Customers / ChangeAddressCustomerInputValidator")]
        public void ValidateWhenValid()
        {
            var input = _fixture.GetValidInputAddress();
            var validator = new ChangeAddressCustomerInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeTrue();
            validateResult.Errors.Should().HaveCount(0);
        }
    }
}
