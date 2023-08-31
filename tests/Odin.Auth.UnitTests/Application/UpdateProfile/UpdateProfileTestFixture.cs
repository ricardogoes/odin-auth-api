using Odin.Auth.Infra.Cognito;

namespace Odin.Auth.UnitTests.Application.UpdateProfile
{
    [CollectionDefinition(nameof(UpdateProfileTestFixtureCollection))]
    public class UpdateProfileTestFixtureCollection : ICollectionFixture<UpdateProfileTestFixture>
    { }

    public class UpdateProfileTestFixture : BaseFixture
    {
        public UpdateProfileTestFixture()
            : base()
        { }

        public AmazonCognitoIdentityRepositoryMock GetAwsIdentityRepository()
            => new();
    }
}
