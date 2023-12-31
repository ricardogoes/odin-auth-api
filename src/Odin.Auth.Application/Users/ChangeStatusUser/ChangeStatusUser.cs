﻿using FluentValidation;
using MediatR;
using Odin.Auth.Domain.Enums;
using Odin.Auth.Domain.Exceptions;
using Odin.Auth.Infra.Keycloak.Interfaces;

namespace Odin.Auth.Application.Users.ChangeStatusUser
{
    public class ChangeStatusUser : IRequestHandler<ChangeStatusUserInput, UserOutput>
    {
        private readonly IValidator<ChangeStatusUserInput> _validator;
        private readonly IUserKeycloakRepository _keycloakRepository;

        public ChangeStatusUser(IValidator<ChangeStatusUserInput> validator, IUserKeycloakRepository keycloakRepository)
        {
            _validator = validator;
            _keycloakRepository = keycloakRepository;
        }

        public async Task<UserOutput> Handle(ChangeStatusUserInput input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new EntityValidationException($"One or more validation errors occurred on type {nameof(input)}.", validationResult.ToDictionary());
            }

            var user = await _keycloakRepository.FindByIdAsync(input.UserId, cancellationToken);

            switch (input.Action)
            {
                case ChangeStatusAction.ACTIVATE:
                    user.Activate();
                    break;
                case ChangeStatusAction.DEACTIVATE:
                    user.Deactivate();
                    break;
            }

            var userUpdated = await _keycloakRepository.UpdateUserAsync(user, cancellationToken);

            return UserOutput.FromUser(userUpdated);
        }
    }
}
