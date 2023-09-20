using FluentValidation;

namespace Odin.Auth.Application.UpdateProfile
{
    public class UpdateProfileInputValidator : AbstractValidator<UpdateProfileInput>
    {
        public UpdateProfileInputValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Groups.FirstOrDefault()).NotNull();
        }
    }
}
