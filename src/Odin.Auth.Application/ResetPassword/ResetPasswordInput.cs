using MediatR;

namespace Odin.Auth.Application.ResetPassword
{
    public class ResetPasswordInput : IRequest<ResetPasswordOutput>
    {
        public string UserId { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmationCode { get; set; }
        public string Username { get; set; }
    }
}
