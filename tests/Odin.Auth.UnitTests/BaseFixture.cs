using Bogus;
using Moq;
using Odin.Auth.Domain.Entities;
using Odin.Auth.Domain.Interfaces;
using Odin.Auth.Domain.ValuesObjects;

namespace Odin.Auth.UnitTests
{
    public abstract class BaseFixture
    {
        public Faker Faker { get; set; }

        protected BaseFixture()
            => Faker = new Faker("pt_BR");

        public Mock<IKeycloakRepository> GetKeycloakRepositoryMock()
            => new();

        public User GetValidUser()
        {
            var user = new User
            (
                Faker.Person.UserName,
                Faker.Person.FirstName,
                Faker.Person.LastName,
                Faker.Person.Email
            );

            user.AddGroup(new UserGroup(Guid.NewGuid(), "odin-group", "/odin-group"));

            return user;
        }
    }
}
