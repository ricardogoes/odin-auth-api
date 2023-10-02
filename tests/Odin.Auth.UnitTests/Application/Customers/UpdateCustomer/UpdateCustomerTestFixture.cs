using Odin.Auth.Application.Customers.UpdateCustomer;

namespace Odin.Auth.UnitTests.Application.Customers.UpdateCustomer
{
    [CollectionDefinition(nameof(UpdateCustomerTestFixtureCollection))]
    public class UpdateCustomerTestFixtureCollection : ICollectionFixture<UpdateCustomerTestFixture>
    { }

    public class UpdateCustomerTestFixture : CustomerBaseFixture
    {
        public UpdateCustomerTestFixture()
            : base() { }

        public UpdateCustomerInput GetValidUpdateCustomerInput(Guid? id = null)
            => new
            (
                id ?? Guid.NewGuid(),
                GetValidCustomerName(),
                GetValidCustomerDocument()
            );

        public UpdateCustomerInput GetUpdateCustomerInputWithEmptyName(Guid? id = null)
            => new
            (
                id ?? Guid.NewGuid(),
                "",
                GetValidCustomerDocument()
            );


        public UpdateCustomerInput GetUpdateCustomerInputWithEmptyDocument(Guid? id = null)
            => new
            (
                id ?? Guid.NewGuid(),
                GetValidCustomerName(),
                ""
            );

        public UpdateCustomerInput GetUpdateCustomerInputWithInvalidDocument(Guid? id = null)
            => new
            (
                id ?? Guid.NewGuid(),
                GetValidCustomerName(),
                "12.312.534/0002-12"
            );
    }
}
