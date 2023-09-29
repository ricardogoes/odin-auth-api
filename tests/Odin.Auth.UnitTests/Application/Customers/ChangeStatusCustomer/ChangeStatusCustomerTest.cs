using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Odin.Auth.Application.Customers.ChangeStatusCustomer;
using Odin.Auth.Domain.Entities;
using Odin.Auth.Domain.Exceptions;
using Odin.Auth.Domain.Interfaces;
using Odin.Auth.Infra.Data.EF.Interfaces;
using App = Odin.Auth.Application.Customers.ChangeStatusCustomer;

namespace Odin.Auth.UnitTests.Application.Customers.ChangeStatusCustomer
{
    [Collection(nameof(ChangeStatusCustomerTestFixtureCollection))]
    public class ChangeStatusCustomerTest
    {
        private readonly ChangeStatusCustomerTestFixture _fixture;

        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ICustomerRepository> _repositoryMock;
        private readonly Mock<IValidator<ChangeStatusCustomerInput>> _validatorMock;

        public ChangeStatusCustomerTest(ChangeStatusCustomerTestFixture fixture)
        {
            _fixture = fixture;
            _unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            _repositoryMock = _fixture.GetCustomerRepositoryMock();
            _validatorMock = new();
        }

        [Fact(DisplayName = "Handle() should activate a customer with valid data")]
        [Trait("Application", "Customers / ChangeStatusCustomer")]
        public async Task ActivateCustomer()
        {
            var validCustomer = _fixture.GetValidCustomer();
            var input = _fixture.GetValidChangeStatusCustomerInputToActivate();

            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<ChangeStatusCustomerInput>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ValidationResult());

            _repositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validCustomer);

            _repositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(validCustomer));

            var useCase = new App.ChangeStatusCustomer(_unitOfWorkMock.Object, _repositoryMock.Object, _validatorMock.Object);
            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.IsActive.Should().BeTrue();
            
            _repositoryMock.Verify(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Handle() should deactivate a customer with valid data")]
        [Trait("Application", "Customers / ChangeStatusCustomer")]
        public async Task DeactivateCustomer()
        {
            var validCustomer = _fixture.GetValidCustomer();
            var input = _fixture.GetValidChangeStatusCustomerInputToDeactivate();

            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<ChangeStatusCustomerInput>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ValidationResult());

            _repositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validCustomer);

            _repositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(validCustomer));

            var useCase = new App.ChangeStatusCustomer(_unitOfWorkMock.Object, _repositoryMock.Object, _validatorMock.Object);
            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.IsActive.Should().BeFalse();

            _repositoryMock.Verify(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
        
        [Fact(DisplayName = "Handle() should throw an error when validation failed")]
        [Trait("Application", "Customers / ChangeStatusCustomer")]
        public async void FluentValidationFailed()
        {
            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<ChangeStatusCustomerInput>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Property", "'Property' must not be empty") })));

            var input = _fixture.GetValidChangeStatusCustomerInputToActivate();
            var useCase = new App.ChangeStatusCustomer(_unitOfWorkMock.Object, _repositoryMock.Object, _validatorMock.Object);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<EntityValidationException>();
        }

        [Fact(DisplayName = "Handle() should throw an error when customer not found")]
        [Trait("Application", "Customers / ChangeStatusCustomer")]
        public async Task ThrowWhenCustomerNotFound()
        {
            var input = _fixture.GetValidChangeStatusCustomerInputToActivate();

            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<ChangeStatusCustomerInput>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ValidationResult());

            _repositoryMock.Setup(x => x.FindByIdAsync(input.Id, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new NotFoundException($"Customer '{input.Id}' not found"));

            var useCase = new App.ChangeStatusCustomer(_unitOfWorkMock.Object, _repositoryMock.Object, _validatorMock.Object);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<NotFoundException>();

            _repositoryMock.Verify(x => x.FindByIdAsync(input.Id, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
