using FluentValidation;
using MediatR;
using Odin.Auth.Domain.Exceptions;
using Odin.Auth.Domain.Interfaces;
using Odin.Auth.Infra.Data.EF.Interfaces;

namespace Odin.Auth.Application.Customers.UpdateCustomer
{
    public class UpdateCustomer : IRequestHandler<UpdateCustomerInput, CustomerOutput>
    {
        private readonly IDocumentService _documentService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomerRepository _repository;
        private readonly IValidator<UpdateCustomerInput> _validator;

        public UpdateCustomer(IDocumentService documentService, IUnitOfWork unitOfWork, ICustomerRepository repository, IValidator<UpdateCustomerInput> validator)
        {
            _documentService = documentService;
            _unitOfWork = unitOfWork;
            _repository = repository;
            _validator = validator;
        }

        public async Task<CustomerOutput> Handle(UpdateCustomerInput input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new EntityValidationException($"One or more validation errors occurred on type {nameof(input)}.", validationResult.ToDictionary());
            }

            var customer = await _repository.FindByIdAsync(input.Id, cancellationToken);
            customer.Update(input.Name, input.Document);

            var isDocumentUnique = await _documentService.IsDocumentUnique(customer, cancellationToken);
            if (!isDocumentUnique)
                throw new EntityValidationException("Document must be unique");

            await _repository.UpdateAsync(customer, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return CustomerOutput.FromCustomer(customer);
        }
    }
}
