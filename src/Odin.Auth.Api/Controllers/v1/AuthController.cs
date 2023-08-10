using Microsoft.AspNetCore.Mvc;
using Odin.Auth.Api.Attributes;
using Odin.Auth.Domain.Interfaces.Services;
using Odin.Auth.Domain.Models;
using Odin.Auth.Domain.Models.UserLogin;

namespace Odin.Auth.Api.Controllers.v1
{
    [ApiController]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/auth")]
    public class AuthController : BaseController
    {
        private readonly ICognitoAuthService _cognitoAuthService;

        public AuthController(ICognitoAuthService cognitoAuthService, ILogger<AuthController> logger)
            : base(logger)
        {
            _cognitoAuthService = cognitoAuthService ?? throw new ArgumentNullException(nameof(cognitoAuthService));
        }

        [HttpPost("sign-in")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> SignInAsync([FromBody] UserAuthRequest request)
        {
            try
            {
                var loginResponse = await _cognitoAuthService.TryLoginAsync(request);
                return Ok(new ApiResponse(ApiResponseState.Success, loginResponse));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost("sign-out")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> SignOutAsync([FromBody] UserSignOutRequest request)
        {
            try
            {
                await _cognitoAuthService.TryLogOutAsync(request);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost("change-password")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] Domain.Models.ChangePassword.ChangePasswordRequest request)
        {
            try
            {
                var changePasswordResponse = await _cognitoAuthService.TryChangePasswordAsync(request);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost("forgot-password")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> ForgotPasswordAsync([FromBody] Domain.Models.ForgotPassword.ForgotPasswordRequest request)
        {
            try
            {
                var forgotPasswordResponse = await _cognitoAuthService.TryInitForgotPasswordAsync(request);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost("reset-password")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] Domain.Models.ResetPassword.ResetPasswordRequest request)
        {
            try
            {
                await _cognitoAuthService.TryResetPasswordWithConfirmationCodeAsync(request);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
    }
}
