﻿using FluentValidation;
using MediatR;
using Odin.Auth.Domain.Exceptions;
using Odin.Auth.Domain.Interfaces;
using Odin.Auth.Domain.Models;
using Odin.Auth.Domain.ValuesObjects;

namespace Odin.Auth.Application.UpdateProfile
{
    public class UpdateProfile : IRequestHandler<UpdateProfileInput, UserOutput>
    {
        private readonly IValidator<UpdateProfileInput> _validator;
        private readonly IKeycloakRepository _keycloakRepository;

        public UpdateProfile(IValidator<UpdateProfileInput> validator, IKeycloakRepository keycloakRepository)
        {
            _validator = validator;
            _keycloakRepository = keycloakRepository;
        }

        public async Task<UserOutput> Handle(UpdateProfileInput input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new EntityValidationException($"One or more validation errors occurred on type {nameof(input)}.", validationResult.ToDictionary());
            }

            var user = await _keycloakRepository.FindByIdAsync(input.UserId, cancellationToken);
            user.Update(input.FirstName, input.LastName, input.Email);

            user.RemoveAllGroups();
            foreach (var group in input.Groups)
            {
                user.AddGroup(new UserGroup(group));
            }

            user.AddAttribute(new KeyValuePair<string, string>("last_updated_at", DateTime.Now.ToString("o")));
            user.AddAttribute(new KeyValuePair<string, string>("last_updated_by", input.LoggedUsername));

            await _keycloakRepository.UpdateUserAsync(user, cancellationToken);

            return UserOutput.FromUser(user);
        }
    }
}
