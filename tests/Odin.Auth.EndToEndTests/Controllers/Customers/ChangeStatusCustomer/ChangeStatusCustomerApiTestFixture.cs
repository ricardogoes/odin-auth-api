using Odin.Auth.Application.Customers.ChangeStatusCustomer;
using Odin.Auth.Domain.Enums;

namespace Odin.Auth.EndToEndTests.Controllers.Customers.ChangeStatusCustomer
{
    [CollectionDefinition(nameof(ChangeStatusCustomerApiTestCollection))]
    public class ChangeStatusCustomerApiTestCollection : ICollectionFixture<ChangeStatusCustomerApiTestFixture>
    { }

    public class ChangeStatusCustomerApiTestFixture : CustomerBaseFixture
    {
        public ChangeStatusCustomerApiTestFixture()
            : base()
        { }

        public ChangeStatusCustomerInput GetValidInputToActivate(Guid? id = null)
            => new
            (
                id ?? Guid.NewGuid(),
                ChangeStatusAction.ACTIVATE
            );

        public ChangeStatusCustomerInput GetValidInputToDeactivate(Guid? id = null)
            => new
            (
                id ?? Guid.NewGuid(),
                ChangeStatusAction.DEACTIVATE
            );

        public ChangeStatusCustomerInput GetInputWithInvalidAction(Guid? id = null)
            => new
            (
                id ?? Guid.NewGuid(),
                ChangeStatusAction.INVALID
            );
    }
}
