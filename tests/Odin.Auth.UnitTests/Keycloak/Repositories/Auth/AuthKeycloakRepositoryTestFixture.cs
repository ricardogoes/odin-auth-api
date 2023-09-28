using Odin.Auth.Domain.Models.AppSettings;

namespace Odin.Auth.UnitTests.Keycloak.Repositories.Auth
{
    [CollectionDefinition(nameof(AuthKeycloakRepositoryTestFixtureCollection))]
    public class AuthKeycloakRepositoryTestFixtureCollection : ICollectionFixture<AuthKeycloakRepositoryTestFixture>
    { }

    public class AuthKeycloakRepositoryTestFixture : BaseFixture
    {
        public AuthKeycloakRepositoryTestFixture()
            : base()
        { }

        public AppSettings GetAppSettings()
        {
            var connectionsString = new ConnectionStringsSettings
            {
                MasterConnection = ""
            };

            var keycloak = new KeycloakSettings 
            { 
                AuthServerUrl = "http://localhost:1234",
                ConfidentialPort = 0,
                Credentials = new KeycloakCredentials { Secret = "123", AdminUsername = "admin", AdminPassword = "admin" },
                Realm = "odin-realm",
                Resource = "odin-client",
                RolesSource = "Realm",
                SslRequired = "none",
                VerifyTokenAudience = true
            };

            return new AppSettings
            {
                ConnectionStringsSettings = connectionsString,
                Keycloak = keycloak
            };
        }
    }
}
