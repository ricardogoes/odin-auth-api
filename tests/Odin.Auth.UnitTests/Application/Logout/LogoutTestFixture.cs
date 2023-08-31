using Odin.Auth.Infra.Cognito;

namespace Odin.Auth.UnitTests.Application.Logout
{
    [CollectionDefinition(nameof(LogoutTestFixtureCollection))]
    public class LogoutTestFixtureCollection : ICollectionFixture<LogoutTestFixture>
    { }

    public class LogoutTestFixture : BaseFixture
    {
        public LogoutTestFixture()
            : base()
        { }

        public AmazonCognitoIdentityRepositoryMock GetAwsIdentityRepository()
            => new();
    }
}
