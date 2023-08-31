using Odin.Auth.Infra.Cognito;

namespace Odin.Auth.UnitTests.Application.ChangeStatusUser
{
    [CollectionDefinition(nameof(ChangeStatusUserTestFixtureCollection))]
    public class ChangeStatusUserTestFixtureCollection : ICollectionFixture<ChangeStatusUserTestFixture>
    { }

    public class ChangeStatusUserTestFixture : BaseFixture
    {
        public ChangeStatusUserTestFixture()
            : base()
        { }

        public AmazonCognitoIdentityRepositoryMock GetAwsIdentityRepository()
            => new();
    }
}
