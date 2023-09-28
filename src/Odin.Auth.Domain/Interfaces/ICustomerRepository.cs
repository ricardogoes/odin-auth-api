using Odin.Auth.Domain.Entities;
using Odin.Auth.Domain.Models;

namespace Odin.Auth.Domain.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer> InsertAsync(Customer entity, CancellationToken cancellationToken);

        Task<Customer> UpdateAsync(Customer entity, CancellationToken cancellationToken);

        Task DeleteAsync(Customer entity);

        Task<Customer> FindByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<PaginatedListOutput<Customer>> FindPaginatedListAsync(Dictionary<string, object?> filters, int pageNumber, int pageSize, string sort, CancellationToken cancellationToken);

        Task<Customer> FindByDocumentAsync(string document, CancellationToken cancellationToken);
    }
}
