using FluentAssertions;
using FluentValidation;
using Odin.Auth.Application.Users.CreateUser;
using Odin.Auth.Application.Users.UpdateProfile;

namespace Odin.Auth.UnitTests.Application.Users.UpdateProfile
{
    [Collection(nameof(UpdateProfileTestFixtureCollection))]
    public class UpdateProfileInputValidatorTest
    {
        private readonly UpdateProfileTestFixture _fixture;

        public UpdateProfileInputValidatorTest(UpdateProfileTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "Validate() should not validate when first name is empty")]
        [Trait("Application", "UpdateProfile / UpdateProfileInputValidator")]
        public void DontValidateWhenEmptyFirstName()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetInputWithEmptyFirstName();

            var validator = new UpdateProfileInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'First Name' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when last name is empty")]
        [Trait("Application", "UpdateProfile / UpdateProfileInputValidator")]
        public void DontValidateWhenEmptyLastName()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetInputWithEmptyLastName();

            var validator = new UpdateProfileInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Last Name' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when email address is empty")]
        [Trait("Application", "UpdateProfile / UpdateProfileInputValidator")]
        public void DontValidateWhenEmptyEmailAddress()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetInputWithEmptyEmail();

            var validator = new UpdateProfileInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Email' is not a valid email address.");
        }

        [Theory(DisplayName = "Validate() should not validate when email address is invalid")]
        [Trait("Application", "UpdateProfile / UpdateProfileInputValidator")]
        [InlineData("email.com")]
        [InlineData("email@")]
        public void DontValidateWhenInvalidEmailAddress(string email)
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = new UpdateProfileInput
            (
                userId: Guid.NewGuid(),
                firstName: _fixture.Faker.Person.FirstName,
                lastName: _fixture.Faker.Person.LastName,
                email: email,
                groups: new List<string> { "role-01" }
            );

            var validator = new UpdateProfileInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Email' is not a valid email address.");
        }

        [Fact(DisplayName = "Validate() should not validate when userId is empty")]
        [Trait("Application", "UpdateProfile / UpdateProfileInputValidator")]
        public void DontValidateWhenEmptyUserId()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = new UpdateProfileInput
            (
                userId: Guid.Empty,
                firstName: _fixture.Faker.Person.FirstName,
                lastName: _fixture.Faker.Person.LastName,
                email: _fixture.Faker.Person.Email,
                groups: new List<string> { "role-01" }
            );

            var validator = new UpdateProfileInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'User Id' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when groups is empty")]
        [Trait("Application", "UpdateProfile / UpdateProfileInputValidator")]
        public void DontValidateWhenEmptyGroups()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = new UpdateProfileInput
            (
                userId: Guid.NewGuid(),
                firstName: _fixture.Faker.Person.FirstName,
                lastName: _fixture.Faker.Person.LastName,
                email: _fixture.Faker.Person.Email,
                groups: new List<string>()
            );

            var validator = new UpdateProfileInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Groups' must not be empty.");
        }
    }
}
