namespace Odin.Auth.Api.Models.Auth
{
    public class LogoutRequest
    {        
        public Guid UserId { get; private set; }

        public LogoutRequest(Guid userId)
        {
            UserId = userId;
        }
    }
}
