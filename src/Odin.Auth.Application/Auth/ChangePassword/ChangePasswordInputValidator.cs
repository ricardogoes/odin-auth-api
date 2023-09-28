using FluentValidation;
using Odin.Auth.Application.Validators;

namespace Odin.Auth.Application.Auth.ChangePassword
{
    public class ChangePasswordInputValidator : AbstractValidator<ChangePasswordInput>
    {
        public ChangePasswordInputValidator()
        {
            Include(new TenantValidator());
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.NewPassword).NotEmpty();
        }
    }
}
