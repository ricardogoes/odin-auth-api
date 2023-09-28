using MediatR;
using Odin.Auth.Domain.Models;
using Odin.Auth.Domain.SeedWork;

namespace Odin.Auth.Application.Users.GetUsers
{
    public class GetUsersInput : Tenant, IRequest<PaginatedListOutput<UserOutput>>
    {
        public int PageNumber { get; private set; }
        public int PageSize { get; private set; }
        public string Sort { get; private set; }
        public string? Username { get; private set; }
        public string? FirstName { get; private set; }
        public string? LastName { get; private set; }
        public string? Email { get; private set; }
        public bool? IsActive { get; private set; }
        public string? CreatedBy { get; private set; }
        public DateTime? CreatedAtStart { get; private set; }
        public DateTime? CreatedAtEnd { get; private set; }
        public string? LastUpdatedBy { get; private set; }
        public DateTime? LastUpdatedAtStart { get; private set; }
        public DateTime? LastUpdatedAtEnd { get; private set; }

        public GetUsersInput(Guid tenantId, int pageNumber, int pageSize, string? sort = null, string? username = null, string? firstName = null, string? lastName = null,
            string? email = null, bool? isActive = null, string? createdBy = null, DateTime? createdAtStart = null, DateTime? createdAtEnd = null,
            string? lastUpdatedBy = null, DateTime? lastUpdatedAtStart = null, DateTime? lastUpdatedAtEnd = null)
            : base(tenantId)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            Sort = sort ?? "firstName desc";
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
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
