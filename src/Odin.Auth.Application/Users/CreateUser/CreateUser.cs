using FluentValidation;
using MediatR;
using Odin.Auth.Domain.Entities;
using Odin.Auth.Domain.Exceptions;
using Odin.Auth.Domain.ValuesObjects;
using Odin.Auth.Infra.Keycloak.Interfaces;

namespace Odin.Auth.Application.Users.CreateUser
{
    public class CreateUser : IRequestHandler<CreateUserInput, UserOutput>
    {
        private readonly IValidator<CreateUserInput> _validator;
        private readonly IUserKeycloakRepository _keycloakRepository;

        public CreateUser(IValidator<CreateUserInput> validator, IUserKeycloakRepository keycloakRepository)
        {
            _validator = validator;
            _keycloakRepository = keycloakRepository;
        }

        public async Task<UserOutput> Handle(CreateUserInput input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new EntityValidationException($"One or more validation errors occurred on type {nameof(input)}.", validationResult.ToDictionary());
            }

            var user = new User(input.Username, input.FirstName, input.LastName, input.Email);

            var credential = new UserCredential(input.Password, temporary: input.PasswordIsTemporary);
            user.AddCredentials(credential);

            if (input.Groups.Any())
            {
                input.Groups.ForEach(group => user.AddGroup(new UserGroup(group)));
            }

            var userInserted = await _keycloakRepository.CreateUserAsync(user, cancellationToken);

            return UserOutput.FromUser(userInserted);
        }
    }
}
