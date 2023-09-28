using Odin.Auth.UnitTests.Application.Customers;
using Odin.Auth.Application.Customers.GetCustomerById;

namespace Odin.Auth.UnitTests.Application.Customers.GetCustomerById
{
    [CollectionDefinition(nameof(GetCustomerByIdTestFixture))]
    public class GetCustomerByIdTestFixtureCollection :ICollectionFixture<GetCustomerByIdTestFixture>
    { }

    public class GetCustomerByIdTestFixture: CustomerBaseFixture
    {
        public GetCustomerByIdInput GetValidGetCustomerByIdInput(Guid? id = null)
            => new()
            {
                Id = id ?? Guid.NewGuid()
            };
    }
}
