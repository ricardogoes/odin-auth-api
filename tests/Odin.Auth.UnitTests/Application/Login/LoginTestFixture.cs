using Odin.Auth.Infra.Cognito;

namespace Odin.Auth.UnitTests.Application.Login
{
    [CollectionDefinition(nameof(LoginTestFixtureCollection))]
    public class LoginTestFixtureCollection : ICollectionFixture<LoginTestFixture>
    { }

    public class LoginTestFixture : BaseFixture
    {
        public LoginTestFixture()
            : base()
        { }

        public AmazonCognitoIdentityRepositoryMock GetAwsIdentityRepository()
            => new();
    }
}
