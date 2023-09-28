using FluentValidation;
using MediatR;
using Odin.Auth.Domain.Exceptions;
using Odin.Auth.Infra.Keycloak.Interfaces;

namespace Odin.Auth.Application.Auth.Logout
{
    public class Logout : IRequestHandler<LogoutInput>
    {
        private readonly IValidator<LogoutInput> _validator;
        private readonly IAuthKeycloakRepository _keycloakRepository;

        public Logout(IValidator<LogoutInput> validator, IAuthKeycloakRepository keycloakRepository)
        {
            _validator = validator;
            _keycloakRepository = keycloakRepository;
        }

        public async Task Handle(LogoutInput input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new EntityValidationException($"One or more validation errors occurred on type {nameof(input)}.", validationResult.ToDictionary());
            }

            await _keycloakRepository.LogoutAsync(input.UserId, cancellationToken);
        }
    }
}
