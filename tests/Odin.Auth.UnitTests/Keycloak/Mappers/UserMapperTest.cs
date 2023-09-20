
using FluentAssertions;
using Odin.Auth.Domain.Entities;
using Odin.Auth.Domain.ValuesObjects;
using Odin.Auth.Infra.Keycloak.Mappers;
using Odin.Auth.Infra.Keycloak.Models;

namespace Odin.Auth.UnitTests.Keycloak.Mappers
{
    [Collection(nameof(MapperTestFixtureCollection))]
    public class UserMapperTest
    {
        private readonly MapperTestFixture _fixture;

        public UserMapperTest(MapperTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "ToUserRepresentation() should map a User to UserRepresentation type")]
        [Trait("Keycloak", "Mappers / UserMapper")]
        public void ToUserRepresentation()
        {
            var userId = Guid.NewGuid();
            var user = new User(userId, "unit.testing", "Unit", "Testing", "unit.testing@email.com", true);

            user.AddAttribute(new KeyValuePair<string, string>("key1", "value1"));
            user.AddAttribute(new KeyValuePair<string, string>("key2", "value2"));

            user.AddCredentials(new UserCredential("password", temporary: false));
            user.AddGroup(new UserGroup("odin-group"));

            var userRepresentation = user.ToUserRepresentation();

            userRepresentation.Should().NotBeNull();
            userRepresentation!.Id.Should().Be(user.Id);
            userRepresentation!.Username.Should().Be(user.Username);
            userRepresentation!.FirstName.Should().Be(user.FirstName);
            userRepresentation!.LastName.Should().Be(user.LastName);
            userRepresentation!.Email.Should().Be(user.Email);
            userRepresentation!.Enabled.Should().Be(user.Enabled);

            userRepresentation.Attributes.Should().NotBeNull();
            userRepresentation.Attributes.Should().HaveCount(2);
            userRepresentation.Attributes!.ContainsKey("key1").Should().BeTrue();
            userRepresentation.Attributes.ContainsKey("key2").Should().BeTrue();

            userRepresentation.Groups.Should().NotBeNull();
            userRepresentation.Groups.Should().HaveCount(1);
            userRepresentation.Groups!.First().Should().Be("odin-group");

            userRepresentation.Credentials.Should().NotBeNull();
            userRepresentation.Credentials!.First().Type.Should().Be("password");
            userRepresentation.Credentials!.First().Value.Should().Be("password");
            userRepresentation.Credentials!.First().Temporary.Should().BeFalse();
        }


        [Fact(DisplayName = "ToUserRepresentation() should map a list of Users to a list of UserRepresentation type")]
        [Trait("Keycloak", "Mappers / UserMapper")]
        public void ToUserRepresentationList()
        {
            var userId1 = Guid.NewGuid();
            var user1 = new User(userId1, "unit01.testing", "Unit01", "Testing", "unit01.testing@email.com", true);

            user1.AddAttribute(new KeyValuePair<string, string>("key11", "value11"));
            user1.AddAttribute(new KeyValuePair<string, string>("key12", "value12"));

            user1.AddCredentials(new UserCredential("password", temporary: false));
            user1.AddGroup(new UserGroup("odin-group"));

            var userId2 = Guid.NewGuid();
            var user2 = new User(userId2, "unit02.testing", "Unit02", "Testing", "unit02.testing@email.com", true);

            user2.AddAttribute(new KeyValuePair<string, string>("key21", "value21"));
            user2.AddAttribute(new KeyValuePair<string, string>("key22", "value22"));

            user2.AddCredentials(new UserCredential("password", temporary: false));
            user2.AddGroup(new UserGroup("odin-group"));

            var users = new List<User> { user1, user2 };

            var usersRepresentation = users!.ToUserRepresentation().ToList();
                        
            usersRepresentation.Should().NotBeNull();
            usersRepresentation.Should().HaveCount(2);

            usersRepresentation[0].Id.Should().Be(user1.Id);
            usersRepresentation[0].Username.Should().Be(user1.Username);
            usersRepresentation[0].FirstName.Should().Be(user1.FirstName);
            usersRepresentation[0].LastName.Should().Be(user1.LastName);
            usersRepresentation[0].Email.Should().Be(user1.Email);
            usersRepresentation[0].Enabled.Should().Be(user1.Enabled);
                
            usersRepresentation[0].Attributes.Should().NotBeNull();
            usersRepresentation[0].Attributes.Should().HaveCount(2);
            usersRepresentation[0].Attributes!.ContainsKey("key11").Should().BeTrue();
            usersRepresentation[0].Attributes!.ContainsKey("key12").Should().BeTrue();
                
            usersRepresentation[0].Groups.Should().NotBeNull();
            usersRepresentation[0].Groups.Should().HaveCount(1);
            usersRepresentation[0].Groups!.First().Should().Be("odin-group");
                
            usersRepresentation[0].Credentials.Should().NotBeNull();
            usersRepresentation[0].Credentials!.First().Type.Should().Be("password");
            usersRepresentation[0].Credentials!.First().Value.Should().Be("password");
            usersRepresentation[0].Credentials!.First().Temporary.Should().BeFalse();

            usersRepresentation[1].Id.Should().Be(user2.Id);
            usersRepresentation[1].Username.Should().Be(user2.Username);
            usersRepresentation[1].FirstName.Should().Be(user2.FirstName);
            usersRepresentation[1].LastName.Should().Be(user2.LastName);
            usersRepresentation[1].Email.Should().Be(user2.Email);
            usersRepresentation[1].Enabled.Should().Be(user2.Enabled);

            usersRepresentation[1].Attributes.Should().NotBeNull();
            usersRepresentation[1].Attributes.Should().HaveCount(2);
            usersRepresentation[1].Attributes!.ContainsKey("key21").Should().BeTrue();
            usersRepresentation[1].Attributes!.ContainsKey("key22").Should().BeTrue();

            usersRepresentation[1].Groups.Should().NotBeNull();
            usersRepresentation[1].Groups.Should().HaveCount(1);
            usersRepresentation[1].Groups!.First().Should().Be("odin-group");

            usersRepresentation[1].Credentials.Should().NotBeNull();
            usersRepresentation[1].Credentials!.First().Type.Should().Be("password");
            usersRepresentation[1].Credentials!.First().Value.Should().Be("password");
            usersRepresentation[1].Credentials!.First().Temporary.Should().BeFalse();
        }


