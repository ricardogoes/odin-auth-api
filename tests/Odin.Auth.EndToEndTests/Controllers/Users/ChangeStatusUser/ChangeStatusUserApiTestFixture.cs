using Odin.Auth.Application.ChangeStatusUser;

namespace Odin.Auth.EndToEndTests.Controllers.Users.ChangeStatusUser
{
    [CollectionDefinition(nameof(ChangeStatusUserApiTestCollection))]
    public class ChangeStatusUserApiTestCollection : ICollectionFixture<ChangeStatusUserApiTestFixture>
    { }

    public class ChangeStatusUserApiTestFixture : UserBaseFixture
    {
        public ChangeStatusUserApiTestFixture()
            : base()
        { }

        public ChangeStatusUserInput GetValidInputToActivate(Guid? id = null)
            => new
            (
                id ?? Guid.NewGuid(),
                ChangeStatusAction.ACTIVATE,
                "admin"
            );

        public ChangeStatusUserInput GetValidInputToDeactivate(Guid? id = null)
            => new
            (
                id ?? Guid.NewGuid(),
                ChangeStatusAction.DEACTIVATE,
                "admin"
            );

        public ChangeStatusUserInput GetInputWithInvalidAction(Guid? id = null)
            => new
            (
                id ?? Guid.NewGuid(),
                ChangeStatusAction.INVALID,
                "admin"
            );
    }
}
