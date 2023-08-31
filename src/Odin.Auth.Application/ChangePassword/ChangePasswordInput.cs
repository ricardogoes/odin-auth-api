using MediatR;

namespace Odin.Auth.Application.ChangePassword
{
    public class ChangePasswordInput : IRequest<ChangePasswordOutput>
    {
        public string Username { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
