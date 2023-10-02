using Odin.Auth.Application.Customers.CreateCustomer;

namespace Odin.Auth.EndToEndTests.Controllers.Customers.CreateCustomer
{
    [CollectionDefinition(nameof(CreateCustomerApiTestCollection))]
    public class CreateCustomerApiTestCollection : ICollectionFixture<CreateCustomerApiTestFixture>
    { }

    public class CreateCustomerApiTestFixture : CustomerBaseFixture
    {
        public CreateCustomerApiTestFixture()
            : base()
        { }

        public CreateCustomerInput GetValidInput()
            => new
            (
                name: GetValidName(),
                document: GetValidDocument(),
                address: GetValidAddress()
            );

        public CreateCustomerInput GetInputWithNameEmpty()
            => new
            (
                name: "",
                document: GetValidDocument(),
                address: GetValidAddress()
            );

        public CreateCustomerInput GetInputWithDocumentEmpty()
            => new
            (
                name: GetValidName(),
                document: "",
                address: GetValidAddress()
            );

        public CreateCustomerInput GetInputWithInvalidDocument()
            => new
            (
                name: GetValidName(),
                document: GetInvalidDocument(),
                address: GetValidAddress()
            );
    }
}
