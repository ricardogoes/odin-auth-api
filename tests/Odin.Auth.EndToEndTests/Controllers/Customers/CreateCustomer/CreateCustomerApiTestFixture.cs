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
                address: GetValidAddress(),
                loggedUsername: GetValidUsername()
            );

        public CreateCustomerInput GetInputWithNameEmpty()
            => new
            (
                name: "",
                document: GetValidDocument(),
                address: GetValidAddress(),
                loggedUsername: GetValidUsername()
            );

        public CreateCustomerInput GetInputWithDocumentEmpty()
            => new
            (
                name: GetValidName(),
                document: "",
                address: GetValidAddress(),
                loggedUsername: GetValidUsername()
            );

        public CreateCustomerInput GetInputWithInvalidDocument()
            => new
            (
                name: GetValidName(),
                document: GetInvalidDocument(),
                address: GetValidAddress(),
                loggedUsername: GetValidUsername()
            );

        public CreateCustomerInput GetInputWithUsernameEmpty()
            => new
            (
                name: GetValidName(),
                document: GetValidDocument(),
                address: GetValidAddress(),
                loggedUsername: ""
            );
    }
}
