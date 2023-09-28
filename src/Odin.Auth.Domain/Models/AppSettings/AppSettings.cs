namespace Odin.Auth.Domain.Models.AppSettings
{
    public class AppSettings
    {
        public ConnectionStringsSettings? ConnectionStringsSettings { get; set; }
        public KeycloakSettings? Keycloak { get; set; }

    }
}
