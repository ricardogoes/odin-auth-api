using Odin.Auth.Infra.Keycloak.Models;

namespace Odin.Auth.UnitTests.Keycloak.Repositories.User
{
    [CollectionDefinition(nameof(UserKeycloakRepositoryTestFixtureCollection))]
    public class UserKeycloakRepositoryTestFixtureCollection : ICollectionFixture<UserKeycloakRepositoryTestFixture>
    { }

    public class UserKeycloakRepositoryTestFixture : KeycloakRepositoryBaseFixture
    {
        public UserKeycloakRepositoryTestFixture()
            : base()
        { }
    }
}
