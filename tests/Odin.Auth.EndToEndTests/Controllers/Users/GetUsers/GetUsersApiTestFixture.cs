namespace Odin.Auth.EndToEndTests.Controllers.Users.GetUsers
{
    [CollectionDefinition(nameof(GetUsersApiTestCollection))]
    public class GetUsersApiTestCollection : ICollectionFixture<GetUsersApiTestFixture>
    { }

    public class GetUsersApiTestFixture : UserBaseFixture
    {
        public GetUsersApiTestFixture()
            : base()
        { }        
    }
}
