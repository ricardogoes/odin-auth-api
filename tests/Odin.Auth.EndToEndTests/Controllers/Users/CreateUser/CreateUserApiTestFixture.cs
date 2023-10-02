using Odin.Auth.Application.Users.CreateUser;

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

        public CreateUserInput GetValidInput()
            => new
            (
                username: Faker.Person.UserName, 
                password: "password",
                passwordIsTemporary: false,
                firstName: Faker.Person.FirstName,
                lastName: Faker.Person.LastName,
                email: Faker.Person.Email, 
                groups: new List<string> { "odin.baseline" }
            );

        public CreateUserInput GetInputWithUsernameEmpty()
            => new
            (
                username: "",
                password: "password",
                passwordIsTemporary: false,
                firstName: Faker.Person.FirstName,
                lastName: Faker.Person.LastName,
                email: Faker.Person.Email,
                groups: new List<string> { "odin.baseline" }
            );

        public CreateUserInput GetInputWithPasswordEmpty()
            => new
            (
                username: Faker.Person.UserName,
                password: "",
                passwordIsTemporary: false,
                firstName: Faker.Person.FirstName,
                lastName: Faker.Person.LastName,
                email: Faker.Person.Email,
                groups: new List<string> { "odin.baseline" }
            );

        public CreateUserInput GetInputWithFirstNameEmpty()
            => new
            (
                username: Faker.Person.UserName,
                password: "password",
                passwordIsTemporary: false,
                firstName: "",
                lastName: Faker.Person.LastName,
                email: Faker.Person.Email,
                groups: new List<string> { "odin.baseline" }
            );

        public CreateUserInput GetInputWithLastNameEmpty()
            => new
            (
                username: Faker.Person.UserName,
                password: "password",
                passwordIsTemporary: false,
                firstName: Faker.Person.FirstName,
                lastName: "",
                email: Faker.Person.Email,
                groups: new List<string> { "odin.baseline" }
            );

        public CreateUserInput GetInputWithEmailEmpty()
            => new
            (
                username: Faker.Person.UserName,
                password: "password",
                passwordIsTemporary: false,
                firstName: Faker.Person.FirstName,
                lastName: Faker.Person.LastName,
                email: "",
                groups: new List<string> { "odin.baseline" }
            );

        public CreateUserInput GetInputWithGroupsEmpty()
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
