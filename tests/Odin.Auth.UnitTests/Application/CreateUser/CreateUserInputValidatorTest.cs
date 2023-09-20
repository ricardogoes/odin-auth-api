using FluentAssertions;
using FluentValidation;
using Odin.Auth.Application.CreateUser;

namespace Odin.Auth.UnitTests.Application.CreateUser
{
    [Collection(nameof(CreateUserTestFixtureCollection))]
    public class CreateUserInputValidatorTest
    {
        private readonly CreateUserTestFixture _fixture;

        public CreateUserInputValidatorTest(CreateUserTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "Validate() should not validate when first name is empty")]
        [Trait("Application", "CreateUser / CreateUserInputValidator")]
        public void DontValidateWhenEmptyFirstName()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = new CreateUserInput
            (
                username: _fixture.Faker.Person.UserName,
                password: "unit.testing",
                passwordIsTemporary: true,
                firstName: "",
                lastName: _fixture.Faker.Person.LastName,
                email: _fixture.Faker.Person.Email,
                groups: new List<string> { "role-01" },
                loggedUsername: "admin"
            );

            var validator = new CreateUserInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'First Name' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when last name is empty")]
        [Trait("Application", "CreateUser / CreateUserInputValidator")]
        public void DontValidateWhenEmptyLastName()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = new CreateUserInput
            (
                username: _fixture.Faker.Person.UserName,
                password: "unit.testing",
                passwordIsTemporary: true,
                firstName: _fixture.Faker.Person.FirstName,
                lastName: "",
                email: _fixture.Faker.Person.Email,
                groups: new List<string> { "role-01" },
                loggedUsername: "admin"
            );

            var validator = new CreateUserInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Last Name' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when email address is empty")]
        [Trait("Application", "CreateUser / CreateUserInputValidator")]
        public void DontValidateWhenEmptyEmailAddress()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = new CreateUserInput
            (
                username: _fixture.Faker.Person.UserName,
                password: "unit.testing",
                passwordIsTemporary: true,
                firstName: _fixture.Faker.Person.FirstName,
                lastName: _fixture.Faker.Person.LastName,
                email: "",
                groups: new List<string> { "role-01" },
                loggedUsername: "admin"
            );

            var validator = new CreateUserInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Email' is not a valid email address.");
        }

        [Theory(DisplayName = "Validate() should not validate when email address is invalid")]
        [Trait("Application", "CreateUser / CreateUserInputValidator")]
        [InlineData("email.com")]
        [InlineData("email@")]
        public void DontValidateWhenInvalidEmailAddress(string email)
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = new CreateUserInput
            (
                username: _fixture.Faker.Person.UserName,
                password: "unit.testing",
                passwordIsTemporary: true,
                firstName: _fixture.Faker.Person.FirstName,
                lastName: _fixture.Faker.Person.LastName,
                email: email,
                groups: new List<string> { "role-01" },
                loggedUsername: "admin"
            );

            var validator = new CreateUserInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Email' is not a valid email address.");
        }

        [Fact(DisplayName = "Validate() should not validate when username is empty")]
        [Trait("Application", "CreateUser / CreateUserInputValidator")]
        public void DontValidateWhenEmptyUsername()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = new CreateUserInput
            (
                username: "",
                password: "unit.testing",
                passwordIsTemporary: true,
                firstName: _fixture.Faker.Person.FirstName,
                lastName: _fixture.Faker.Person.LastName,
                email: _fixture.Faker.Person.Email,
                groups: new List<string> { "role-01" },
                loggedUsername: "admin"
            );

            var validator = new CreateUserInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Username' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when password is empty")]
        [Trait("Application", "CreateUser / CreateUserInputValidator")]
        public void DontValidateWhenEmptyPassword()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = new CreateUserInput
            (
                username: _fixture.Faker.Person.UserName,
                password: "",
                passwordIsTemporary: true,
                firstName: _fixture.Faker.Person.FirstName,
                lastName: _fixture.Faker.Person.LastName,
                email: _fixture.Faker.Person.Email,
                groups: new List<string> { "role-01" },
                loggedUsername: "admin"
            );

            var validator = new CreateUserInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Password' must not be empty.");
        }
    }
}
