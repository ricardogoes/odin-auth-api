using Odin.Auth.Domain.Entities;

namespace Odin.Auth.Domain.Interfaces
{
    public interface IDocumentService
    {
        Task<bool> IsDocumentUnique(Customer customer, CancellationToken cancellationToken);
    }
}
