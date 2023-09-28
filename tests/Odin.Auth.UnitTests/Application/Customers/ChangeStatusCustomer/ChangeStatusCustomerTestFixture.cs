using Odin.Auth.UnitTests.Application.Customers;
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
                ChangeStatusAction.ACTIVATE,
                "unit.testing"
            );

        public ChangeStatusCustomerInput GetValidChangeStatusCustomerInputToDeactivate(Guid? id = null)
            => new
            (
                id ?? Guid.NewGuid(),
                ChangeStatusAction.DEACTIVATE,
                "unit.testing"
            );

        public ChangeStatusCustomerInput GetChangeStatusCustomerInputWithEmptyAction(Guid? id = null)
          => new
          (
              id ?? Guid.NewGuid(),
              null,
              "unit.testing"
          );

        public ChangeStatusCustomerInput GetChangeStatusCustomerInputWithEmptyLoggedUsername(Guid? id = null)
           => new
           (
               id ?? Guid.NewGuid(),
               ChangeStatusAction.ACTIVATE,
               ""
           );
    }
}
