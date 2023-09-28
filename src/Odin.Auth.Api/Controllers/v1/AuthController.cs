using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Odin.Auth.Api.Filters;
using Odin.Auth.Api.Models.Auth;
using Odin.Auth.Application.Auth.ChangePassword;
using Odin.Auth.Application.Auth.Login;
using Odin.Auth.Application.Auth.Logout;

namespace Odin.Auth.Api.Controllers.v1
{
    [ApiController]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("sign-in")]
        [ProducesResponseType(typeof(LoginOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SignInAsync([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            var input = new LoginInput(request.Username, request.Password);
            var output = await _mediator.Send(input, cancellationToken);
            return Ok(output);

        }

        [HttpPost("sign-out")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SignOutAsync([FromBody] LogoutRequest request, CancellationToken cancellationToken)
        {
            var input = new LogoutInput(request.UserId);
            await _mediator.Send(input, cancellationToken);
            return NoContent();
        }

        [HttpPost("change-password")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPasswordAsync([FromHeader(Name = "X-TENANT-ID")] Guid tenantId, [FromBody] ChangePasswordRequest request, CancellationToken cancellationToken)
        {
            var input = new ChangePasswordInput(tenantId, request.UserId, request.NewPassword, request.Temporary);
            await _mediator.Send(input, cancellationToken);
            return NoContent();
        }
    }
}
