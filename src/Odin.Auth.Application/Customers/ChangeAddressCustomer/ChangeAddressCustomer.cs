using FluentValidation;
using MediatR;
using Odin.Auth.Domain.Exceptions;
using Odin.Auth.Domain.Interfaces;
using Odin.Auth.Domain.ValueObjects;
using Odin.Auth.Infra.Data.EF.Interfaces;

namespace Odin.Auth.Application.Customers.ChangeAddressCustomer
{
    public class ChangeAddressCustomer : IRequestHandler<ChangeAddressCustomerInput, CustomerOutput>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomerRepository _repository;
        private readonly IValidator<ChangeAddressCustomerInput> _validator;

        public ChangeAddressCustomer(IUnitOfWork unitOfWork, ICustomerRepository repository, IValidator<ChangeAddressCustomerInput> validator)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _validator = validator;
        }

        public async Task<CustomerOutput> Handle(ChangeAddressCustomerInput input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new EntityValidationException($"One or more validation errors occurred on type {nameof(input)}.", validationResult.ToDictionary());
            }

            var customer = await _repository.FindByIdAsync(input.CustomerId, cancellationToken);

            var address = new Address(input.StreetName, input.StreetNumber, input.Complement, input.Neighborhood, input.ZipCode, input.City, input.State);
            customer.ChangeAddress(address);

            await _repository.UpdateAsync(customer, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return CustomerOutput.FromCustomer(customer);
        }
    }
}
