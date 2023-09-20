using FluentAssertions;
using Odin.Auth.Domain.Exceptions;
using ValueObject = Odin.Auth.Domain.ValuesObjects;

namespace Odin.Auth.UnitTests.Domain.ValuesObjects.UserGroup
{
    [Collection(nameof(UserGroupTestFixtureCollection))]
    public class UserGroupTest
    {
        private readonly UserGroupTestFixture _fixture;

        public UserGroupTest(UserGroupTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "ctor() should instantiate a new group")]
        [Trait("Domain", "Value Objects / UserGroup")]
        public void Instantiate()
        {
            var validUserGroup = _fixture.GetValidUserGroup();
            var group = new ValueObject.UserGroup(validUserGroup.Id, validUserGroup.Name, validUserGroup.Path!);

            group.Should().NotBeNull();
            group.Id.Should().Be(validUserGroup.Id);
            group.Name.Should().Be(validUserGroup.Name);
            group.Path.Should().Be(validUserGroup.Path);
        }

        [Theory(DisplayName = "ctor() should throw an error when name is empty")]
        [Trait("Domain", "Value Objects / UserGroup")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void InstantiateErrorWhenNameIsEmpty(string? name)
        {
            var validUserGroup = _fixture.GetValidUserGroup();

            Action action = () => new ValueObject.UserGroup(validUserGroup.Id, name!, validUserGroup.Path!);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("Name should not be empty or null");
        }
    }
}
