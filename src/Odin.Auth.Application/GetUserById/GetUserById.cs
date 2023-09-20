﻿using FluentValidation;
using MediatR;
using Odin.Auth.Domain.Exceptions;
using Odin.Auth.Domain.Interfaces;
using Odin.Auth.Domain.Models;

namespace Odin.Auth.Application.GetUserById
{
    public class GetUserById : IRequestHandler<GetUserByIdInput, UserOutput>
    {
        private readonly IValidator<GetUserByIdInput> _validator;
        private readonly IKeycloakRepository _keycloakRepository;

        public GetUserById(IValidator<GetUserByIdInput> validator, IKeycloakRepository keycloakRepository)
        {
            _validator = validator;
            _keycloakRepository = keycloakRepository;
        }

        public async Task<UserOutput> Handle(GetUserByIdInput input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new EntityValidationException($"One or more validation errors occurred on type {nameof(input)}.", validationResult.ToDictionary());
            }

            var user = await _keycloakRepository.FindByIdAsync(input.UserId, cancellationToken);
            
            return UserOutput.FromUser(user);
        }
    }
}
