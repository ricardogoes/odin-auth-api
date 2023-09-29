namespace Odin.Auth.Api.Models.Auth
{
    public class ChangePasswordRequest
    {
        public Guid UserId { get; private set; }
        public string NewPassword { get; private set; }
        public bool Temporary { get; private set; }

        public ChangePasswordRequest(Guid userId, string newPassword, bool temporary)
        {
            UserId = userId;
            NewPassword = newPassword;
            Temporary = temporary;
        }
    }
}
