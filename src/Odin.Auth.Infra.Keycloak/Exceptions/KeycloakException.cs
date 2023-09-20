namespace Odin.Auth.Infra.Keycloak.Exceptions
{
    public class KeycloakException : Exception
    {
        public KeycloakException(string? message)
            : base(message)
        { }
    }
}
