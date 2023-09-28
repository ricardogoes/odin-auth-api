using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Odin.Auth.Api.Helpers;
using Odin.Auth.Api.Models;
using Odin.Auth.Api.Models.Users;
using Odin.Auth.Application.Users;
using Odin.Auth.Application.Users.ChangeStatusUser;
using Odin.Auth.Application.Users.CreateUser;
using Odin.Auth.Application.Users.GetUserById;
using Odin.Auth.Application.Users.GetUsers;
using Odin.Auth.Application.Users.UpdateProfile;
using Odin.Auth.Domain.Enums;
using Odin.Auth.Domain.Exceptions;

namespace Odin.Auth.Api.Controllers.v1
{
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/users")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        [ProducesResponseType(typeof(UserOutput), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            CancellationToken cancellationToken,
            [FromHeader(Name = "X-TENANT-ID")] Guid tenantId,
            [FromQuery(Name = "page_number")] int? pageNumber = null,
            [FromQuery(Name = "page_size")] int? pageSize = null,
            [FromQuery(Name = "sort")] string? sort = null,
            [FromQuery(Name = "username")] string? username = null,
            [FromQuery(Name = "first_name")] string? firstName = null,
            [FromQuery(Name = "last_name")] string? lastName = null,
            [FromQuery(Name = "email")] string? email = null,
            [FromQuery(Name = "is_active")] bool? isActive = null)
        {
            var input = new GetUsersInput
            (
                tenantId: tenantId,
                pageNumber: pageNumber ?? 1,
                pageSize: pageSize ?? 5,
                sort: Utils.GetSortParam(sort),
                username: username,
                firstName: firstName,
                lastName: lastName,
                email: email,
                isActive: isActive
            );

            var paginatedUsers = await _mediator.Send(input, cancellationToken);

            return Ok(new PaginatedApiResponse<UserOutput>(paginatedUsers));
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(UserOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromHeader(Name = "X-TENANT-ID")] Guid tenantId, [FromRoute] Guid id, 
            CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                throw new BadRequestException("Invalid request");

            var user = await _mediator.Send(new GetUserByIdInput(tenantId, id), cancellationToken);

            return Ok(user);
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> InsertUserAsync([FromHeader(Name = "X-TENANT-ID")] Guid tenantId, [FromBody] CreateUserApiRequest request, 
            CancellationToken cancellationToken)
        {
            var loggedUsername = User.Identity!.Name!;

            var input = new CreateUserInput(tenantId, request.Username, request.Password, request.PasswordIsTemporary, request.FirstName,
                request.LastName, request.Email, request.Groups, loggedUsername);

            var output = await _mediator.Send(input, cancellationToken);
            return Ok(output);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(UserOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UpdateUserAsync([FromHeader(Name = "X-TENANT-ID")] Guid tenantId, [FromRoute] Guid id, [FromBody] UpdateProfileApiRequest request, 
            CancellationToken cancellationToken)
        {
            if (id == Guid.Empty || id != request.UserId)
                throw new BadRequestException("Invalid request");

            var loggedUsername = User.Identity!.Name!;
            var input = new UpdateProfileInput(tenantId, id, request.FirstName, request.LastName, request.Email, request.Groups, loggedUsername);

            var output = await _mediator.Send(input, cancellationToken);
            return Ok(output);
        }

        [HttpPut("{id:guid}/status")]
        [ProducesResponseType(typeof(UserOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> ChangeStatusUserAsync([FromHeader(Name = "X-TENANT-ID")] Guid tenantId, [FromRoute] Guid id, [FromQuery] string action, 
            CancellationToken cancellationToken)
        {
            if (id == Guid.Empty || string.IsNullOrWhiteSpace(action))
                throw new BadRequestException("Invalid request");

            if (action.ToUpper() != "ACTIVATE" && action.ToUpper() != "DEACTIVATE")
                throw new BadRequestException("Invalid action. Only ACTIVATE or DEACTIVATE values are allowed");

            var loggedUsername = User.Identity!.Name!;
            var userUpdated = await _mediator.Send(new ChangeStatusUserInput
            (
                tenantId, 
                id,
                (ChangeStatusAction)Enum.Parse(typeof(ChangeStatusAction), action, true),
                loggedUsername
            ), cancellationToken);

            return Ok(userUpdated);
        }        
    }
}
