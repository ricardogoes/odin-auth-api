using MediatR;
using Odin.Auth.Domain.Interfaces;
using Odin.Auth.Domain.Models;

namespace Odin.Auth.Application.Customers.GetCustomers
{
    public class GetCustomers : IRequestHandler<GetCustomersInput, PaginatedListOutput<CustomerOutput>>
    {
        private readonly ICustomerRepository _repository;

        public GetCustomers(ICustomerRepository repository)
            => _repository = repository;

        public async Task<PaginatedListOutput<CustomerOutput>> Handle(GetCustomersInput input, CancellationToken cancellationToken)
        {
            var filters = new Dictionary<string, object?>
            {
                { "Name", input.Name },
                { "Document", input.Document },
                { "IsActive", input.IsActive },
                { "CreatedBy", input.CreatedBy },
                { "CreatedAtStart", input.CreatedAtStart },
                { "CreatedAtEnd", input.CreatedAtEnd },
                { "LastUpdatedBy", input.LastUpdatedBy },
                { "LastUpdatedAtStart", input.LastUpdatedAtStart },
                { "LastUpdatedAtEnd", input.LastUpdatedAtEnd }
            };

            var customers = await _repository.FindPaginatedListAsync(
                filters, input.PageNumber, input.PageSize, input.Sort!,
                cancellationToken: cancellationToken);

            return new PaginatedListOutput<CustomerOutput>
            (
                pageNumber: input.PageNumber,
                pageSize: input.PageSize,
                totalPages: PaginatedListOutput<CustomerOutput>.GetTotalPages(customers.TotalItems, input.PageSize),
                totalItems: customers.TotalItems,
                items: CustomerOutput.FromCustomer(customers.Items)
            );
        }
    }
}
