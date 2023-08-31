using FluentValidation;

namespace Odin.Auth.Application.ChangePassword
{
    public class ChangePasswordInputValidator : AbstractValidator<ChangePasswordInput>
    {
        public ChangePasswordInputValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.CurrentPassword).NotEmpty();
            RuleFor(x => x.NewPassword).NotEmpty();
        }
    }
}
