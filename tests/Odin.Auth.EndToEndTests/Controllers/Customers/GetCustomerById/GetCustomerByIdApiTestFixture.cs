using Odin.Auth.EndToEndTests.Controllers.Customers;

namespace Odin.Auth.EndToEndTests.Controllers.Customers.GetCustomerById
{
    [CollectionDefinition(nameof(GetCustomerByIdApiTestCollection))]
    public class GetCustomerByIdApiTestCollection : ICollectionFixture<GetCustomerByIdApiTestFixture>
    { }

    public class GetCustomerByIdApiTestFixture : CustomerBaseFixture
    {
        public GetCustomerByIdApiTestFixture()
            : base()
        { }
    }
}
