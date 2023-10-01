using Odin.Auth.Application.Users.ChangeStatusUser;
using Odin.Auth.Domain.Enums;

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
                ChangeStatusAction.ACTIVATE
            );

        public ChangeStatusUserInput GetValidInputToDeactivate(Guid? id = null)
            => new
            (
                id ?? Guid.NewGuid(),
                ChangeStatusAction.DEACTIVATE
            );

        public ChangeStatusUserInput GetInputWithInvalidAction(Guid? id = null)
            => new
            (
                id ?? Guid.NewGuid(),
                ChangeStatusAction.INVALID
            );
    }
}
