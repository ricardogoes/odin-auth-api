using FluentValidation;
using Odin.Auth.Domain.Validations;

namespace Odin.Auth.Application.Customers.UpdateCustomer
{
    public class UpdateCustomerInputValidator
        : AbstractValidator<UpdateCustomerInput>
    {
        public UpdateCustomerInputValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Document).Must(CpfCnpjValidation.IsCpfCnpj).WithMessage("'Document' must be a valid CPF or CNPJ");
        }
    }
}
