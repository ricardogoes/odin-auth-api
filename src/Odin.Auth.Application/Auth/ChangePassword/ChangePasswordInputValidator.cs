using FluentValidation;

namespace Odin.Auth.Application.Auth.ChangePassword
{
    public class ChangePasswordInputValidator : AbstractValidator<ChangePasswordInput>
    {
        public ChangePasswordInputValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.NewPassword).NotEmpty();
        }
    }
}
