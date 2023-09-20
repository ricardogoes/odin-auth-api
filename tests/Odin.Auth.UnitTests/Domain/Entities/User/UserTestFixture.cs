using DomainEntity = Odin.Auth.Domain.Entities;

namespace Odin.Auth.UnitTests.Domain.Entities.User
{
    [CollectionDefinition(nameof(UserTestFixtureCollection))]
    public class UserTestFixtureCollection : ICollectionFixture<UserTestFixture>
    { }

    public class UserTestFixture : BaseFixture
    {
        public UserTestFixture()
            : base() { }
    }   
}
