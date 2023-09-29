using Bogus;
using Odin.Auth.Application.Users.CreateUser;

namespace Odin.Auth.UnitTests.Application.Users.CreateUser
{
    [CollectionDefinition(nameof(CreateUserTestFixtureCollection))]
    public class CreateUserTestFixtureCollection : ICollectionFixture<CreateUserTestFixture>
    { }

    public class CreateUserTestFixture : BaseFixture
    {
        public CreateUserTestFixture()
            : base() { }

        public CreateUserInput GetValidCreateUserInput()
        {
            return new CreateUserInput
            (
                tenantId: Guid.NewGuid(),
                username: Faker.Person.UserName,
                password: "password",
                passwordIsTemporary: true,
                firstName: Faker.Person.FirstName,
                lastName: Faker.Person.LastName,
                email: Faker.Person.Email,
                groups: new List<string> { "role-01 " },
                loggedUsername: "admin"
            );
        }

        public CreateUserInput GetInputWithEmptyUsername()
        {
            return new CreateUserInput
            (
                tenantId: Guid.NewGuid(),
                username: "",
                password: "password",
                passwordIsTemporary: true,
                firstName: Faker.Person.FirstName,
                lastName: Faker.Person.LastName,
                email: Faker.Person.Email,
                groups: new List<string> { "role-01 " },
                loggedUsername: "admin"
            );
        }

        public CreateUserInput GetInputWithEmptyPassword()
        {
            return new CreateUserInput
            (
                tenantId: Guid.NewGuid(),
                username: Faker.Person.UserName,
                password: "",
                passwordIsTemporary: true,
                firstName: Faker.Person.FirstName,
                lastName: Faker.Person.LastName,
                email: Faker.Person.Email,
                groups: new List<string> { "role-01 " },
                loggedUsername: "admin"
            );
        }

        public CreateUserInput GetInputWithEmptyFirstName()
        {
            return new CreateUserInput
            (
                tenantId: Guid.NewGuid(),
                username: Faker.Person.UserName,
                password: "password",
                passwordIsTemporary: true,
                firstName: "",
                lastName: Faker.Person.LastName,
                email: Faker.Person.Email,
                groups: new List<string> { "role-01 " },
                loggedUsername: "admin"
            );
        }

        public CreateUserInput GetInputWithEmptyLastName()
        {
            return new CreateUserInput
            (
                tenantId: Guid.NewGuid(),
                username: Faker.Person.UserName,
                password: "password",
                passwordIsTemporary: true,
                firstName: Faker.Person.FirstName,
                lastName: "",
                email: Faker.Person.Email,
                groups: new List<string> { "role-01 " },
                loggedUsername: "admin"
            );
        }

        public CreateUserInput GetInputWithEmptyEmail()
        {
            return new CreateUserInput
            (
                tenantId: Guid.NewGuid(),
                username: Faker.Person.UserName,
                password: "password",
                passwordIsTemporary: true,
                firstName: Faker.Person.FirstName,
                lastName: Faker.Person.LastName,
                email: "",
                groups: new List<string> { "role-01 " },
                loggedUsername: "admin"
            );
        }

        public CreateUserInput GetInputWithEmptyRole()
        {
            return new CreateUserInput
            (
                tenantId: Guid.NewGuid(),
                username: Faker.Person.UserName,
                password: "password",
                passwordIsTemporary: true,
                firstName: Faker.Person.FirstName,
                lastName: Faker.Person.LastName,
                email: Faker.Person.Email,
                groups: new List<string>(),
                loggedUsername: "admin"
            );
        }
    }
}
