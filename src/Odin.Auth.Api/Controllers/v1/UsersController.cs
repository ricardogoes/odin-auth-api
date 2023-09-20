using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Odin.Auth.Api.Models;
using Odin.Auth.Application.ChangeStatusUser;
using Odin.Auth.Application.CreateUser;
using Odin.Auth.Application.GetUserById;
using Odin.Auth.Application.GetUsers;
using Odin.Auth.Application.UpdateProfile;
using Odin.Auth.Domain.Exceptions;
using Odin.Auth.Domain.Models;
using Odin.Auth.Infra.Messaging.Extensions;

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
                pageNumber: pageNumber ?? 1,
                pageSize: pageSize ?? 5,
                sort: GetSortParam(sort),
                username: username,
                firstName: firstName,
                lastName: lastName,
                email: email,
                enabled: isActive
            );

            var paginatedUsers = await _mediator.Send(input, cancellationToken);

            return Ok(new PaginatedApiResponse<UserOutput>(paginatedUsers));
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(UserOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                throw new BadRequestException("Invalid request");

            var user = await _mediator.Send(new GetUserByIdInput(id), cancellationToken);

            return Ok(user);
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> InsertUserAsync([FromBody] CreateUserApiRequest request, CancellationToken cancellationToken)
        {
            var loggedUsername = User.Identity!.Name!;

            var input = new CreateUserInput(request.Username, request.Password, request.PasswordIsTemporary, request.FirstName,
                request.LastName, request.Email, request.Groups, loggedUsername);

            var output = await _mediator.Send(input, cancellationToken);
            return Ok(output);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(UserOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UpdateUserAsync([FromRoute] Guid id, [FromBody] UpdateProfileApiRequest request, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty || id != request.UserId)
                throw new BadRequestException("Invalid request");

            var loggedUsername = User.Identity!.Name!;
            var input = new UpdateProfileInput(id, request.FirstName, request.LastName, request.Email, request.Groups, loggedUsername);

            var output = await _mediator.Send(input, cancellationToken);
            return Ok(output);
        }

        [HttpPut("{id:guid}/status")]
        [ProducesResponseType(typeof(UserOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> ChangeStatusUserAsync([FromRoute] Guid id, [FromQuery] string action, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty || string.IsNullOrWhiteSpace(action))
                throw new BadRequestException("Invalid request");

            if (action.ToUpper() != "ACTIVATE" && action.ToUpper() != "DEACTIVATE")
                throw new BadRequestException("Invalid action. Only ACTIVATE or DEACTIVATE values are allowed");

            var loggedUsername = User.Identity!.Name!;
            var userUpdated = await _mediator.Send(new ChangeStatusUserInput
            (
                id,
                (ChangeStatusAction)Enum.Parse(typeof(ChangeStatusAction), action, true),
                loggedUsername
            ), cancellationToken);

            return Ok(userUpdated);
        }

        private static string? GetSortParam(string? sort)
        {
            if (string.IsNullOrWhiteSpace(sort))
                return null;

            if (sort.Contains(' '))
            {
                var splittedSort = sort.Split(' ');
                return $"{splittedSort[0].ToPascalCase()} {splittedSort[1]}";
            }
            else
                return sort;
        }
    }
}
