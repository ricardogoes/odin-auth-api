using FluentAssertions;
using Odin.Auth.Domain.Exceptions;
using Odin.Auth.Domain.ValuesObjects;
using System.Configuration;
using DomainEntity = Odin.Auth.Domain.Entities;

namespace Odin.Auth.UnitTests.Domain.Entities.User
{
    [Collection(nameof(UserTestFixtureCollection))]
    public class UserTest
    {
        private readonly UserTestFixture _fixture;

        public UserTest(UserTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "ctor() should instantiate a new user")]
        [Trait("Domain", "Entities / User")]
        public void Instantiate()
        {
            var validUser = _fixture.GetValidUser();
            var user = new DomainEntity.User(validUser.Username, validUser.FirstName, validUser.LastName, validUser.Email);
            user.AddGroup(validUser.Groups.First());

            user.Should().NotBeNull();
            user.Username.Should().Be(validUser.Username);
            user.FirstName.Should().Be(validUser.FirstName);
            user.LastName.Should().Be(validUser.LastName);
            user.Email.Should().Be(validUser.Email);
            user.Groups.Should().HaveCount(1);
            user.Id.Should().NotBeEmpty();
            user.IsActive.Should().BeTrue();
        }

        [Theory(DisplayName = "ctor() should throw an error when username is empty")]
        [Trait("Domain", "Entities / User")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void InstantiateErrorWhenUsernameIsEmpty(string? username)
        {
            var validUser = _fixture.GetValidUser();

            Action action = () => new DomainEntity.User(username!, validUser.FirstName,validUser.LastName, validUser.Email);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("Username should not be empty or null");
        }

        [Theory(DisplayName = "ctor() should throw an error when first name is empty")]
        [Trait("Domain", "Entities / User")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void InstantiateErrorWhenFirstNameIsEmpty(string? firstName)
        {
            var validUser = _fixture.GetValidUser();

            Action action = () => new DomainEntity.User(validUser.Username, firstName!, validUser.LastName, validUser.Email);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("FirstName should not be empty or null");
        }

        [Theory(DisplayName = "ctor() should throw an error when last name is empty")]
        [Trait("Domain", "Entities / User")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void InstantiateErrorWhenLastNameIsEmpty(string? lastName)
        {
            var validUser = _fixture.GetValidUser();

            Action action = () => new DomainEntity.User(validUser.Username, validUser.FirstName, lastName!, validUser.Email);            

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("LastName should not be empty or null");
        }

        [Theory(DisplayName = "ctor() should throw an error when email is empty or invalid")]
        [Trait("Domain", "Entities / User")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        [InlineData("123123123")]
        [InlineData("email@email")]
        [InlineData("email.com")]
        public void InstantiateErrorWhenEmailIsEmpty(string? email)
        {
            var validUser = _fixture.GetValidUser();

            Action action = () => new DomainEntity.User(validUser.Username, validUser.FirstName, validUser.LastName, email!);            

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("Email should be a valid email");
        }

        [Fact(DisplayName = "Activate() should activate a user")]
        [Trait("Domain", "Entities / User")]
        public void Activate()
        {
            var validUser = _fixture.GetValidUser();

            var user = new DomainEntity.User(validUser.Username, validUser.FirstName, validUser.LastName, validUser.Email);
            user.AddGroup(validUser.Groups.First());

            user.Activate();

            user.IsActive.Should().BeTrue();
        }

        [Fact(DisplayName = "Deactivate() should deactivate a user")]
        [Trait("Domain", "Entities / User")]
        public void Deactivate()
        {
            var validUser = _fixture.GetValidUser();

            var user = new DomainEntity.User(validUser.Username, validUser.FirstName, validUser.LastName, validUser.Email);
            user.AddGroup(validUser.Groups.First());
            user.Deactivate();

            user.IsActive.Should().BeFalse();
        }

        [Fact(DisplayName = "Update() should update a user")]
        [Trait("Domain", "Entities / User")]
        public void Update()
        {
            var user = _fixture.GetValidUser();
            var userWithNewValues = _fixture.GetValidUser();

            user.Update(userWithNewValues.FirstName, userWithNewValues.LastName, userWithNewValues.Email);
            user.AddGroup(userWithNewValues.Groups.First());

            user.Username.Should().Be(user.Username);
            user.FirstName.Should().Be(userWithNewValues.FirstName);
            user.LastName.Should().Be(userWithNewValues.LastName);
            user.Email.Should().Be(userWithNewValues.Email);
            user.Groups.Should().NotBeNull();
            user.Groups.Should().HaveCount(2);
        }

        [Fact(DisplayName = "AddCredentials() should add a new credential to user")]
        [Trait("Domain", "Entities / User")]
        public void AddCredential_AddNewCredential()
        {
            var validUser = _fixture.GetValidUser();

            var user = new DomainEntity.User(validUser.Username, validUser.FirstName, validUser.LastName, validUser.Email);
            user.AddGroup(validUser.Groups.First());

            user.AddCredentials(new UserCredential("password", true));

            user.Credentials.Should().NotBeNull();
            user.Credentials.Should().HaveCount(1);
            user.Credentials.First().Value.Should().Be("password");
            user.Credentials.First().Temporary.Should().BeTrue();

        }

        [Fact(DisplayName = "AddGroup() should add a new group to user")]
        [Trait("Domain", "Entities / User")]
        public void AddGroup()
        {
            var validUser = _fixture.GetValidUser();

            var user = new DomainEntity.User(validUser.Username, validUser.FirstName, validUser.LastName, validUser.Email);            
            user.AddGroup(validUser.Groups.First());

            user.AddGroup(new UserGroup(Guid.NewGuid(), "odin-group-02", "/odin-group-02"));
            
            user.Groups.Should().NotBeNull();
            user.Groups.Should().HaveCount(2);
            user.Groups.Last().Name.Should().Be("odin-group-02");
            user.Groups.Last().Path.Should().Be("/odin-group-02");

        }

        [Fact(DisplayName = "RemoveAllGroups() should remove all groups of a user")]
        [Trait("Domain", "Entities / User")]
        public void RemoveAllGroups()
        {
            var validUser = _fixture.GetValidUser();

            var user = new DomainEntity.User(validUser.Username, validUser.FirstName, validUser.LastName, validUser.Email);            
            user.AddGroup(validUser.Groups.First());
            user.AddGroup(new UserGroup(Guid.NewGuid(), "odin-group-02", "/odin-group-02"));

            user.RemoveAllGroups();

            user.Groups.Should().NotBeNull();
            user.Groups.Should().HaveCount(0);
        }


        [Fact(DisplayName = "AddAttributes() should add a new attribute to user")]
        [Trait("Domain", "Entities / User")]
        public void AddAttributes_AddNewAttribute()
        {
            var validUser = _fixture.GetValidUser();

            var user = new DomainEntity.User(validUser.Username, validUser.FirstName, validUser.LastName, validUser.Email);
            user.AddGroup(validUser.Groups.First());

            user.AddAttribute(new KeyValuePair<string, string>("key", "value"));

            user.Attributes.Should().NotBeNull();
            user.Attributes.Should().HaveCount(1);
            user.Attributes.First().Key.Should().Be("key");
            user.Attributes.First().Value.Should().Be("value");
        }

        [Fact(DisplayName = "AddAttributes() should update an attribute with same key")]
        [Trait("Domain", "Entities / User")]
        public void AddAttributes_UpdateAttribute()
        {
            var validUser = _fixture.GetValidUser();

            var user = new DomainEntity.User(validUser.Username, validUser.FirstName, validUser.LastName, validUser.Email);
            user.AddGroup(validUser.Groups.First());

            user.AddAttribute(new KeyValuePair<string, string>("key", "value"));
            user.AddAttribute(new KeyValuePair<string, string>("key", "new_value"));

            user.Attributes.Should().NotBeNull();
            user.Attributes.Should().HaveCount(1);
            user.Attributes.First().Key.Should().Be("key");
            user.Attributes.First().Value.Should().Be("new_value");
        }

        [Fact(DisplayName = "SetAuditLog() should set auditLog")]
        [Trait("Domain", "Entities / Customer")]
        public void AuditLog()
        {
            var user = _fixture.GetValidUser();

            var createdAt = DateTime.Now;
            var createdBy = "unit.testing";
            var lastUpdatedAt = DateTime.Now;
            var lastUpdatedBy = "unit.testing";

            user.SetAuditLog(createdAt, createdBy, lastUpdatedAt, lastUpdatedBy);

            user.CreatedAt.Should().Be(createdAt);
            user.CreatedBy.Should().Be(createdBy);
            user.LastUpdatedAt.Should().Be(lastUpdatedAt);
            user.LastUpdatedBy.Should().Be(lastUpdatedBy);
        }
    }
}
