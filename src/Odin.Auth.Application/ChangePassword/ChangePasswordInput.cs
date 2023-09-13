using MediatR;

namespace Odin.Auth.Application.ChangePassword
{
    public class ChangePasswordInput : IRequest<ChangePasswordOutput>
    {  
        public string Username { get; private set; }
        public string CurrentPassword { get; private set; }
        public string NewPassword { get; private set; }

        public ChangePasswordInput(string username, string currentPassword, string newPassword)
        {
            Username = username;
            CurrentPassword = currentPassword;
            NewPassword = newPassword;
        }
    }
}