        [Fact(DisplayName = "ToUser() should map a UserRepresentation to User type without user group")]
        [Trait("Keycloak", "Mappers / UserMapper")]
        public void ToUser()
        {
            var userRepresentation = new UserRepresentation
            {
                Id = Guid.NewGuid(),
                Username = "unit.testing",
                FirstName = "Unit",
                LastName = "Testing",
                Email = "unit.testing@email.com",
                EmailVerified = true,
                Enabled = true,
                Attributes = new Dictionary<string, List<string>> { { "key1", new List<string> { "value1" } }, { "key2", new List<string> { "value2" } } },
                Credentials = new List<CredentialRepresentation> { new CredentialRepresentation { Id = Guid.NewGuid(), Type = "password", Value = "password", Temporary = true } },
                Groups = new List<string> { "odin-group-01" }
            };

            var user = userRepresentation.ToUser();

            user.Should().NotBeNull();
            user.Id.Should().Be(userRepresentation.Id!.Value);
            user.Username.Should().Be(userRepresentation.Username);
            user.FirstName.Should().Be(userRepresentation.FirstName);
            user.LastName.Should().Be(userRepresentation.LastName);
            user.Email.Should().Be(userRepresentation.Email);
            user.Enabled.Should().Be(userRepresentation.Enabled!.Value);

            user.Attributes.Should().NotBeNull();
            user.Attributes.Should().HaveCount(2);
            user.Attributes!.ContainsKey("key1").Should().BeTrue();
            user.Attributes!.ContainsKey("key2").Should().BeTrue();
        }

        [Fact(DisplayName = "ToUser() should map a UserRepresentation to User type with user group")]
        [Trait("Keycloak", "Mappers / UserMapper")]
        public void ToUserWithUserGroup()
        {
            var userRepresentation = new UserRepresentation
            {
                Id = Guid.NewGuid(),
                Username = "unit.testing",
                FirstName = "Unit",
                LastName = "Testing",
                Email = "unit.testing@email.com",
                EmailVerified = true,
                Enabled = true,
                Attributes = new Dictionary<string, List<string>> { { "key1", new List<string> { "value1" } }, { "key2", new List<string> { "value2" } } },
                Credentials = new List<CredentialRepresentation> { new CredentialRepresentation { Id = Guid.NewGuid(), Type = "password", Value = "password", Temporary = true } },
                Groups = new List<string> { "odin-group-01" }
            };

            var userGroup = new UserGroup(Guid.NewGuid(), "odin-group-01", "/odin-group-01");

            var user = userRepresentation.ToUser(new List<UserGroup> { userGroup });

            user.Should().NotBeNull();
            user.Id.Should().Be(userRepresentation.Id!.Value);
            user.Username.Should().Be(userRepresentation.Username);
            user.FirstName.Should().Be(userRepresentation.FirstName);
            user.LastName.Should().Be(userRepresentation.LastName);
            user.Email.Should().Be(userRepresentation.Email);
            user.Enabled.Should().Be(userRepresentation.Enabled!.Value);

            user.Attributes.Should().NotBeNull();
            user.Attributes.Should().HaveCount(2);
            user.Attributes!.ContainsKey("key1").Should().BeTrue();
            user.Attributes!.ContainsKey("key2").Should().BeTrue();

            user.Groups.Should().NotBeNull();
            user.Groups.Should().HaveCount(1);
            user.Groups!.First().Name.Should().Be("odin-group-01");
        }

