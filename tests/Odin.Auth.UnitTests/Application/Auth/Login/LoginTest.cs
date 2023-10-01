using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Odin.Auth.Domain.Exceptions;
using Odin.Auth.Infra.Keycloak.Exceptions;
using Odin.Auth.Infra.Keycloak.Interfaces;
using Odin.Auth.Infra.Keycloak.Models;
using App = Odin.Auth.Application.Auth.Login;

namespace Odin.Auth.UnitTests.Application.Auth.Login
{
    [Collection(nameof(LoginTestFixtureCollection))]
    public class LoginTest
    {
        private readonly LoginTestFixture _fixture;

        private readonly Mock<IValidator<App.LoginInput>> _validatorMock;
        private readonly Mock<IAuthKeycloakRepository> _keycloakRepositoryMock;

        public LoginTest(LoginTestFixture fixture)
        {
            _fixture = fixture;

            _validatorMock = new();
            _keycloakRepositoryMock = _fixture.GetAuthKeycloakRepositoryMock();
        }

        [Fact(DisplayName = "Handle() should login with valid data")]
        [Trait("Application", "Login / Login")]
        public async void Login()
        {
            var input = _fixture.GetValidLoginInput();

            var expectedOutput = new KeycloakAuthResponse("access-token", "id-token", "refresh-token", 3600);

            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<App.LoginInput>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(new ValidationResult()));

            _keycloakRepositoryMock.Setup(s => s.AuthAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(expectedOutput));

            var useCase = new App.Login(_validatorMock.Object, _keycloakRepositoryMock.Object);
            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.AccessToken.Should().Be(expectedOutput.AccessToken);
            output.IdToken.Should().Be(expectedOutput.IdToken);
            output.RefreshToken.Should().Be(expectedOutput.RefreshToken);
            output.ExpiresIn.Should().Be(expectedOutput.ExpiresIn);
        }

        [Fact(DisplayName = "Handle() should throw an error when validation failed")]
        [Trait("Application", "Login / Login")]
        public async void Login_ValidationFailed()
        {
            var input = new App.LoginInput("admin", "admin");

            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<App.LoginInput>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Property", "'Property' must not be empty") })));

            var useCase = new App.Login(_validatorMock.Object, _keycloakRepositoryMock.Object);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<EntityValidationException>();
        }

        [Theory(DisplayName = "Handle() should throw an error when data is invalid")]
        [Trait("Application", "Login / Login")]
        [MemberData(
            nameof(LoginTestDataGenerator.GetInvalidInputs),
            parameters: 12,
            MemberType = typeof(LoginTestDataGenerator)
        )]
        public async void ThrowWhenCantInstantiateUser(App.LoginInput input)
        {
            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<App.LoginInput>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(new ValidationResult()));

            _keycloakRepositoryMock.Setup(s => s.AuthAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
               .Throws(new KeycloakException("Error"));

            var useCase = new App.Login(_validatorMock.Object, _keycloakRepositoryMock.Object);

            Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<KeycloakException>();
        }
    }
}
