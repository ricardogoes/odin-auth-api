using Odin.Auth.Application.Auth.Login;
using Odin.Auth.EndToEndTests.Controllers.Auth;

namespace Odin.Auth.EndToEndTests.Controllers.Auth.SignIn
{
    [CollectionDefinition(nameof(SignInTestFixtureCollection))]
    public class SignInTestFixtureCollection : ICollectionFixture<SignInApiTestFixture>
    { }

    public class SignInApiTestFixture : AuthBaseFixture
    {
        public SignInApiTestFixture()
            : base()
        { }

        public LoginInput GetInvalidLoginInput()
        {
            return new LoginInput("admin1", "admin1");
        }
    }
}
