using Odin.Auth.Application.Customers.CreateCustomer;

namespace Odin.Auth.UnitTests.Application.Customers.CreateCustomer
{
    [CollectionDefinition(nameof(CreateCustomerTestFixtureCollection))]
    public class CreateCustomerTestFixtureCollection : ICollectionFixture<CreateCustomerTestFixture>
    { }

    public class CreateCustomerTestFixture : CustomerBaseFixture
    {
        public CreateCustomerTestFixture()
            : base() { }

        public CreateCustomerInput GetValidCreateCustomerInputWithoutAddress()
            => new
            (
                GetValidCustomerName(),
                GetValidCustomerDocument(),
                null,
                GetValidUsername()
            );

        public CreateCustomerInput GetValidCreateCustomerInputWithAddress()
            => new
            (
                GetValidCustomerName(),
                GetValidCustomerDocument(),
                GetValidAddress(),
                GetValidUsername()
            );

        public CreateCustomerInput GetCreateCustomerInputWithEmptyName()
            => new
            (
                "",
                GetValidCustomerDocument(),
                GetValidAddress(),
                GetValidUsername()
            );


        public CreateCustomerInput GetCreateCustomerInputWithEmptyDocument()
            => new
            (
                GetValidCustomerName(),
                "",
                GetValidAddress(),
                GetValidUsername()
            );

        public CreateCustomerInput GetCreateCustomerInputWithInvalidDocument()
            => new
            (
                GetValidCustomerName(),
                "12.132.432/0002-12",
                GetValidAddress(),
                GetValidUsername()
            );

        public CreateCustomerInput GetCreateCustomerInputWithEmptyLoggedUsername()
           => new
           (
               GetValidCustomerName(),
               GetValidCustomerDocument(),
                GetValidAddress(),
               ""
           );
    }


}
