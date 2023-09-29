using Bogus.Extensions.Brazil;

namespace Odin.Auth.UnitTests.Domain.Entities.Customer
{
    public class CustomerTestFixture : BaseFixture
    {
        public CustomerTestFixture()
            : base() { }
    }        

    [CollectionDefinition(nameof(CustomerTestFixture))]
    public class CustomerTestFixtureCollection
        : ICollectionFixture<CustomerTestFixture>
    { }
}
