using FluentValidation;

namespace Odin.Auth.Application.GetUserById
{
    public class GetUserByIdInputValidator : AbstractValidator<GetUserByIdInput>
    {
        public GetUserByIdInputValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
