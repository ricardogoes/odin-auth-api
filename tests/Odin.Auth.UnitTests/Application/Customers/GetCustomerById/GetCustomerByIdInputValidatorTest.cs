using FluentAssertions;
using FluentValidation;
using Odin.Auth.Application.Customers.GetCustomerById;

namespace Odin.Auth.UnitTests.Application.Customers.GetCustomerById
{
    [Collection(nameof(GetCustomerByIdTestFixture))]
    public class GetCustomerByIdInputValidatorTest
    {
        private readonly GetCustomerByIdTestFixture _fixture;

        public GetCustomerByIdInputValidatorTest(GetCustomerByIdTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "Validate() should not validate when Id is empty")]
        [Trait("Application", "Customers / GetCustomerByIdInputValidator")]
        public void DontValidateWhenEmptyId()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetValidGetCustomerByIdInput(Guid.Empty);
            var validator = new GetCustomerByIdInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Id' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should validate with valid data")]
        [Trait("Application", "Customers / GetCustomerByIdInputValidator")]
        public void ValidateWhenValid()
        {
            var input = _fixture.GetValidGetCustomerByIdInput();
            var validator = new GetCustomerByIdInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeTrue();
            validateResult.Errors.Should().HaveCount(0);
        }
    }
}
