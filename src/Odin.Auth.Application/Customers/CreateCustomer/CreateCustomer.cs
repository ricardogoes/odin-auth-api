using FluentValidation;
using MediatR;
using Odin.Auth.Domain.Entities;
using Odin.Auth.Domain.Exceptions;
using Odin.Auth.Domain.Interfaces;
using Odin.Auth.Infra.Data.EF.Interfaces;

namespace Odin.Auth.Application.Customers.CreateCustomer
{
    public class CreateCustomer : IRequestHandler<CreateCustomerInput, CustomerOutput>
    {
        private readonly IDocumentService _documentService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomerRepository _customerRepository;
        private readonly IValidator<CreateCustomerInput> _validator;

        public CreateCustomer(IDocumentService documentService, IUnitOfWork unitOfWork, ICustomerRepository customerRepository,
            IValidator<CreateCustomerInput> validator)
        {
            _documentService = documentService;
            _unitOfWork = unitOfWork;
            _customerRepository = customerRepository;
            _validator = validator;
        }

        public async Task<CustomerOutput> Handle(CreateCustomerInput input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new EntityValidationException($"One or more validation errors occurred on type {nameof(input)}.", validationResult.ToDictionary());
            }

            var customer = new Customer(input.Name, input.Document, isActive: true);
            
            var isDocumentUnique = await _documentService.IsDocumentUnique(customer, cancellationToken);
            if (!isDocumentUnique)
                throw new EntityValidationException("Document must be unique");


            if(input.Address is not null)
            {
                customer.ChangeAddress(input.Address, input.LoggedUsername);
            }

            customer.Create(input.LoggedUsername);

            await _customerRepository.InsertAsync(customer, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            return CustomerOutput.FromCustomer(customer);
        }
    }
}
