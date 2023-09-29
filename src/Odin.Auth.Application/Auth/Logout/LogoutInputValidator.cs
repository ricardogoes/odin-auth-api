using FluentValidation;

namespace Odin.Auth.Application.Auth.Logout
{
    public class LogoutInputValidator : AbstractValidator<LogoutInput>
    {
        public LogoutInputValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
