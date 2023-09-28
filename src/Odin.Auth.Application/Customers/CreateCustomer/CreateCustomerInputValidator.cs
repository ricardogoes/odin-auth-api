using FluentValidation;
using Odin.Auth.Domain.Validations;

namespace Odin.Auth.Application.Customers.CreateCustomer
{
    public class CreateCustomerInputValidator
        : AbstractValidator<CreateCustomerInput>
    {
        public CreateCustomerInputValidator()
        {
            RuleFor(x => x.Name).NotEmpty();

            RuleFor(x => x.Document)
                .Must(CpfCnpjValidation.IsCpfCnpj)
                .WithMessage("'Document' must be a valid CPF or CNPJ");

            RuleFor(x => x.LoggedUsername).NotEmpty();
        }
    }
}
