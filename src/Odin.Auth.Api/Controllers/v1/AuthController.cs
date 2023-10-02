using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> SignInAsync([FromBody] LoginInput input, CancellationToken cancellationToken)
        {
            var output = await _mediator.Send(input, cancellationToken);
            return Ok(output);

        }

        [HttpPost("sign-out")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SignOutAsync([FromBody] LogoutInput input, CancellationToken cancellationToken)
        {
            await _mediator.Send(input, cancellationToken);
            return NoContent();
        }

        [HttpPost("change-password")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ChangePasswordInput input, CancellationToken cancellationToken)
        {
            await _mediator.Send(input, cancellationToken);
            return NoContent();
        }
    }
}
