using Odin.Auth.EndToEndTests.Controllers.Customers;
using Odin.Auth.Infra.Data.EF.Models;

namespace Odin.Auth.EndToEndTests.Controllers.Customers.GetCustomers
{
    [CollectionDefinition(nameof(GetCustomersApiTestCollection))]
    public class GetCustomersApiTestCollection : ICollectionFixture<GetCustomersApiTestFixture>
    { }

    public class GetCustomersApiTestFixture : CustomerBaseFixture
    {
        public GetCustomersApiTestFixture()
            : base()
        { }
    }
}
