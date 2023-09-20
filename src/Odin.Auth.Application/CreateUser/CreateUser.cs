using FluentValidation;
using MediatR;
using Odin.Auth.Domain.Entities;
using Odin.Auth.Domain.Exceptions;
using Odin.Auth.Domain.Interfaces;
using Odin.Auth.Domain.Models;
using Odin.Auth.Domain.ValuesObjects;

namespace Odin.Auth.Application.CreateUser
{
    public class CreateUser : IRequestHandler<CreateUserInput, UserOutput>
    {
        private readonly IValidator<CreateUserInput> _validator;
        private readonly IKeycloakRepository _keycloakRepository;

        public CreateUser(IValidator<CreateUserInput> validator, IKeycloakRepository keycloakRepository)
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

            user.AddAttribute(new KeyValuePair<string, string>("created_at", DateTime.Now.ToString("o")));
            user.AddAttribute(new KeyValuePair<string, string>("created_by", input.LoggedUsername!)) ;
            user.AddAttribute(new KeyValuePair<string, string>("last_updated_at", DateTime.Now.ToString("o")));
            user.AddAttribute(new KeyValuePair<string, string>("last_updated_by", input.LoggedUsername!));

            await _keycloakRepository.CreateUserAsync(user, cancellationToken);

            return UserOutput.FromUser(user);
        }
    }
}