        [Fact(DisplayName = "ToUser() should map a UserRepresentation to User type without attributes")]
        [Trait("Keycloak", "Mappers / UserMapper")]
        public void ToUserWithoutAttributes()
        {
            var userRepresentation = new UserRepresentation
            {
                Id = Guid.NewGuid(),
                Username = "unit.testing",
                FirstName = "Unit",
                LastName = "Testing",
                Email = "unit.testing@email.com",
                EmailVerified = true,
                Enabled = true,
                Credentials = new List<CredentialRepresentation> { new CredentialRepresentation { Id = Guid.NewGuid(), Type = "password", Value = "password", Temporary = true } },
                Groups = new List<string> { "odin-group-01" }
            };

            var userGroup = new UserGroup(Guid.NewGuid(), "odin-group-01", "/odin-group-01");

            var user = userRepresentation.ToUser(new List<UserGroup> { userGroup });

            user.Should().NotBeNull();
            user.Id.Should().Be(userRepresentation.Id!.Value);
            user.Username.Should().Be(userRepresentation.Username);
            user.FirstName.Should().Be(userRepresentation.FirstName);
            user.LastName.Should().Be(userRepresentation.LastName);
            user.Email.Should().Be(userRepresentation.Email);
            user.Enabled.Should().Be(userRepresentation.Enabled!.Value);

            user.Groups.Should().NotBeNull();
            user.Groups.Should().HaveCount(1);
            user.Groups!.First().Name.Should().Be("odin-group-01");
        }

        [Fact(DisplayName = "ToUser() should map a list of UserRepresentation to list of User with groups")]
        [Trait("Keycloak", "Mappers / UserMapper")]
        public void ToUserWithGroupsList()
        {
            var userId1 = Guid.NewGuid();
            var userRepresentation1 = new UserRepresentation
            {
                Id = userId1,
                Username = "unit1.testing",
                FirstName = "Unit 1",
                LastName = "Testing",
                Email = "unit1.testing@email.com",
                EmailVerified = true,
                Enabled = true,
                Attributes = new Dictionary<string, List<string>> { { "key11", new List<string> { "value11" } }, { "key12", new List<string> { "value12" } } },
                Credentials = new List<CredentialRepresentation> { new CredentialRepresentation { Id = Guid.NewGuid(), Type = "password", Value = "password", Temporary = true } },
                Groups = new List<string> { "odin-group-01" }
            };

            var userId2 = Guid.NewGuid();
            var userRepresentation2 = new UserRepresentation
            {
                Id = userId2,
                Username = "uni2t.testing",
                FirstName = "Unit 2",
                LastName = "Testing",
                Email = "unit2.testing@email.com",
                EmailVerified = true,
                Enabled = true,
                Attributes = new Dictionary<string, List<string>> { { "key21", new List<string> { "value21" } }, { "key22", new List<string> { "value22" } } },
                Credentials = new List<CredentialRepresentation> { new CredentialRepresentation { Id = Guid.NewGuid(), Type = "password", Value = "password", Temporary = true } },
                Groups = new List<string> { "odin-group-01" }
            };

            var usersRepresentation = new List<UserRepresentation> { userRepresentation1, userRepresentation2 };

            var userGroup1 = new UserGroup(Guid.NewGuid(), "odin-group-01", "/odin-group-01");
            var userGroup2 = new UserGroup(Guid.NewGuid(), "odin-group-02", "/odin-group-02");

            var dicUserGroup = new Dictionary<Guid, List<UserGroup>>
            {
                { userId1, new List<UserGroup> {userGroup1} },
                { userId2, new List<UserGroup> {userGroup2} }
            };

            var users = usersRepresentation.ToUser(dicUserGroup).ToList();

            users.Should().NotBeNull();
            users.Should().HaveCount(2);

            users[0].Should().NotBeNull();
            users[0].Id.Should().Be(userRepresentation1.Id!.Value);
            users[0].Username.Should().Be(userRepresentation1.Username);
            users[0].FirstName.Should().Be(userRepresentation1.FirstName);
            users[0].LastName.Should().Be(userRepresentation1.LastName);
            users[0].Email.Should().Be(userRepresentation1.Email);
            users[0].Enabled.Should().Be(userRepresentation1.Enabled!.Value);

            users[0].Attributes.Should().NotBeNull();
            users[0].Attributes.Should().HaveCount(2);
            users[0].Attributes!.ContainsKey("key11").Should().BeTrue();
            users[0].Attributes!.ContainsKey("key12").Should().BeTrue();

            users[0].Groups.Should().NotBeNull();
            users[0].Groups.Should().HaveCount(1);
            users[0].Groups!.First().Name.Should().Be("odin-group-01");

            users[1].Should().NotBeNull();
            users[1].Id.Should().Be(userRepresentation2.Id!.Value);
            users[1].Username.Should().Be(userRepresentation2.Username);
            users[1].FirstName.Should().Be(userRepresentation2.FirstName);
            users[1].LastName.Should().Be(userRepresentation2.LastName);
            users[1].Email.Should().Be(userRepresentation2.Email);
            users[1].Enabled.Should().Be(userRepresentation2.Enabled!.Value);

            users[1].Attributes.Should().NotBeNull();
            users[1].Attributes.Should().HaveCount(2);
            users[1].Attributes!.ContainsKey("key21").Should().BeTrue();
            users[1].Attributes!.ContainsKey("key22").Should().BeTrue();

            users[1].Groups.Should().NotBeNull();
            users[1].Groups.Should().HaveCount(1);
            users[1].Groups!.First().Name.Should().Be("odin-group-02");
        }

