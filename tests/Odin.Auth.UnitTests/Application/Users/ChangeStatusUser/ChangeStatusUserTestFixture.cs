using Odin.Auth.Application.Users.ChangeStatusUser;
using Odin.Auth.Domain.Enums;

namespace Odin.Auth.UnitTests.Application.Users.ChangeStatusUser
{
    [CollectionDefinition(nameof(ChangeStatusUserTestFixtureCollection))]
    public class ChangeStatusUserTestFixtureCollection : ICollectionFixture<ChangeStatusUserTestFixture>
    { }

    public class ChangeStatusUserTestFixture : BaseFixture
    {
        public ChangeStatusUserTestFixture()
            : base() { }

        public ChangeStatusUserInput GetValidChangeStatusUserInputToActivate(Guid? id = null)
            => new
            (
               Guid.NewGuid(),
                id ?? Guid.NewGuid(),
                ChangeStatusAction.ACTIVATE,
                loggedUsername: "admin"
            );

        public ChangeStatusUserInput GetValidChangeStatusUserInputToDeactivate(Guid? id = null)
            => new
            (
               Guid.NewGuid(),
                id ?? Guid.NewGuid(),
                ChangeStatusAction.DEACTIVATE,
                loggedUsername: "admin"
            );

        public ChangeStatusUserInput GetChangeStatusUserInputWithEmptyAction(Guid? id = null)
          => new
          (
             Guid.NewGuid(),
              id ?? Guid.NewGuid(),
              null,
              loggedUsername: "admin"
          );
    }
}
