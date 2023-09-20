using Odin.Auth.Api.Models;

namespace Odin.Auth.EndToEndTests.Controllers.Users.CreateUser
{
    [CollectionDefinition(nameof(CreateUserApiTestCollection))]
    public class CreateUserApiTestCollection : ICollectionFixture<CreateUserApiTestFixture>
    { }

    public class CreateUserApiTestFixture : UserBaseFixture
    {
        public CreateUserApiTestFixture()
            : base()
        { }

        public CreateUserApiRequest GetValidInput()
            => new
            (
                username: Faker.Person.UserName, 
                password: "password",
                passwordIsTemporary: false,
                firstName: Faker.Person.FirstName,
                lastName: Faker.Person.LastName,
                email: Faker.Person.Email, 
                groups: new List<string> { "common-user" }
            );

        public CreateUserApiRequest GetInputWithUsernameEmpty()
            => new
            (
                username: "",
                password: "password",
                passwordIsTemporary: false,
                firstName: Faker.Person.FirstName,
                lastName: Faker.Person.LastName,
                email: Faker.Person.Email,
                groups: new List<string> { "common-user" }
            );

        public CreateUserApiRequest GetInputWithPasswordEmpty()
            => new
            (
                username: Faker.Person.UserName,
                password: "",
                passwordIsTemporary: false,
                firstName: Faker.Person.FirstName,
                lastName: Faker.Person.LastName,
                email: Faker.Person.Email,
                groups: new List<string> { "common-user" }
            );

        public CreateUserApiRequest GetInputWithFirstNameEmpty()
            => new
            (
                username: Faker.Person.UserName,
                password: "password",
                passwordIsTemporary: false,
                firstName: "",
                lastName: Faker.Person.LastName,
                email: Faker.Person.Email,
                groups: new List<string> { "common-user" }
            );

        public CreateUserApiRequest GetInputWithLastNameEmpty()
            => new
            (
                username: Faker.Person.UserName,
                password: "password",
                passwordIsTemporary: false,
                firstName: Faker.Person.FirstName,
                lastName: "",
                email: Faker.Person.Email,
                groups: new List<string> { "common-user" }
            );

        public CreateUserApiRequest GetInputWithEmailEmpty()
            => new
            (
                username: Faker.Person.UserName,
                password: "password",
                passwordIsTemporary: false,
                firstName: Faker.Person.FirstName,
                lastName: Faker.Person.LastName,
                email: "",
                groups: new List<string> { "common-user" }
            );

        public CreateUserApiRequest GetInputWithGroupsEmpty()
            => new
            (
                username: Faker.Person.UserName,
                password: "password",
                passwordIsTemporary: false,
                firstName: Faker.Person.FirstName,
                lastName: Faker.Person.LastName,
                email: Faker.Person.Email,
                groups: new List<string>()
            );
    }
}
