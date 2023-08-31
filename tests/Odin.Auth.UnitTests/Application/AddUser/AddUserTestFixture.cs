using Moq;
using Odin.Auth.Infra.Cognito;

namespace Odin.Auth.UnitTests.Application.AddUser
{
    [CollectionDefinition(nameof(AddUserTestFixtureCollection))]
    public class AddUserTestFixtureCollection : ICollectionFixture<AddUserTestFixture>
    { }

    public class AddUserTestFixture : BaseFixture
    {
        public AddUserTestFixture()
            : base()
        { }

        public AmazonCognitoIdentityRepositoryMock GetAwsIdentityRepository()
            => new();
    }
}
