using FluentValidation;
using MediatR;
using Odin.Auth.Domain.Exceptions;
using Odin.Auth.Domain.Interfaces;
using Odin.Auth.Domain.Models;

namespace Odin.Auth.Application.ChangeStatusUser
{
    public class ChangeStatusUser : IRequestHandler<ChangeStatusUserInput, UserOutput>
    {
        private readonly IValidator<ChangeStatusUserInput> _validator;
        private readonly IKeycloakRepository _keycloakRepository;

        public ChangeStatusUser(IValidator<ChangeStatusUserInput> validator, IKeycloakRepository keycloakRepository)
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
                        
            user.AddAttribute(new KeyValuePair<string, string>("last_updated_at", DateTime.Now.ToString("o")));
            user.AddAttribute(new KeyValuePair<string, string>("last_updated_by", input.LoggedUsername));

            await _keycloakRepository.UpdateUserAsync(user, cancellationToken);

            return UserOutput.FromUser(user);
        }
    }
}
