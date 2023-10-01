using Odin.Auth.Application.Customers.UpdateCustomer;

namespace Odin.Auth.EndToEndTests.Controllers.Customers.UpdateCustomer
{
    [CollectionDefinition(nameof(UpdateCustomerApiTestCollection))]
    public class UpdateCustomerApiTestCollection : ICollectionFixture<UpdateCustomerApiTestFixture>
    { }

    public class UpdateCustomerApiTestFixture : CustomerBaseFixture
    {
        public UpdateCustomerApiTestFixture()
            : base()
        { }

        public UpdateCustomerInput GetValidInput(Guid id)
            => new
            (
                id: id,
                name: GetValidName(),
                document: GetValidDocument()
            );

        public UpdateCustomerInput GetInputWithIdEmpty()
            => new
            (
                id: Guid.Empty,
                name: GetValidName(),
                document: GetValidDocument()
            );


        public UpdateCustomerInput GetInputWithNameEmpty(Guid id)
            => new
            (
                id: id,
                name: "",
                document: GetValidDocument()
            );

        public UpdateCustomerInput GetInputWithDocumentEmpty(Guid id)
            => new
            (
                id: id,
                name: GetValidName(),
                document: ""
            );

        public UpdateCustomerInput GetInputWithInvalidDocument(Guid id)
            => new
            (
                id: id,
                name: GetValidName(),
                document: GetInvalidDocument()
            );
    }
}
