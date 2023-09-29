using FluentValidation;
using Odin.Auth.Application.Validators;

namespace Odin.Auth.Application.Users.GetUserById
{
    public class GetUserByIdInputValidator : AbstractValidator<GetUserByIdInput>
    {
        public GetUserByIdInputValidator()
        {
            Include(new TenantValidator());
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
