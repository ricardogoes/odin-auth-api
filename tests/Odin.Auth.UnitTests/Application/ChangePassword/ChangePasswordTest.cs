using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Odin.Auth.Application.ChangePassword;
using Odin.Auth.Domain.Entities;
using Odin.Auth.Domain.Exceptions;
using Odin.Auth.Domain.Interfaces;
using App = Odin.Auth.Application.ChangePassword;

namespace Odin.Auth.UnitTests.Application.ChangePassword
{
    [Collection(nameof(ChangePasswordTestFixtureCollection))]
    public class ChangePasswordTest
    {
        private readonly ChangePasswordTestFixture _fixture;

        private readonly Mock<IKeycloakRepository> _keycloakRepositoryMock;
        private readonly Mock<IValidator<ChangePasswordInput>> _validatorMock;

        public ChangePasswordTest(ChangePasswordTestFixture fixture)
        {
            _fixture = fixture;

            _keycloakRepositoryMock = _fixture.GetKeycloakRepositoryMock();
            _validatorMock = new();
        }

        [Fact(DisplayName = "Handle() should change password with valid data")]
        [Trait("Application", "ChangePassword / ChangePassword")]
        public async void ChangePassword()
        {
            var userId = Guid.NewGuid();
            var input = _fixture.GetValidChangePasswordInput(userId);

            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<ChangePasswordInput>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(new ValidationResult()));

            _keycloakRepositoryMock.Setup(x => x.FindByIdAsync(input.UserId, It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(_fixture.GetValidUser()));

            _keycloakRepositoryMock.Setup(s => s.ChangePasswordAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .Verifiable();

            var useCase = new App.ChangePassword(_validatorMock.Object, _keycloakRepositoryMock.Object);
            await useCase.Handle(input, CancellationToken.None);

            _keycloakRepositoryMock.Verify(x => x.ChangePasswordAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Handle() should throw an error when validation failed")]
        [Trait("Application", "ChangePassword / ChangePassword")]
        public async void ChangePassword_ValidationFailed()
        {
            var userId = Guid.NewGuid();
            var input = _fixture.GetValidChangePasswordInput(userId);

            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<ChangePasswordInput>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Property", "'Property' must not be empty") })));

            var useCase = new App.ChangePassword(_validatorMock.Object, _keycloakRepositoryMock.Object);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<EntityValidationException>();
        }

        [Fact(DisplayName = "Handle() should throw an error when user not found")]
        [Trait("Application", "ChangePassword / ChangePassword")]
        public async Task ThrowWhenCustomerNotFound()
        {
            var input = _fixture.GetValidChangePasswordInput();

            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<ChangePasswordInput>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(new ValidationResult()));

            _keycloakRepositoryMock.Setup(x => x.FindByIdAsync(input.UserId, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new NotFoundException($"User '{input.UserId}' not found"));

            var useCase = new App.ChangePassword(_validatorMock.Object, _keycloakRepositoryMock.Object);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<NotFoundException>();

            _keycloakRepositoryMock.Verify(x => x.FindByIdAsync(input.UserId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
