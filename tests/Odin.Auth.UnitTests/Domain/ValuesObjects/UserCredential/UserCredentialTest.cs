using FluentAssertions;
using Odin.Auth.Domain.Exceptions;
using ValueObject = Odin.Auth.Domain.ValuesObjects;

namespace Odin.Auth.UnitTests.Domain.ValuesObjects.UserCredential
{
    [Collection(nameof(UserCredentialTestFixtureCollection))]
    public class UserCredentialTest
    {
        private readonly UserCredentialTestFixture _fixture;

        public UserCredentialTest(UserCredentialTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "ctor() should instantiate a new credential")]
        [Trait("Domain", "Value Objects / UserCredential")]
        public void Instantiate()
        {
            var validUserCredential = _fixture.GetValidUserCredential();
            var credential = new ValueObject.UserCredential(validUserCredential.Value, validUserCredential.Temporary);

            credential.Should().NotBeNull();
            credential.Value.Should().Be(validUserCredential.Value);
            credential.Temporary.Should().Be(validUserCredential.Temporary);
        }

        [Theory(DisplayName = "ctor() should throw an error when value is empty")]
        [Trait("Domain", "Value Objects / UserCredential")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void InstantiateErrorWhenValueIsEmpty(string? name)
        {
            var validUserCredential = _fixture.GetValidUserCredential();

            Action action = () => new ValueObject.UserCredential(name!, temporary: true);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("Value should not be empty or null");
        }
    }
}
