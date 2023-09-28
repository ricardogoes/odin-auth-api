using FluentValidation;
using Odin.Auth.Application.Validators;

namespace Odin.Auth.Application.Users.UpdateProfile
{
    public class UpdateProfileInputValidator : AbstractValidator<UpdateProfileInput>
    {
        public UpdateProfileInputValidator()
        {
            Include(new TenantValidator());
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Groups).NotEmpty();
        }
    }
}
