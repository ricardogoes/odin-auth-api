using Odin.Auth.Domain.Models.AppSettings;

namespace Odin.Auth.UnitTests.Keycloak.Repositories
{
    public class KeycloakRepositoryBaseFixture : BaseFixture
    {
        public AppSettings GetAppSettings()
        {
            var connectionsString = new ConnectionStringsSettings("");

            var keycloak = new KeycloakSettings
            {
                AuthServerUrl = "http://localhost:1234",
                ConfidentialPort = 0,
                Credentials = new KeycloakCredentials { Secret = "123" },
                Realm = "odin-realm",
                Resource = "odin-client",
                RolesSource = "Realm",
                SslRequired = "none",
                VerifyTokenAudience = true
            };

            return new AppSettings(connectionsString, keycloak);
        }
    }
}
