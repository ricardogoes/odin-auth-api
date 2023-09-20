using FluentValidation;
using MediatR;
using Odin.Auth.Domain.Exceptions;
using Odin.Auth.Domain.Interfaces;

namespace Odin.Auth.Application.Login
{
    public class Login : IRequestHandler<LoginInput, LoginOutput>
    {
        private readonly IValidator<LoginInput> _validator;
        private readonly IKeycloakRepository _keycloakRepository;

        public Login(IValidator<LoginInput> validator, IKeycloakRepository keycloakRepository)
        {
            _validator = validator;
            _keycloakRepository = keycloakRepository;
        }

        public async Task<LoginOutput> Handle(LoginInput input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new EntityValidationException($"One or more validation errors occurred on type {nameof(input)}.", validationResult.ToDictionary());
            }

            var authResponse = await _keycloakRepository.AuthAsync(input.Username, input.Password, cancellationToken);
            return new LoginOutput(authResponse.IdToken, authResponse.AccessToken, authResponse.RefreshToken, authResponse.ExpiresIn);
        }
    }
}
