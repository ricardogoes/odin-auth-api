using Odin.Auth.Application.ChangeStatusUser;

namespace Odin.Auth.UnitTests.Application.ChangeStatusUser
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
                id ?? Guid.NewGuid(),
                ChangeStatusAction.ACTIVATE,
                loggedUsername: "admin"
            );

        public ChangeStatusUserInput GetValidChangeStatusUserInputToDeactivate(Guid? id = null)
            => new
            (
                id ?? Guid.NewGuid(),
                ChangeStatusAction.DEACTIVATE,
                loggedUsername: "admin"
            );

        public ChangeStatusUserInput GetChangeStatusUserInputWithEmptyAction(Guid? id = null)
          => new
          (
              id ?? Guid.NewGuid(),
              null,
              loggedUsername: "admin"
          );
    }
}
