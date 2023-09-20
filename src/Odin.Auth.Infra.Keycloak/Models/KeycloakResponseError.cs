namespace Odin.Auth.Infra.Keycloak.Models
{
    public class KeycloakResponseError
    {
        public string Error { get; private set; }
        public string ErrorDescription { get; private set; }

        public KeycloakResponseError(string error, string errorDescription)
        {
            Error = error;
            ErrorDescription = errorDescription;
        }
    }
}
