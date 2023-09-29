using Odin.Auth.Application.Auth.Logout;

namespace Odin.Auth.UnitTests.Application.Auth.Logout
{
    [CollectionDefinition(nameof(LogoutTestFixtureCollection))]
    public class LogoutTestFixtureCollection : ICollectionFixture<LogoutTestFixture>
    { }

    public class LogoutTestFixture : BaseFixture
    {
        public LogoutTestFixture()
            : base() { }

        public LogoutInput GetValidLogoutInput(Guid? id = null)
        {
            return new LogoutInput(id ?? Guid.NewGuid());
        }

        public LogoutInput GetInputWithEmptyUserId()
        {
            return new LogoutInput(Guid.Empty);
        }
    }
}
