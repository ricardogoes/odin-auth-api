using FluentValidation;
using MediatR;
using Odin.Auth.Domain.Exceptions;
using Odin.Auth.Domain.Interfaces;
using Odin.Auth.Domain.ValuesObjects;

namespace Odin.Auth.Application.ChangePassword
{
    public class ChangePassword : IRequestHandler<ChangePasswordInput>
    {
        private readonly IValidator<ChangePasswordInput> _validator;
        private readonly IKeycloakRepository _keycloakRepository;

        public ChangePassword(IValidator<ChangePasswordInput> validator, IKeycloakRepository keycloakRepository)
        {
            _validator = validator;
            _keycloakRepository = keycloakRepository;
        }

        public async Task Handle(ChangePasswordInput input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid) 
            {
                throw new EntityValidationException($"One or more validation errors occurred on type {nameof(input)}.", validationResult.ToDictionary());
            }

            var user = await _keycloakRepository.FindByIdAsync(input.UserId, cancellationToken);
            user.AddCredentials(new UserCredential(input.NewPassword, temporary: input.Temporary));

            await _keycloakRepository.ChangePasswordAsync(user, cancellationToken);
        }
    }
}
