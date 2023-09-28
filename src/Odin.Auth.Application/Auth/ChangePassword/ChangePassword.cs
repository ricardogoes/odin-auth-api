using FluentValidation;
using MediatR;
using Odin.Auth.Domain.Exceptions;
using Odin.Auth.Domain.ValuesObjects;
using Odin.Auth.Infra.Keycloak.Interfaces;

namespace Odin.Auth.Application.Auth.ChangePassword
{
    public class ChangePassword : IRequestHandler<ChangePasswordInput>
    {
        private readonly IValidator<ChangePasswordInput> _validator;
        private readonly IAuthKeycloakRepository _authKeycloakRepository;
        private readonly IUserKeycloakRepository _userKeycloakRepository;

        public ChangePassword(IValidator<ChangePasswordInput> validator, IAuthKeycloakRepository authKeycloakRepository,
            IUserKeycloakRepository userKeycloakRepository)
        {
            _validator = validator;
            _authKeycloakRepository = authKeycloakRepository;
            _userKeycloakRepository = userKeycloakRepository;
        }

        public async Task Handle(ChangePasswordInput input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new EntityValidationException($"One or more validation errors occurred on type {nameof(input)}.", validationResult.ToDictionary());
            }

            var user = await _userKeycloakRepository.FindByIdAsync(input.TenantId, input.UserId, cancellationToken);
            user.AddCredentials(new UserCredential(input.NewPassword, temporary: input.Temporary));

            await _authKeycloakRepository.ChangePasswordAsync(user, cancellationToken);
        }
    }
}
