using ValueObject = Odin.Auth.Domain.ValuesObjects;

namespace Odin.Auth.UnitTests.Domain.ValuesObjects.UserCredential
{
    [CollectionDefinition(nameof(UserCredentialTestFixtureCollection))]
    public class UserCredentialTestFixtureCollection : ICollectionFixture<UserCredentialTestFixture>
    { }

    public class UserCredentialTestFixture : BaseFixture
    {
        public UserCredentialTestFixture()
            : base() { }

        public ValueObject.UserCredential GetValidUserCredential()
        {
            return new ValueObject.UserCredential(value: "password", temporary: true);
        }
    }
}
