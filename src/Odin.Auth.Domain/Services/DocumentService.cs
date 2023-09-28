using Odin.Auth.Domain.Entities;
using Odin.Auth.Domain.Exceptions;
using Odin.Auth.Domain.Interfaces;

namespace Odin.Auth.Domain.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly ICustomerRepository _customerRepository;

        public DocumentService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<bool> IsDocumentUnique(Customer customer, CancellationToken cancellationToken)
        {
            try
            {
                var searchedCustomer = await _customerRepository.FindByDocumentAsync(customer.Document, cancellationToken);
                return searchedCustomer.Id == customer.Id;
            }
            catch (NotFoundException)
            {
                return true;
            }
        }
    }
}
