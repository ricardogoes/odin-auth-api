namespace Odin.Auth.EndToEndTests.Controllers.Users.GetUserById
{
    [CollectionDefinition(nameof(GetUserByIdApiTestCollection))]
    public class GetUserByIdApiTestCollection : ICollectionFixture<GetUserByIdApiTestFixture>
    { }

    public class GetUserByIdApiTestFixture : UserBaseFixture
    {
        public GetUserByIdApiTestFixture()
            : base()
        { }
    }
}
