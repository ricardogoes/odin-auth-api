using Odin.Auth.Application.Auth.Logout;

namespace Odin.Auth.EndToEndTests.Controllers.Auth.SignOut
{
    [CollectionDefinition(nameof(SignOutTestFixtureCollection))]
    public class SignOutTestFixtureCollection : ICollectionFixture<SignOutAPiTestFixture>
    { }

    public class SignOutAPiTestFixture : AuthBaseFixture
    {
        public SignOutAPiTestFixture()
            : base()
        { }

        public LogoutInput GetValidLogoutInput()
        {
            return new LogoutInput(RealmAdminUserId);
        }
    }
}
