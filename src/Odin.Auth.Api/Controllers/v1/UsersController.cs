using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Odin.Auth.Application.AddUser;
using Odin.Auth.Application.ChangeStatusUser;
using Odin.Auth.Application.UpdateProfile;

namespace Odin.Auth.Api.Controllers.v1
{
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/users")]
    public class UsersController : BaseController
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator,
            ILogger<AuthController> logger)
            : base(logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        [ProducesResponseType(typeof(AddUserOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> InsertUserAsync([FromBody] AddUserInput request, CancellationToken cancellationToken)
        {
            var output = await _mediator.Send(request, cancellationToken);
            return Ok(output);
        }

        [HttpPut]
        [ProducesResponseType(typeof(UpdateProfileOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateProfileInput request, CancellationToken cancellationToken)
        {
            var output = await _mediator.Send(request, cancellationToken);
            return Ok(output);
        }

        [HttpPut("status")]
        [ProducesResponseType(typeof(ChangeStatusUserOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> ChangeStatusUserAsync([FromBody] ChangeStatusUserInput request, CancellationToken cancellationToken)
        {
            var output = await _mediator.Send(request, cancellationToken);
            return Ok(output);
        }
    }
}
