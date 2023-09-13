using MediatR;

namespace Odin.Auth.Application.ResetPassword
{
    public class ResetPasswordInput : IRequest<ResetPasswordOutput>
    {        
        public string UserId { get; private set; }
        public string NewPassword { get; private set; }
        public string ConfirmationCode { get; private set; }
        public string Username { get; private set; }

        public ResetPasswordInput(string userId, string newPassword, string confirmationCode, string username)
        {
            UserId = userId;
            NewPassword = newPassword;
            ConfirmationCode = confirmationCode;
            Username = username;
        }
    }
}
