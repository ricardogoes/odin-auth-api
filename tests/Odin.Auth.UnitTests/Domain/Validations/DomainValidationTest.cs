using FluentAssertions;
using Odin.Auth.Domain.Exceptions;
using Odin.Auth.Domain.Validations;

namespace Odin.Auth.UnitTests.Domain.Validations
{
    [Collection(nameof(DomainValidationTestFixtureCollection))]
    public class DomainValidationTest
    {
        private readonly DomainValidationTestFixture _fixture;

        public DomainValidationTest(DomainValidationTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "NotNull() should validate when value is not null")]
        [Trait("Domain", "Validations / Domain Validation")]
        public void NotNullOk()
        {
            string fieldName = _fixture.GetValidFieldName();
            var value = _fixture.GetValidValue();
            Action action =
                () => DomainValidation.NotNull(value, fieldName);
            action.Should().NotThrow();
        }

        [Fact(DisplayName = "NotNull() should throw an error when value is null")]
        [Trait("Domain", "Validations / Domain Validation")]
        public void NotNullThrowWhenNull()
        {
            string? value = null;
            string fieldName = _fixture.GetValidFieldName();

            Action action =
                () => DomainValidation.NotNull(value, fieldName);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage($"{fieldName} should not be null");
        }


        [Theory(DisplayName = "NotNullOrEmpty() should validate when value is not null or empty")]
        [Trait("Domain", "Validations / Domain Validation")]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData("00000000-0000-0000-0000-000000000000")]
        public void NotNullOrEmptyThrowWhenEmpty(object obj)
        {
            string fieldName = _fixture.GetValidFieldName();
            object? target = null;

            if (obj is not null)
            {
                if (obj is string && obj.ToString()!.Length < 36) target = obj.ToString();
                else if (obj is int) target = int.Parse(obj.ToString()!);
                else if (obj.ToString()!.Length == 36) target = Guid.Parse(obj.ToString()!);

                Action action =
                () => DomainValidation.NotNullOrEmpty(target, fieldName);

                action.Should().Throw<EntityValidationException>()
                    .WithMessage($"{fieldName} should not be empty or null");
            }
        }

        [Theory(DisplayName = "NotNullOrEmpty() should throw an error when value is null or empty")]
        [Trait("Domain", "Validations / Domain Validation")]
        [InlineData("string")]
        [InlineData(1)]
        [InlineData("39E95CB2-2E78-4713-97F0-303818900B00")]
        public void NotNullOrEmptyOk(object obj)
        {
            string fieldName = _fixture.GetValidFieldName();

            object? target = null;

            if (obj is not null)
            {
                if (obj is string && obj.ToString()!.Length < 36) target = obj.ToString();
                else if (obj is int) target = int.Parse(obj.ToString()!);
                else if (obj.ToString()!.Length == 36) target = Guid.Parse(obj.ToString()!);

                Action action =
                    () => DomainValidation.NotNullOrEmpty(target, fieldName);

                action.Should().NotThrow();
            }
        }

        [Theory(DisplayName = "MinLength() should throw an error when value length is less than minimum length")]
        [Trait("Domain", "Validations / Domain Validation")]
        [MemberData(
            nameof(DomainValidationTestDataGenerator.GetValuesSmallerThanMin),
            parameters: 10,
            MemberType = typeof(DomainValidationTestDataGenerator))]
        public void MinLengthThrowWhenLess(string target, int minLength)
        {
            string fieldName = _fixture.GetValidFieldName();

            Action action =
                () => DomainValidation.MinLength(target, minLength, fieldName);

            action.Should().Throw<EntityValidationException>()
                .WithMessage($"{fieldName} should be at least {minLength} characters long");
        }



        [Theory(DisplayName = "MinLength() should validate when value length is greater than mininum length")]
        [Trait("Domain", "Validations / Domain Validation")]
        [MemberData(
            nameof(DomainValidationTestDataGenerator.GetValuesGreaterThanMin),
            parameters: 10,
            MemberType = typeof(DomainValidationTestDataGenerator))]
        public void MinLengthOk(string target, int minLength)
        {
            string fieldName = _fixture.GetValidFieldName().Replace(" ", "");

            Action action =
                () => DomainValidation.MinLength(target, minLength, fieldName);

            action.Should().NotThrow();
        }

        [Theory(DisplayName = "MaxLength() should throw an error when value length is greater then max length")]
        [Trait("Domain", "Validations / Domain Validation")]
        [MemberData(
            nameof(DomainValidationTestDataGenerator.GetValuesGreaterThanMax),
            parameters: 10,
            MemberType = typeof(DomainValidationTestDataGenerator))]
        public void MaxLengthThrowWhenGreater(string target, int maxLength)
        {
            string fieldName = _fixture.GetValidFieldName();

            Action action =
                () => DomainValidation.MaxLength(target, maxLength, fieldName);

            action.Should().Throw<EntityValidationException>()
                .WithMessage($"{fieldName} should be less or equal {maxLength} characters long");
        }

        [Theory(DisplayName = "MaxLength() should validate when value length is lesser than maximum length")]
        [Trait("Domain", "Validations / Domain Validation")]
        [MemberData(
            nameof(DomainValidationTestDataGenerator.GetValuesLessThanMax),
            parameters: 10,
            MemberType = typeof(DomainValidationTestDataGenerator))]
        public void MaxLengthOk(string target, int maxLength)
        {
            string fieldName = _fixture.GetValidFieldName();

            Action action =
                () => DomainValidation.MaxLength(target, maxLength, fieldName);

            action.Should().NotThrow();
        }

        [Theory(DisplayName = "Email() should throw an error when email is invalid")]
        [Trait("Domain", "Validations / Domain Validation")]
        [InlineData("123123123")]
        [InlineData("email@email")]
        [InlineData("")]
        [InlineData("email.com")]
        public void ThrowErrorWithInvalidEmail(string target)
        {
            string fieldName = "Email";

            Action action =
                () => DomainValidation.Email(target, fieldName);

            action.Should().Throw<EntityValidationException>()
                .WithMessage($"{fieldName} should be a valid email");
        }

        [Theory(DisplayName = "Email() should validate email")]
        [Trait("Domain", "Validations / Domain Validation")]
        [InlineData("email@email.com")]
        [InlineData("email@email.com.br")]
        public void EmailOk(string target)
        {
            string fieldName = _fixture.GetValidFieldName();

            Action action =
                () => DomainValidation.Email(target, fieldName);

            action.Should().NotThrow();
        }

        [Fact(DisplayName = "ListNotNullOrEmpty() should throw an error when list is empty")]
        [Trait("Domain", "Validations / Domain Validation")]
        public void ListNotNullOrEmptyThrowWhenEmpty()
        {
            string fieldName = _fixture.GetValidFieldName();

            var list = new List<string>();
            
            Action action = () => DomainValidation.ListNotNullOrEmpty(list, fieldName);
            action.Should().Throw<EntityValidationException>()
                   .WithMessage($"{fieldName}, should not be empty or null");
        }

        [Fact(DisplayName = "ListNotNullOrEmpty() should throw an error when list is null")]
        [Trait("Domain", "Validations / Domain Validation")]
        public void ListNotNullOrEmptyThrowWhenNull()
        {
            string fieldName = _fixture.GetValidFieldName();

            Action action = () => DomainValidation.ListNotNullOrEmpty<string>(null, fieldName);
            action.Should().Throw<EntityValidationException>()
                   .WithMessage($"{fieldName}, should not be empty or null");
        }
    }
}
