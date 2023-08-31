using Odin.Auth.Infra.Cognito;

namespace Odin.Auth.UnitTests.Application.Common
{
    [CollectionDefinition(nameof(CommonServiceTestFixtureCollection))]
    public class CommonServiceTestFixtureCollection : ICollectionFixture<CommonServiceTestFixture>
    { }

    public class CommonServiceTestFixture : BaseFixture
    {
        public CommonServiceTestFixture()
            : base()
        { }

        public AmazonCognitoIdentityRepositoryMock GetAwsIdentityRepository()
            => new();
    }
}
