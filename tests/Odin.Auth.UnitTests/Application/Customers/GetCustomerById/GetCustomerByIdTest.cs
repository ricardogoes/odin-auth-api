using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Odin.Auth.Application.Customers.GetCustomerById;
using Odin.Auth.Domain.Exceptions;
using Odin.Auth.Domain.Interfaces;
using App = Odin.Auth.Application.Customers.GetCustomerById;

namespace Odin.Auth.UnitTests.Application.Customers.GetCustomerById
{
    [Collection(nameof(GetCustomerByIdTestFixture))]
    public class GetCustomerByIdTest
    {
        private readonly GetCustomerByIdTestFixture _fixture;

        private readonly Mock<ICustomerRepository> _repositoryMock;
        private readonly Mock<IValidator<GetCustomerByIdInput>> _validatorMock;

        public GetCustomerByIdTest(GetCustomerByIdTestFixture fixture)
        {
            _fixture = fixture;
            _repositoryMock = _fixture.GetCustomerRepositoryMock();
            _validatorMock = new();
        }

        [Fact(DisplayName = "Handle() should get a customer when searched by valid Id")]
        [Trait("Application", "Customers / GetCustomerById")]
        public async Task GetCustomerById()
        {
            var validCustomer = _fixture.GetValidCustomer();

            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<GetCustomerByIdInput>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ValidationResult());

            _repositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validCustomer);

            var input = new App.GetCustomerByIdInput
            {
                Id = validCustomer.Id
            };

            var useCase = new App.GetCustomerById(_repositoryMock.Object, _validatorMock.Object);

            var output = await useCase.Handle(input, CancellationToken.None);

            _repositoryMock.Verify(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

            output.Should().NotBeNull();
            output.Name.Should().Be(validCustomer.Name);
            output.Document.Should().Be(validCustomer.Document);
            output.IsActive.Should().Be(validCustomer.IsActive);
            output.Id.Should().Be(validCustomer.Id);
            output.CreatedAt.Should().Be(validCustomer.CreatedAt);
        }

        [Fact(DisplayName = "Handle() should throw an error when validation failed")]
        [Trait("Application", "Customers / GetCustomerById")]
        public async void GetCustomerById_ValidationFailed()
        {
            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<GetCustomerByIdInput>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Property", "'Property' must not be empty") })));

            var useCase = new App.GetCustomerById(_repositoryMock.Object, _validatorMock.Object);

            var task = async () => await useCase.Handle(new App.GetCustomerByIdInput { Id = Guid.NewGuid() }, CancellationToken.None);

            await task.Should().ThrowAsync<EntityValidationException>();
        }

        [Fact(DisplayName = "Handle() should throw an error when customer does not exist")]
        [Trait("Application", "Customers / GetCustomerById")]
        public async Task NotFoundExceptionWhenCustomerDoesntExist()
        {
            var exampleGuid = Guid.NewGuid();

            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<GetCustomerByIdInput>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ValidationResult());

            _repositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new NotFoundException($"Customer '{exampleGuid}' not found"));

            var input = new App.GetCustomerByIdInput
            {
                Id = exampleGuid
            };

            var useCase = new App.GetCustomerById(_repositoryMock.Object, _validatorMock.Object);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<NotFoundException>();
            _repositoryMock.Verify(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
