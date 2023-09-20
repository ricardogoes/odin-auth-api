using ValueObject = Odin.Auth.Domain.ValuesObjects;

namespace Odin.Auth.UnitTests.Domain.ValuesObjects.UserGroup
{
    [CollectionDefinition(nameof(UserGroupTestFixtureCollection))]
    public class UserGroupTestFixtureCollection : ICollectionFixture<UserGroupTestFixture>
    { }

    public class UserGroupTestFixture : BaseFixture
    {
        public UserGroupTestFixture()
            : base() { }

        public ValueObject.UserGroup GetValidUserGroup()
        {
            return new ValueObject.UserGroup(id: Guid.NewGuid(), name: "odin-group", path: "/odin-group");
        }
    }
}
