using FluentValidation;

namespace Odin.Auth.Application.CreateUser
{
    public class CreateUserInputValidator : AbstractValidator<CreateUserInput>
    {
        public CreateUserInputValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Groups).NotEmpty();
        }
    }
}
