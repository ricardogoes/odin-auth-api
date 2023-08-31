using Odin.Auth.Infra.Cognito;

namespace Odin.Auth.UnitTests.Application.ResetPassword
{
    [CollectionDefinition(nameof(ResetPasswordTestFixtureCollection))]
    public class ResetPasswordTestFixtureCollection : ICollectionFixture<ResetPasswordTestFixture>
    { }

    public class ResetPasswordTestFixture : BaseFixture
    {
        public ResetPasswordTestFixture()
            : base()
        { }

        public AmazonCognitoIdentityRepositoryMock GetAwsIdentityRepository()
            => new();
    }
}
