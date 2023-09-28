using FluentValidation;
using MediatR;
using Odin.Auth.Domain.Exceptions;
using Odin.Auth.Domain.Interfaces;

namespace Odin.Auth.Application.Customers.GetCustomerById
{
    public class GetCustomerById : IRequestHandler<GetCustomerByIdInput, CustomerOutput>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IValidator<GetCustomerByIdInput> _validator;

        public GetCustomerById(ICustomerRepository customerRepository, IValidator<GetCustomerByIdInput> validator)
        {
            _customerRepository = customerRepository;
            _validator = validator;
        }

        public async Task<CustomerOutput> Handle(GetCustomerByIdInput input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new EntityValidationException($"One or more validation errors occurred on type {nameof(input)}.", validationResult.ToDictionary());
            }

            var customer = await _customerRepository.FindByIdAsync(input.Id, cancellationToken);
            return CustomerOutput.FromCustomer(customer);
        }
    }
}
