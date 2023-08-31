using FluentValidation;

namespace Odin.Auth.Application.AddUser
{
    public class AddUserInputValidator : AbstractValidator<AddUserInput>
    {
        public AddUserInputValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.EmailAddress).NotEmpty();
            RuleFor(x => x.EmailAddress).EmailAddress();
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
