using FluentValidation;
using Odin.Auth.Application.Validators;

namespace Odin.Auth.Application.Users.ChangeStatusUser
{
    public class ChangeStatusUserInputValidator : AbstractValidator<ChangeStatusUserInput>
    {
        public ChangeStatusUserInputValidator()            
        {
            Include(new TenantValidator());
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Action).NotEmpty();
        }
    }
}
