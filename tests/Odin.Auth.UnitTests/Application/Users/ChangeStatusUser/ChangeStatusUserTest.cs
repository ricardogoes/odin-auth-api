using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Odin.Auth.Application.Users.ChangeStatusUser;
using Odin.Auth.Domain.Entities;
using Odin.Auth.Domain.Exceptions;
using Odin.Auth.Infra.Keycloak.Interfaces;
using App = Odin.Auth.Application.Users.ChangeStatusUser;

namespace Odin.Auth.UnitTests.Application.Users.ChangeStatusUser
{
    [Collection(nameof(ChangeStatusUserTestFixtureCollection))]
    public class ChangeStatusUserTest
    {
        private readonly ChangeStatusUserTestFixture _fixture;

        private readonly Mock<IValidator<ChangeStatusUserInput>> _validatorMock;
        private readonly Mock<IUserKeycloakRepository> _keycloakRepositoryMock;

        public ChangeStatusUserTest(ChangeStatusUserTestFixture fixture)
        {
            _fixture = fixture;
            _keycloakRepositoryMock = _fixture.GetUserKeycloakRepositoryMock();
            _validatorMock = new();
        }

        [Fact(DisplayName = "Handle() should activate a user with valid data")]
        [Trait("Application", "ChangeStatusUser / ChangeStatusUser")]
        public async Task ActivateUser()
        {
            var validUser = _fixture.GetValidUser();
            var input = _fixture.GetValidChangeStatusUserInputToActivate();

            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<ChangeStatusUserInput>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(new ValidationResult()));

            _keycloakRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validUser);

            _keycloakRepositoryMock.Setup(x => x.UpdateUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(validUser));

            var useCase = new App.ChangeStatusUser(_validatorMock.Object, _keycloakRepositoryMock.Object);
            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.IsActive.Should().BeTrue();

            _keycloakRepositoryMock.Verify(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _keycloakRepositoryMock.Verify(x => x.UpdateUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Handle() should deactivate a user with valid data")]
        [Trait("Application", "ChangeStatusUser / ChangeStatusUser")]
        public async Task DeactivateUser()
        {
            var validUser = _fixture.GetValidUser();
            var input = _fixture.GetValidChangeStatusUserInputToDeactivate();

            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<ChangeStatusUserInput>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(new ValidationResult()));

            _keycloakRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validUser);

            _keycloakRepositoryMock.Setup(x => x.UpdateUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(validUser));

            var useCase = new App.ChangeStatusUser(_validatorMock.Object, _keycloakRepositoryMock.Object);
            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.IsActive.Should().BeFalse();

            _keycloakRepositoryMock.Verify(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _keycloakRepositoryMock.Verify(x => x.UpdateUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Handle() should throw an error when validation failed")]
        [Trait("Application", "ChangeStatusUser / ChangeStatusUser")]
        public async void ChangeStatusUser_ValidationFailed()
        {
            var userId = Guid.NewGuid();
            var input = _fixture.GetValidChangeStatusUserInputToActivate(userId);

            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<ChangeStatusUserInput>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Property", "'Property' must not be empty") })));

            var useCase = new App.ChangeStatusUser(_validatorMock.Object, _keycloakRepositoryMock.Object);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<EntityValidationException>();
        }

        [Fact(DisplayName = "Handle() should throw an error when user not found")]
        [Trait("Application", "ChangeStatusUser / ChangeStatusUser")]
        public async Task ThrowWhenUserNotFound()
        {
            var input = _fixture.GetValidChangeStatusUserInputToActivate();

            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<ChangeStatusUserInput>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(new ValidationResult()));

            _keycloakRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new NotFoundException($"User '{input.UserId}' not found"));

            var useCase = new App.ChangeStatusUser(_validatorMock.Object, _keycloakRepositoryMock.Object);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<NotFoundException>();

            _keycloakRepositoryMock.Verify(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
