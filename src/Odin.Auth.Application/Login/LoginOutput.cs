namespace Odin.Auth.Application.Login
{
    public class LoginOutput
    {
        public string IdToken { get; private set; }
        public string AccessToken { get; private set; }
        public string RefreshToken { get; private set; }
        public int ExpiresIn { get; private set; }

        public LoginOutput(string idToken, string accessToken, string refreshToken, int expiresIn)
        {
            IdToken = idToken;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            ExpiresIn = expiresIn;
        }
    }
}
