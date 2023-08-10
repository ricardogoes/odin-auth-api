using Odin.Auth.Domain.Models;
using Odin.Auth.Domain.Models.ResetPassword;
using Odin.Auth.Domain.Models.UserLogin;

namespace Odin.Auth.Domain.Interfaces.Services
{
    public interface ICognitoAuthService
    {
        Task<Domain.Models.ChangePassword.ChangePasswordResponse> TryChangePasswordAsync(Domain.Models.ChangePassword.ChangePasswordRequest request);
        Task<AuthResponse> TryLoginAsync(UserAuthRequest request);
        Task<bool> TryLogOutAsync(UserSignOutRequest model);
        Task<Domain.Models.ForgotPassword.ForgotPasswordResponse> TryInitForgotPasswordAsync(Domain.Models.ForgotPassword.ForgotPasswordRequest request);
        Task<bool> TryResetPasswordWithConfirmationCodeAsync(ResetPasswordRequest request);

    }
}
