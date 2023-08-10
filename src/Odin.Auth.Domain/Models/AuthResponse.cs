namespace Odin.Auth.Domain.Models
{
    public class AuthResponse
    {
        public string Username { get; set; }
        public string Message { get; set; }
        public TokenResponse Tokens { get; set; }
    }

    public class TokenResponse
    {
        public string IdToken { get; set; }
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }
        public string RefreshToken { get; set; }
    }
}
