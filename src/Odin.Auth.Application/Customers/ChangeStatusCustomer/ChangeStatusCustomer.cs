using FluentValidation;
using MediatR;
using Odin.Auth.Domain.Enums;
using Odin.Auth.Domain.Exceptions;
using Odin.Auth.Domain.Interfaces;
using Odin.Auth.Infra.Data.EF.Interfaces;

namespace Odin.Auth.Application.Customers.ChangeStatusCustomer
{
    public class ChangeStatusCustomer : IRequestHandler<ChangeStatusCustomerInput, CustomerOutput>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomerRepository _repository;
        private readonly IValidator<ChangeStatusCustomerInput> _validator;

        public ChangeStatusCustomer(IUnitOfWork unitOfWork, ICustomerRepository repository, IValidator<ChangeStatusCustomerInput> validator)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _validator = validator;
        }

        public async Task<CustomerOutput> Handle(ChangeStatusCustomerInput input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new EntityValidationException($"One or more validation errors occurred on type {nameof(input)}.", validationResult.ToDictionary());
            }

            var customer = await _repository.FindByIdAsync(input.Id, cancellationToken);

            switch (input.Action)
            {
                case ChangeStatusAction.ACTIVATE:
                    customer.Activate(input.LoggedUsername);
                    break;
                case ChangeStatusAction.DEACTIVATE:
                    customer.Deactivate(input.LoggedUsername);
                    break;
            }

            await _repository.UpdateAsync(customer, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return CustomerOutput.FromCustomer(customer);
        }
    }
}
