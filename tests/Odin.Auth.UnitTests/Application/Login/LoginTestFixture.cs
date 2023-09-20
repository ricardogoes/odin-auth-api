using Bogus;
using Odin.Auth.Application.Login;

namespace Odin.Auth.UnitTests.Application.Login
{
    [CollectionDefinition(nameof(LoginTestFixtureCollection))]
    public class LoginTestFixtureCollection : ICollectionFixture<LoginTestFixture>
    { }

    public class LoginTestFixture : BaseFixture
    {
        public LoginTestFixture()
            : base() { }

        public LoginInput GetValidLoginInput()
        {
            return new LoginInput
            (
                username: Faker.Person.UserName,
                password: "password"
            );
        }

        public LoginInput GetInputWithEmptyUsername()
        {
            return new LoginInput
            (
                username: "",
                password: "password"
            );
        }

        public LoginInput GetInputWithEmptyPassword()
        {
            return new LoginInput
            (
                username: Faker.Person.UserName,
                password: ""
            );
        }
    }
}
