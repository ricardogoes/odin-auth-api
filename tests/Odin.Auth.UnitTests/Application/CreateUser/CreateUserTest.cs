using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Odin.Auth.Application.ChangeStatusUser;
using Odin.Auth.Application.CreateUser;
using Odin.Auth.Domain.Entities;
using Odin.Auth.Domain.Exceptions;
using Odin.Auth.Domain.Interfaces;
using Odin.Auth.Domain.Models;
using Odin.Auth.Domain.ValuesObjects;
using App = Odin.Auth.Application.CreateUser;

namespace Odin.Auth.UnitTests.Application.CreateUser
{
    [Collection(nameof(CreateUserTestFixtureCollection))]
    public class CreateUserTest
    {
        private readonly CreateUserTestFixture _fixture;

        private readonly Mock<IValidator<CreateUserInput>> _validatorMock;
        private readonly Mock<IKeycloakRepository> _keycloakRepositoryMock;

        public CreateUserTest(CreateUserTestFixture fixture)
        {
            _fixture = fixture;

            _validatorMock = new();
            _keycloakRepositoryMock = _fixture.GetKeycloakRepositoryMock();
        }

        [Fact(DisplayName = "Handle() should create a user with valid data")]
        [Trait("Application", "CreateUser / CreateUser")]
        public async void CreateUser()
        {
            var input = _fixture.GetValidCreateUserInput();
            var userToInsert = new User(input.Username, input.FirstName, input.LastName, input.Email);
            userToInsert.AddGroup(new UserGroup(input.Groups.Select(group => group).First()));

            var expectedUserInserted = UserOutput.FromUser(userToInsert);

            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<CreateUserInput>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(new ValidationResult()));

            _keycloakRepositoryMock.Setup(s => s.CreateUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(userToInsert));

            var useCase = new App.CreateUser(_validatorMock.Object, _keycloakRepositoryMock.Object);
            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Username.Should().Be(expectedUserInserted.Username);
            output.FirstName.Should().Be(expectedUserInserted.FirstName);
            output.FirstName.Should().Be(expectedUserInserted.FirstName);
            output.LastName.Should().Be(expectedUserInserted.LastName);
            output.Enabled.Should().BeTrue();
            output.Id.Should().NotBeEmpty();
        }

        [Fact(DisplayName = "Handle() should throw an error when validation failed")]
        [Trait("Application", "CreateUser / CreateUser")]
        public async void CreateUser_ValidationFailed()
        {
            var userId = Guid.NewGuid();
            var input = _fixture.GetValidCreateUserInput();

            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<CreateUserInput>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Property", "'Property' must not be empty") })));

            var useCase = new App.CreateUser(_validatorMock.Object, _keycloakRepositoryMock.Object);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<EntityValidationException>();
        }

        [Theory(DisplayName = "Handle() should throw an error when data is invalid")]
        [Trait("Application", "CreateUser / CreateUser")]
        [MemberData(
            nameof(CreateUserTestDataGenerator.GetInvalidInputs),
            parameters: 12,
            MemberType = typeof(CreateUserTestDataGenerator)
        )]
        public async void ThrowWhenCantInstantiateUser(
            App.CreateUserInput input,
            string exceptionMessage
        )
        {
            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<CreateUserInput>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(new ValidationResult()));
            
            var useCase = new App.CreateUser(_validatorMock.Object, _keycloakRepositoryMock.Object);

            Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should()
                .ThrowAsync<EntityValidationException>()
                .WithMessage(exceptionMessage);
        }
    }
}
