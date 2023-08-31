using Odin.Auth.Infra.Cognito;

namespace Odin.Auth.UnitTests.Application.ChangePassword
{
    [CollectionDefinition(nameof(ChangePasswordTestFixtureCollection))]
    public class ChangePasswordTestFixtureCollection : ICollectionFixture<ChangePasswordTestFixture>
    { }

    public class ChangePasswordTestFixture : BaseFixture
    {
        public ChangePasswordTestFixture()
            : base()
        { }

        public AmazonCognitoIdentityRepositoryMock GetAwsIdentityRepository()
            => new();
    }
}
