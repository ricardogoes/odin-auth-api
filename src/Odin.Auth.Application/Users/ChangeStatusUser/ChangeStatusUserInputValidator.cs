﻿using FluentValidation;

namespace Odin.Auth.Application.Users.ChangeStatusUser
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
