using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Odin.Auth.Application.Customers.ChangeAddressCustomer;
using Odin.Auth.Domain.Exceptions;
using Odin.Auth.Domain.Interfaces;
using Odin.Auth.Infra.Data.EF.Interfaces;
using App = Odin.Auth.Application.Customers.ChangeAddressCustomer;

namespace Odin.Auth.UnitTests.Application.Customers.ChangeAddressCustomer
{
    [Collection(nameof(ChangeAddressCustomerTestFixtureCollection))]
    public class ChangeAddressCustomerTest
    {
        private readonly ChangeAddressCustomerTestFixture _fixture;

        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ICustomerRepository> _repositoryMock;
        private readonly Mock<IValidator<ChangeAddressCustomerInput>> _validatorMock;

        public ChangeAddressCustomerTest(ChangeAddressCustomerTestFixture fixture)
        {
            _fixture = fixture;
            _unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            _repositoryMock = _fixture.GetCustomerRepositoryMock();
            _validatorMock = new();
        }
        
        [Fact(DisplayName = "Handle() should change address with valid data")]
        [Trait("Application", "Customers / ChangeAddressCustomer")]
        public async void ChangeAddress()
        {
            var validCustomer = _fixture.GetValidCustomer();
            
            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<ChangeAddressCustomerInput>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ValidationResult());

            _repositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validCustomer);

            var input = _fixture.GetValidInputAddress();
            var useCase = new App.ChangeAddressCustomer(_unitOfWorkMock.Object, _repositoryMock.Object, _validatorMock.Object);

            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Address.Should().NotBeNull();
            output.Address!.StreetName.Should().Be(input.StreetName);
            output.Address.StreetNumber.Should().Be(input.StreetNumber);
            output.Address.Complement.Should().Be(input.Complement);
            output.Address.Neighborhood.Should().Be(input.Neighborhood);
            output.Address.ZipCode.Should().Be(input.ZipCode);
            output.Address.City.Should().Be(input.City);
            output.Address.State.Should().Be(input.State);

            _repositoryMock.Verify(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Handle() should throw an error when validation failed")]
        [Trait("Application", "Customers / ChangeAddressCustomer")]
        public async void ChangeAddress_ValidationFailed()
        {
            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<ChangeAddressCustomerInput>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Property", "'Property' must not be empty") })));

            var input = _fixture.GetValidInputAddress();
            var useCase = new App.ChangeAddressCustomer(_unitOfWorkMock.Object, _repositoryMock.Object, _validatorMock.Object);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<EntityValidationException>();
        }

        [Fact(DisplayName = "Handle() should throw an error when customer not found")]
        [Trait("Application", "Customers / ChangeAddressCustomer")]
        public async void ChangeAddress_ThrowErrorCustomerNotFound()
        {
            var validCustomer = _fixture.GetValidCustomer();

            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<ChangeAddressCustomerInput>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(new ValidationResult());

            _repositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new NotFoundException("Customer not found"));

            var input = _fixture.GetValidInputAddress();
            var useCase = new App.ChangeAddressCustomer(_unitOfWorkMock.Object, _repositoryMock.Object, _validatorMock.Object);

            var task = async () => await useCase.Handle(input, CancellationToken.None);
            await task.Should().ThrowAsync<NotFoundException>();

            _repositoryMock.Verify(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
