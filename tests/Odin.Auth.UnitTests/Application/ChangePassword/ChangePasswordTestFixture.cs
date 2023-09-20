using Odin.Auth.Application.ChangePassword;

namespace Odin.Auth.UnitTests.Application.ChangePassword
{
    [CollectionDefinition(nameof(ChangePasswordTestFixtureCollection))]
    public class ChangePasswordTestFixtureCollection : ICollectionFixture<ChangePasswordTestFixture>
    { }

    public class ChangePasswordTestFixture : BaseFixture
    {
        public ChangePasswordTestFixture()
            : base() { }

        public ChangePasswordInput GetValidChangePasswordInput(Guid? userId = null)
        {
            return new ChangePasswordInput
            (
                userId: userId ?? Guid.NewGuid(),
                newPassword: "new-password",
                temporary: true
            );
        }

        public ChangePasswordInput GetInputWithEmptyUserId()
        {
            return new ChangePasswordInput
            (
                userId: Guid.Empty,
                newPassword: "new-password",
                temporary: true
            );
        }

        public ChangePasswordInput GetInputWithEmptyNewPassword()
        {
            return new ChangePasswordInput
            (
                userId: Guid.NewGuid(),
                newPassword: "",
                temporary: true
            );
        }
    }
}
