using FluentValidation;
using Odin.Auth.Domain.SeedWork;

namespace Odin.Auth.Application.Validators
{
    public class TenantValidator : AbstractValidator<Tenant>
    {
        public TenantValidator()
        {
            RuleFor(x => x.TenantId).NotEmpty();
        }
    }
}
