using FluentValidation;

namespace Odin.Auth.Application.ChangeStatusUser
{
    public class ChangeStatusUserInputValidator : AbstractValidator<ChangeStatusUserInput>
    {
        public ChangeStatusUserInputValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Action).NotEmpty();
        }
    }
}
