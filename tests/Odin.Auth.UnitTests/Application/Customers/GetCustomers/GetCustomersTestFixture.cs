using Odin.Auth.UnitTests.Application.Customers;

namespace Odin.Auth.UnitTests.Application.Customers.GetCustomers
{
    [CollectionDefinition(nameof(GetCustomerByIdTestFixtureCollection))]
    public class GetCustomerByIdTestFixtureCollection : ICollectionFixture<GetCustomersTestFixture>
    { }

    public class GetCustomersTestFixture : CustomerBaseFixture
    { }
}
