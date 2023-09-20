using FluentValidation;

namespace Odin.Auth.Application.Logout
{
    public class LogoutInputValidator : AbstractValidator<LogoutInput>
    {
        public LogoutInputValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