        [Fact(DisplayName = "ToUser() should map a list of UserRepresentation to list of User without groups")]
        [Trait("Keycloak", "Mappers / UserMapper")]
        public void ToUserWithoutGroupsList()
        {
            var userId1 = Guid.NewGuid();
            var userRepresentation1 = new UserRepresentation
            {
                Id = userId1,
                Username = "unit1.testing",
                FirstName = "Unit 1",
                LastName = "Testing",
                Email = "unit1.testing@email.com",
                EmailVerified = true,
                Enabled = true,
                Attributes = new Dictionary<string, List<string>> { { "key11", new List<string> { "value11" } }, { "key12", new List<string> { "value12" } } },
                Credentials = new List<CredentialRepresentation> { new CredentialRepresentation { Id = Guid.NewGuid(), Type = "password", Value = "password", Temporary = true } },
                Groups = new List<string> { "odin-group-01" }
            };

            var userId2 = Guid.NewGuid();
            var userRepresentation2 = new UserRepresentation
            {
                Id = userId2,
                Username = "uni2t.testing",
                FirstName = "Unit 2",
                LastName = "Testing",
                Email = "unit2.testing@email.com",
                EmailVerified = true,
                Enabled = true,
                Attributes = new Dictionary<string, List<string>> { { "key21", new List<string> { "value21" } }, { "key22", new List<string> { "value22" } } },
                Credentials = new List<CredentialRepresentation> { new CredentialRepresentation { Id = Guid.NewGuid(), Type = "password", Value = "password", Temporary = true } },
                Groups = new List<string> { "odin-group-01" }
            };

            var usersRepresentation = new List<UserRepresentation> { userRepresentation1, userRepresentation2 };

            var users = usersRepresentation.ToUser().ToList();

            users.Should().NotBeNull();
            users.Should().HaveCount(2);

            users[0].Should().NotBeNull();
            users[0].Id.Should().Be(userRepresentation1.Id!.Value);
            users[0].Username.Should().Be(userRepresentation1.Username);
            users[0].FirstName.Should().Be(userRepresentation1.FirstName);
            users[0].LastName.Should().Be(userRepresentation1.LastName);
            users[0].Email.Should().Be(userRepresentation1.Email);
            users[0].Enabled.Should().Be(userRepresentation1.Enabled!.Value);

            users[0].Attributes.Should().NotBeNull();
            users[0].Attributes.Should().HaveCount(2);
            users[0].Attributes!.ContainsKey("key11").Should().BeTrue();
            users[0].Attributes!.ContainsKey("key12").Should().BeTrue();

            users[1].Should().NotBeNull();
            users[1].Id.Should().Be(userRepresentation2.Id!.Value);
            users[1].Username.Should().Be(userRepresentation2.Username);
            users[1].FirstName.Should().Be(userRepresentation2.FirstName);
            users[1].LastName.Should().Be(userRepresentation2.LastName);
            users[1].Email.Should().Be(userRepresentation2.Email);
            users[1].Enabled.Should().Be(userRepresentation2.Enabled!.Value);

            users[1].Attributes.Should().NotBeNull();
            users[1].Attributes.Should().HaveCount(2);
            users[1].Attributes!.ContainsKey("key21").Should().BeTrue();
            users[1].Attributes!.ContainsKey("key22").Should().BeTrue();
        }
    }
}
