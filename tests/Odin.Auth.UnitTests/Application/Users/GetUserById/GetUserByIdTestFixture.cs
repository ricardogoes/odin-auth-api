using Odin.Auth.Application.Users.GetUserById;

namespace Odin.Auth.UnitTests.Application.Users.GetUserById
{
    [CollectionDefinition(nameof(GetUserByIdTestFixture))]
    public class GetUserByIdTestFixtureCollection : ICollectionFixture<GetUserByIdTestFixture>
    { }

    public class GetUserByIdTestFixture : BaseFixture
    {
        public GetUserByIdInput GetValidGetUserByIdInput(Guid? id = null)
            => new
            (
                tenantId: Guid.NewGuid(),
                userId: id ?? Guid.NewGuid()
            );

        public GetUserByIdInput GetInputWithEmtptyTenantId()
            => new
            (
                tenantId: Guid.Empty,
                userId: Guid.NewGuid()
            );
    }
}
