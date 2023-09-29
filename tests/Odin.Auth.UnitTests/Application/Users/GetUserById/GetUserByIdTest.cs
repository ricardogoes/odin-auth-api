using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Odin.Auth.Domain.Exceptions;
using Odin.Auth.Infra.Keycloak.Interfaces;
using App = Odin.Auth.Application.Users.GetUserById;

namespace Odin.Auth.UnitTests.Application.Users.GetUserById
{
    [Collection(nameof(GetUserByIdTestFixture))]
    public class GetUserByIdTest
    {
        private readonly GetUserByIdTestFixture _fixture;

        private readonly Mock<IValidator<App.GetUserByIdInput>> _validatorMock;
        private readonly Mock<IUserKeycloakRepository> _keycloakRepositoryMock;

        public GetUserByIdTest(GetUserByIdTestFixture fixture)
        {
            _fixture = fixture;

            _validatorMock = new();
            _keycloakRepositoryMock = _fixture.GetUserKeycloakRepositoryMock();
        }

        [Fact(DisplayName = "Handle() should get a user when searched by valid Id")]
        [Trait("Application", "GetUserById / GetUserById")]
        public async Task GetUserById()
        {
            var tenantId = Guid.NewGuid();
            var validUser = _fixture.GetValidUser(tenantId);

            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<App.GetUserByIdInput>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(new ValidationResult()));

            _keycloakRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validUser);

            var input = new App.GetUserByIdInput
            (
                tenantId: tenantId,
                userId: validUser.Id
            );

            var useCase = new App.GetUserById(_validatorMock.Object, _keycloakRepositoryMock.Object);

            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Username.Should().Be(validUser.Username);
            output.FirstName.Should().Be(validUser.FirstName);
            output.LastName.Should().Be(validUser.LastName);
            output.Email.Should().Be(validUser.Email);
            output.IsActive.Should().Be(validUser.IsActive);
            output.Id.Should().Be(validUser.Id);

            _keycloakRepositoryMock.Verify(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Handle() should throw an error when validation failed")]
        [Trait("Application", "GetUserById / GetUserById")]
        public async void GetUserById_ValidationFailed()
        {
            var userId = Guid.NewGuid();
            var input = _fixture.GetValidGetUserByIdInput(userId);

            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<App.GetUserByIdInput>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Property", "'Property' must not be empty") })));

            var useCase = new App.GetUserById(_validatorMock.Object, _keycloakRepositoryMock.Object);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<EntityValidationException>();
        }

        [Fact(DisplayName = "Handle() should throw an error when user does not exist")]
        [Trait("Application", "GetUserById / GetUserById")]
        public async Task NotFoundExceptionWhenUserDoesntExist()
        {
            var guidToQuery = Guid.NewGuid();
            var tenantToQuery = Guid.NewGuid();

            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<App.GetUserByIdInput>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(new ValidationResult()));

            _keycloakRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new NotFoundException($"User '{guidToQuery}' not found"));

            var input = new App.GetUserByIdInput
            (
                tenantId: tenantToQuery,
                userId: guidToQuery
            );

            var useCase = new App.GetUserById(_validatorMock.Object, _keycloakRepositoryMock.Object);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<NotFoundException>();
            _keycloakRepositoryMock.Verify(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
