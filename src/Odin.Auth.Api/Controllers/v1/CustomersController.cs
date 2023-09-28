using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Odin.Auth.Api.Helpers;
using Odin.Auth.Api.Models;
using Odin.Auth.Application.Customers;
using Odin.Auth.Application.Customers.ChangeAddressCustomer;
using Odin.Auth.Application.Customers.ChangeStatusCustomer;
using Odin.Auth.Application.Customers.CreateCustomer;
using Odin.Auth.Application.Customers.GetCustomerById;
using Odin.Auth.Application.Customers.GetCustomers;
using Odin.Auth.Application.Customers.UpdateCustomer;
using Odin.Auth.Domain.Enums;
using Odin.Auth.Domain.Exceptions;
using Odin.Baseline.Api.Models.Customers;

namespace Odin.Auth.Api.Controllers.v1
{
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/customers")]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(CustomerOutput), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            CancellationToken cancellationToken,
            [FromQuery(Name = "page_number")] int? PageNumber = null,
            [FromQuery(Name = "page_size")] int? PageSize = null,
            [FromQuery(Name = "sort")] string? Sort = null,
            [FromQuery(Name = "name")] string? Name = null,
            [FromQuery(Name = "document")] string? Document = null,
            [FromQuery(Name = "is_active")] bool? IsActive = null)
        {
            var input = new GetCustomersInput
            (
                page: PageNumber ?? 1,
                pageSize: PageSize ?? 10,
                sort: Utils.GetSortParam(Sort),
                name: !string.IsNullOrWhiteSpace(Name) ? Name : "",
                document: !string.IsNullOrWhiteSpace(Document) ? Document : "",
                isActive: IsActive
            );

            var paginatedCustomers = await _mediator.Send(input, cancellationToken);

            return Ok(new PaginatedApiResponse<CustomerOutput>(paginatedCustomers));
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(CustomerOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                throw new BadRequestException("Invalid request");

            var customer = await _mediator.Send(new GetCustomerByIdInput { Id = id }, cancellationToken);

            return Ok(customer);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CustomerOutput), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Create([FromBody] CreateCustomerApiRequest request, CancellationToken cancellationToken)
        {

            var loggedUsername = User.Identity!.Name!;
            var input = new CreateCustomerInput(request.Name, request.Document, request.Address, loggedUsername);

            var customerCreated = await _mediator.Send(input, cancellationToken);

            return CreatedAtAction(
                actionName: nameof(GetById),
                routeValues: new { id = customerCreated.Id },
                value: customerCreated);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(CustomerOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateCustomerApiRequest request, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty || id != request.Id)
                throw new BadRequestException("Invalid request");

            var loggedUsername = User.Identity!.Name!;
            var input = new UpdateCustomerInput(request.Id, request.Name, request.Document, loggedUsername);

            var customerUpdated = await _mediator.Send(input, cancellationToken);

            return Ok(customerUpdated);
        }

        [HttpPut("{id:guid}/status")]
        [ProducesResponseType(typeof(CustomerOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangeStatus([FromRoute] Guid id, [FromQuery] string action, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty || string.IsNullOrWhiteSpace(action))
                throw new BadRequestException("Invalid request");

            if (action.ToUpper() != "ACTIVATE" && action.ToUpper() != "DEACTIVATE")
                throw new BadRequestException("Invalid action. Only ACTIVATE or DEACTIVATE values are allowed");

            var loggedUsername = User.Identity!.Name!;

            var customerUpdated = await _mediator.Send(new ChangeStatusCustomerInput
            (
                id,
                (ChangeStatusAction)Enum.Parse(typeof(ChangeStatusAction), action, true),
                loggedUsername
            ), cancellationToken);

            return Ok(customerUpdated);
        }

        [HttpPut("{id:guid}/addresses")]
        [ProducesResponseType(typeof(CustomerOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangeAddress([FromRoute] Guid id, [FromBody] ChangeAddressCustomerApiRequest request, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty || id != request.CustomerId)
                throw new BadRequestException("Invalid request");

            var loggedUsername = User.Identity!.Name!;
            var input = new ChangeAddressCustomerInput(id, request.StreetName, request.StreetNumber, request.Neighborhood, 
                request.ZipCode, request.City, request.State, loggedUsername, request.Complement);

            var customerUpdated = await _mediator.Send(input, cancellationToken);

            return Ok(customerUpdated);
        }
    }
}
