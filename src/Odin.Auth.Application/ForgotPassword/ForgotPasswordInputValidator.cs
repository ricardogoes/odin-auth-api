using FluentValidation;

namespace Odin.Auth.Application.ForgotPassword
{
    public class ForgotPasswordInputValidator : AbstractValidator<ForgotPasswordInput>
    {
        public ForgotPasswordInputValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
        }
    }
}
