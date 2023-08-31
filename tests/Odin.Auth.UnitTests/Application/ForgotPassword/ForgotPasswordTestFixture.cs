using Odin.Auth.Infra.Cognito;

namespace Odin.Auth.UnitTests.Application.ForgotPassword
{
    [CollectionDefinition(nameof(ForgotPasswordTestFixtureCollection))]
    public class ForgotPasswordTestFixtureCollection : ICollectionFixture<ForgotPasswordTestFixture>
    { }

    public class ForgotPasswordTestFixture : BaseFixture
    {
        public ForgotPasswordTestFixture()
            : base()
        { }

        public AmazonCognitoIdentityRepositoryMock GetAwsIdentityRepository()
            => new();
    }
}
