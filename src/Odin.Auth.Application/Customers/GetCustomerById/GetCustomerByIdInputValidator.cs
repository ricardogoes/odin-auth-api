using FluentValidation;

namespace Odin.Auth.Application.Customers.GetCustomerById
{
    public class GetCustomerByIdInputValidator
        : AbstractValidator<GetCustomerByIdInput>
    {
        public GetCustomerByIdInputValidator()
            => RuleFor(x => x.Id).NotEmpty();
    }
}
