using Odin.Auth.Application.Customers.ChangeStatusCustomer;
using Odin.Auth.Domain.Enums;

namespace Odin.Auth.UnitTests.Application.Customers.ChangeStatusCustomer
{
    [CollectionDefinition(nameof(ChangeStatusCustomerTestFixtureCollection))]
    public class ChangeStatusCustomerTestFixtureCollection : ICollectionFixture<ChangeStatusCustomerTestFixture>
    { }

    public class ChangeStatusCustomerTestFixture : CustomerBaseFixture
    {
        public ChangeStatusCustomerTestFixture()
            : base() { }

        public ChangeStatusCustomerInput GetValidChangeStatusCustomerInputToActivate(Guid? id = null)
            => new
            (            
                id ?? Guid.NewGuid(),
                ChangeStatusAction.ACTIVATE
            );

        public ChangeStatusCustomerInput GetValidChangeStatusCustomerInputToDeactivate(Guid? id = null)
            => new
            (
                id ?? Guid.NewGuid(),
                ChangeStatusAction.DEACTIVATE
            );

        public ChangeStatusCustomerInput GetChangeStatusCustomerInputWithEmptyAction(Guid? id = null)
          => new
          (
              id ?? Guid.NewGuid(),
              null
          );
    }
}
