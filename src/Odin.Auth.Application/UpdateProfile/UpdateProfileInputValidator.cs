using FluentValidation;

namespace Odin.Auth.Application.UpdateProfile
{
    public class UpdateProfileInputValidator : AbstractValidator<UpdateProfileInput>
    {
        public UpdateProfileInputValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();

            RuleFor(x => x.EmailAddress).NotEmpty();
            RuleFor(x => x.EmailAddress).EmailAddress();
        }
    }
}
