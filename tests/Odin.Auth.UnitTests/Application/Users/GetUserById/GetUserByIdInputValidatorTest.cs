using FluentAssertions;
using FluentValidation;
using App = Odin.Auth.Application.Users.GetUserById;

namespace Odin.Auth.UnitTests.Application.Users.GetUserById
{
    [Collection(nameof(GetUserByIdTestFixture))]
    public class GetUserByIdInputValidatorTest
    {
        private readonly GetUserByIdTestFixture _fixture;

        public GetUserByIdInputValidatorTest(GetUserByIdTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "Validate() should not validate when Id is empty")]
        [Trait("Application", "GetUserById / GetUserByIdInputValidator")]
        public void DontValidateWhenEmptyId()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetValidGetUserByIdInput(Guid.Empty);
            var validator = new App.GetUserByIdInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'User Id' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should validate with valid data")]
        [Trait("Application", "GetUserById / GetUserByIdInputValidator")]
        public void ValidateWhenValid()
        {
            var input = _fixture.GetValidGetUserByIdInput();
            var validator = new App.GetUserByIdInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeTrue();
            validateResult.Errors.Should().HaveCount(0);
        }
    }
}
