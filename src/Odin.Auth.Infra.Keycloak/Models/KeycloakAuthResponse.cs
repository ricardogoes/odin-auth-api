namespace Odin.Auth.Infra.Keycloak.Models
{
    public class KeycloakAuthResponse
    {
        public string AccessToken { get; private set; }
        public string IdToken { get; private set; }
        public string RefreshToken { get; private set; }
        public int ExpiresIn { get; private set; }

        public KeycloakAuthResponse(string accessToken, string idToken, string refreshToken, int expiresIn)
        {
            AccessToken = accessToken;
            IdToken = idToken;
            RefreshToken = refreshToken;
            ExpiresIn = expiresIn;
        }
    }
}
