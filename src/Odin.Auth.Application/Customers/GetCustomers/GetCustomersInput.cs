using MediatR;
using Odin.Auth.Domain.Models;

namespace Odin.Auth.Application.Customers.GetCustomers
{
    public class GetCustomersInput : PaginatedListInput, IRequest<PaginatedListOutput<CustomerOutput>>
    {
        public string? Name { get; private set; }
        public string? Document { get; private set; }
        public bool? IsActive { get; private set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAtStart { get; set; }
        public DateTime? CreatedAtEnd { get; set; }
        public string? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedAtStart { get; set; }
        public DateTime? LastUpdatedAtEnd { get; set; }

        public GetCustomersInput()
        { }

        public GetCustomersInput(int page, int pageSize, string? sort = null, string? name = null, string? document = null, bool? isActive = null,
            string? createdBy = null, DateTime? createdAtStart = null, DateTime? createdAtEnd = null,
            string? lastUpdatedBy = null, DateTime? lastUpdatedAtStart = null, DateTime? lastUpdatedAtEnd = null)
            : base(page, pageSize, sort)
        {
            Name = name;
            Document = document;
            IsActive = isActive;

            CreatedBy = createdBy;
            CreatedAtStart = createdAtStart;
            CreatedAtEnd = createdAtEnd;

            LastUpdatedBy = lastUpdatedBy;
            LastUpdatedAtStart = lastUpdatedAtStart;
            LastUpdatedAtEnd = lastUpdatedAtEnd;
        }
    }
}
