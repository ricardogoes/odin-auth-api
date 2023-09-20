using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Odin.Auth.Application.Login;
using Odin.Auth.Application.Logout;
using Odin.Auth.Domain.Exceptions;
using Odin.Auth.Domain.Interfaces;
using Odin.Auth.Domain.Models;
using App = Odin.Auth.Application.Logout;

namespace Odin.Auth.UnitTests.Application.Logout
{
    [Collection(nameof(LogoutTestFixtureCollection))]
    public class LogoutTest
    {
        private readonly LogoutTestFixture _fixture;

        private readonly Mock<IValidator<LogoutInput>> _validatorMock;
        private readonly Mock<IKeycloakRepository> _keycloakRepositoryMock;

        public LogoutTest(LogoutTestFixture fixture)
        {
            _fixture = fixture;

            _validatorMock = new();
            _keycloakRepositoryMock = _fixture.GetKeycloakRepositoryMock();
        }

        [Fact(DisplayName = "Handle() should logout with valid data")]
        [Trait("Application", "Logout / Logout")]
        public async void Logout()
        {
            var input = _fixture.GetValidLogoutInput();

            var expectedOutput = new KeycloakAuthResponse("access-token", "id-token", "refresh-token", 3600);

            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<LogoutInput>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(new ValidationResult()));

            _keycloakRepositoryMock.Setup(s => s.LogoutAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .Verifiable();

            var useCase = new App.Logout(_validatorMock.Object, _keycloakRepositoryMock.Object);
            await useCase.Handle(input, CancellationToken.None);

            _keycloakRepositoryMock.Verify(s => s.LogoutAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
               
        }

        [Fact(DisplayName = "Handle() should throw an error when validation failed")]
        [Trait("Application", "Logout / Logout")]
        public async void Logout_ValidationFailed()
        {
            var input = _fixture.GetValidLogoutInput();

            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<LogoutInput>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Property", "'Property' must not be empty") })));

            var useCase = new App.Logout(_validatorMock.Object, _keycloakRepositoryMock.Object);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<EntityValidationException>();
        }
    }
}
