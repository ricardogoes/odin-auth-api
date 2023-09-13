namespace Odin.Auth.Application.Login
{
    public class LoginOutput
    {        
        public string Username { get; private set; }
        public string? Message { get; private set; }
        public TokenResponse? Tokens { get; private set; }

        public LoginOutput(string username, string message, TokenResponse tokens)
        {
            Username = username;
            Message = message;
            Tokens = tokens;
        }

        public LoginOutput(string username, string message)
        {
            Username = username;
            Message = message;
        }

        public LoginOutput(string username, TokenResponse tokens)
        {
            Username = username;
            Tokens = tokens;
        }
    }

    public class TokenResponse
    {
        public string IdToken { get; private set; }
        public string AccessToken { get; private set; }
        public int ExpiresIn { get; private set; }
        public string RefreshToken { get; private set; }

        public TokenResponse(string idToken, string accessToken, int expiresIn, string refreshToken)
        {
            IdToken = idToken;
            AccessToken = accessToken;
            ExpiresIn = expiresIn;
            RefreshToken = refreshToken;
        }
    }
}
