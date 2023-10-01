using MediatR;

namespace Odin.Auth.Application.Auth.ChangePassword
{
    public class ChangePasswordInput : IRequest
    {
        public Guid UserId { get; private set; }
        public string NewPassword { get; private set; }
        public bool Temporary { get; private set; }

        public ChangePasswordInput(Guid userId, string newPassword, bool temporary)
        {
            UserId = userId;
            NewPassword = newPassword;
            Temporary = temporary;
        }
    }
}
