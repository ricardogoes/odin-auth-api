using Amazon.CognitoIdentityProvider.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Odin.Auth.Api.Attributes;
using Odin.Auth.Domain.Interfaces.Services;
using Odin.Auth.Domain.Models;
using Odin.Auth.Domain.Models.ChangeStatusUser;
using Odin.Auth.Domain.Models.UpdateProfile;
using Odin.Auth.Domain.Models.UserLogin;

namespace Odin.Auth.Api.Controllers.v1
{
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/users")]
    public class UsersController : BaseController
    {
        private readonly ICognitoUserService _cognitoUserService;

        public UsersController(ICognitoUserService cognitoUserService,
            ILogger<AuthController> logger)
            : base(logger)
        {
            _cognitoUserService = cognitoUserService ?? throw new ArgumentNullException(nameof(cognitoUserService));
        }
        
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> InsertUserAsync([FromBody] InsertUserRequest request)
        {
            try
            {
                var response = await _cognitoUserService.InsertUserAsync(request);
                return Ok(new ApiResponse(ApiResponseState.Success, response));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPut]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateProfileRequest request)
        {
            try
            {
                var response = await _cognitoUserService.UpdateUserAttributesAsync(request);
                return Ok(new ApiResponse(ApiResponseState.Success, response));
            }
            catch (UserNotFoundException)
            {
                return NotFound(new ApiResponse(ApiResponseState.Failed, "User not found"));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPut("status")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> ChangeStatusUserAsync([FromBody] ChangeStatusUserRequest request)
        {
            try
            {
                var response = new UpdateProfileResponse();

                switch(request.Action)
                {
                    case "ENABLE":
                        response = await _cognitoUserService.EnableUserAsync(request.Username);
                        break;
                    case "DISABLE":
                        response = await _cognitoUserService.DisableUserAsync(request.Username);
                        break;
                }

                return Ok(new ApiResponse(ApiResponseState.Success, response));
            }
            catch (UserNotFoundException)
            {
                return NotFound(new ApiResponse(ApiResponseState.Failed, "User not found"));
            }   
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
    }
}
