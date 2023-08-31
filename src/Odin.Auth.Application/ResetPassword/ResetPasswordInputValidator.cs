using FluentValidation;

namespace Odin.Auth.Application.ResetPassword
{
    public class ResetPasswordInputValidator : AbstractValidator<ResetPasswordInput>
    {
        public ResetPasswordInputValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.NewPassword).NotEmpty();
            RuleFor(x => x.ConfirmationCode).NotEmpty();
        }
    }
}
