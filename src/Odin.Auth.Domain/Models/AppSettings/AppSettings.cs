namespace Odin.Auth.Domain.Models.AppSettings
{
    public class AppSettings
    {        
        public ConnectionStringsSettings ConnectionStringsSettings { get; private set; }
        public KeycloakSettings KeycloakSettings { get; private set; }

        public AppSettings(ConnectionStringsSettings connectionStringsSettings, KeycloakSettings keycloakSettings)
        {
            ConnectionStringsSettings = connectionStringsSettings;
            KeycloakSettings = keycloakSettings;
        }


    }
}
