using FluentValidation;

namespace Odin.Auth.Application.Users.GetUserById
{
    public class GetUserByIdInputValidator : AbstractValidator<GetUserByIdInput>
    {
        public GetUserByIdInputValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
