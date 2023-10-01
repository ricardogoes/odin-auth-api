using Odin.Auth.Application.Auth.ChangePassword;

namespace Odin.Auth.EndToEndTests.Controllers.Auth.ChangePassword
{
    [CollectionDefinition(nameof(ChangePasswordTestFixtureCollection))]
    public class ChangePasswordTestFixtureCollection : ICollectionFixture<ChangePasswordApiTestFixture>
    { }

    public class ChangePasswordApiTestFixture : AuthBaseFixture
    {


        public ChangePasswordApiTestFixture()
            : base()
        { }

        public ChangePasswordInput GetValidChangePasswordInput()
        {
            return new ChangePasswordInput(CommonUserId, "admin", temporary: false);
        }
    }
}
