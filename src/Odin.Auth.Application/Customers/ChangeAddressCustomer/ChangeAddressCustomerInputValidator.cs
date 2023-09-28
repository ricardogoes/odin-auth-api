using FluentValidation;

namespace Odin.Auth.Application.Customers.ChangeAddressCustomer
{
    public class ChangeAddressCustomerInputValidator : AbstractValidator<ChangeAddressCustomerInput>
    {
        public ChangeAddressCustomerInputValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty();
            RuleFor(x => x.StreetName).NotEmpty();
            RuleFor(x => x.StreetNumber).GreaterThan(0);
            RuleFor(x => x.Neighborhood).NotEmpty();
            RuleFor(x => x.ZipCode).NotEmpty();
            RuleFor(x => x.City).NotEmpty();
            RuleFor(x => x.State).NotEmpty();
        }
    }
}
