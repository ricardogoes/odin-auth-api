using Odin.Auth.Application.Auth.ChangePassword;

namespace Odin.Auth.UnitTests.Application.Auth.ChangePassword
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
                tenantId: Guid.NewGuid(),
                userId: userId ?? Guid.NewGuid(),
                newPassword: "new-password",
                temporary: true
            );
        }

        public ChangePasswordInput GetInputWithEmptyUserId()
        {
            return new ChangePasswordInput
            (
                tenantId: Guid.NewGuid(),
                userId: Guid.Empty,
                newPassword: "new-password",
                temporary: true
            );
        }

        public ChangePasswordInput GetInputWithEmptyNewPassword()
        {
            return new ChangePasswordInput
            (
                tenantId: Guid.NewGuid(),
                userId: Guid.NewGuid(),
                newPassword: "",
                temporary: true
            );
        }

        public ChangePasswordInput GetInputWithEmptyTenantId()
        {
            return new ChangePasswordInput
            (
                tenantId: Guid.Empty,
                userId: Guid.NewGuid(),
                newPassword: "new-password",
                temporary: true
            );
        }
    }
}
