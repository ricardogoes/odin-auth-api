using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Odin.Auth.Application.Logout;
using Odin.Auth.Application.UpdateProfile;
using Odin.Auth.Domain.Entities;
using Odin.Auth.Domain.Exceptions;
using Odin.Auth.Domain.Interfaces;
using Odin.Auth.Domain.Models;
using App = Odin.Auth.Application.UpdateProfile;

namespace Odin.Auth.UnitTests.Application.UpdateProfile
{
    [Collection(nameof(UpdateProfileTestFixtureCollection))]
    public class UpdateProfileTest
    {
        private readonly UpdateProfileTestFixture _fixture;

        private readonly Mock<IValidator<UpdateProfileInput>> _validatorMock;
        private readonly Mock<IKeycloakRepository> _keycloakRepositoryMock;

        public UpdateProfileTest(UpdateProfileTestFixture fixture)
        {
            _fixture = fixture;

            _validatorMock = new();
            _keycloakRepositoryMock = _fixture.GetKeycloakRepositoryMock();
        }

        [Fact(DisplayName = "Handle() should create a user with valid data")]
        [Trait("Application", "UpdateProfile / UpdateProfile")]
        public async void UpdateProfile()
        {
            var input = _fixture.GetValidUpdateProfileInput();
            var userToUpdate = new User("unit.testing", input.FirstName, input.LastName, input.Email);
            var expectedUserUpdated = UserOutput.FromUser(userToUpdate);

            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<UpdateProfileInput>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(new ValidationResult()));

            _keycloakRepositoryMock.Setup(s => s.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(userToUpdate));

            _keycloakRepositoryMock.Setup(s => s.UpdateUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(userToUpdate));

            var useCase = new App.UpdateProfile(_validatorMock.Object, _keycloakRepositoryMock.Object);
            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Username.Should().Be(expectedUserUpdated.Username);
            output.FirstName.Should().Be(expectedUserUpdated.FirstName);
            output.FirstName.Should().Be(expectedUserUpdated.FirstName);
            output.LastName.Should().Be(expectedUserUpdated.LastName);
            output.Enabled.Should().BeTrue();
            output.Id.Should().NotBeEmpty();
        }

        [Fact(DisplayName = "Handle() should throw an error when validation failed")]
        [Trait("Application", "UpdateProfile / UpdateProfile")]
        public async void UpdateProfile_ValidationFailed()
        {
            var input = _fixture.GetValidUpdateProfileInput();

            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<UpdateProfileInput>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Property", "'Property' must not be empty") })));

            var useCase = new App.UpdateProfile(_validatorMock.Object, _keycloakRepositoryMock.Object);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<EntityValidationException>();
        }

        [Fact(DisplayName = "Handle() should throw an error when user not found")]
        [Trait("Application", "UpdateProfile / UpdateProfile")]
        public async Task ThrowWhenUserNotFound()
        {
            var input = _fixture.GetValidUpdateProfileInput();

            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<UpdateProfileInput>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(new ValidationResult()));

            _keycloakRepositoryMock.Setup(x => x.FindByIdAsync(input.UserId, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new NotFoundException($"User '{input.UserId}' not found"));

            var useCase = new App.UpdateProfile(_validatorMock.Object, _keycloakRepositoryMock.Object);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<NotFoundException>();

            _keycloakRepositoryMock.Verify(x => x.FindByIdAsync(input.UserId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
