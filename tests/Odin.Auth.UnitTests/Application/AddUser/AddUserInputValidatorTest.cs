using FluentAssertions;
using FluentValidation;
using Odin.Auth.Application.AddUser;

namespace Odin.Auth.UnitTests.Application.AddUser
{
    [Collection(nameof(AddUserTestFixtureCollection))]
    public class AddUserInputValidatorTest
    {
        private readonly AddUserTestFixture _fixture;

        public AddUserInputValidatorTest(AddUserTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "Validate() should not validate when first name is empty")]
        [Trait("Application", "AddUser / AddUserInputValidator")]
        public void DontValidateWhenEmptyFirstName()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = new AddUserInput
            {
                FirstName = "",
                LastName = _fixture.Faker.Person.LastName,
                EmailAddress = _fixture.Faker.Person.Email,
                Username = _fixture.Faker.Person.UserName,
                Password = "unit.testing"
            };

            var validator = new AddUserInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'First Name' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when last name is empty")]
        [Trait("Application", "AddUser / AddUserInputValidator")]
        public void DontValidateWhenEmptyLastName()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = new AddUserInput
            {
                FirstName = _fixture.Faker.Person.FirstName,
                LastName = "",
                EmailAddress = _fixture.Faker.Person.Email,
                Username = _fixture.Faker.Person.UserName,
                Password = "unit.testing"
            };

            var validator = new AddUserInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Last Name' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when email address is empty")]
        [Trait("Application", "AddUser / AddUserInputValidator")]
        public void DontValidateWhenEmptyEmailAddress()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = new AddUserInput
            {
                FirstName = _fixture.Faker.Person.FirstName,
                LastName = _fixture.Faker.Person.LastName,
                EmailAddress = "",
                Username = _fixture.Faker.Person.UserName,
                Password = "unit.testing"
            };

            var validator = new AddUserInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(2);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Email Address' must not be empty.");
            validateResult.Errors[1].ErrorMessage.Should().Be("'Email Address' is not a valid email address.");
        }

        [Theory(DisplayName = "Validate() should not validate when email address is invalid")]
        [Trait("Application", "AddUser / AddUserInputValidator")]
        [InlineData("email.com")]
        [InlineData("email@")]
        public void DontValidateWhenInvalidEmailAddress(string email)
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = new AddUserInput
            {
                FirstName = _fixture.Faker.Person.FirstName,
                LastName = _fixture.Faker.Person.LastName,
                EmailAddress = email,
                Username = _fixture.Faker.Person.UserName,
                Password = "unit.testing"
            };

            var validator = new AddUserInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Email Address' is not a valid email address.");
        }

        [Fact(DisplayName = "Validate() should not validate when username is empty")]
        [Trait("Application", "AddUser / AddUserInputValidator")]
        public void DontValidateWhenEmptyUsername()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = new AddUserInput
            {
                FirstName = _fixture.Faker.Person.FirstName,
                LastName = _fixture.Faker.Person.LastName,
                EmailAddress = _fixture.Faker.Person.Email,
                Username = "",
                Password = "unit.testing"
            };

            var validator = new AddUserInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Username' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when password is empty")]
        [Trait("Application", "AddUser / AddUserInputValidator")]
        public void DontValidateWhenEmptyPassword()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = new AddUserInput
            {
                FirstName = _fixture.Faker.Person.FirstName,
                LastName = _fixture.Faker.Person.LastName,
                EmailAddress = _fixture.Faker.Person.Email,
                Username = _fixture.Faker.Person.UserName,
                Password = ""
            };

            var validator = new AddUserInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Password' must not be empty.");
        }
    }
}
