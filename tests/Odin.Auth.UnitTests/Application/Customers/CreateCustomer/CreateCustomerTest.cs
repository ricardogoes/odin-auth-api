using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Odin.Auth.Application.Customers;
using Odin.Auth.Application.Customers.CreateCustomer;
using Odin.Auth.Domain.Entities;
using Odin.Auth.Domain.Exceptions;
using Odin.Auth.Domain.Interfaces;
using Odin.Auth.Infra.Data.EF.Interfaces;
using App = Odin.Auth.Application.Customers.CreateCustomer;

namespace Odin.Auth.UnitTests.Application.Customers.CreateCustomer
{
    [Collection(nameof(CreateCustomerTestFixtureCollection))]
    public class CreateCustomerTest
    {
        private readonly CreateCustomerTestFixture _fixture;

        private readonly Mock<IDocumentService> _documentServiceMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly Mock<IValidator<CreateCustomerInput>> _validatorMock;

        public CreateCustomerTest(CreateCustomerTestFixture fixture)
        {
            _fixture = fixture;

            _documentServiceMock = _fixture.GetDocumentServiceMock();
            _unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            _customerRepositoryMock = _fixture.GetCustomerRepositoryMock();
            _validatorMock = new();
        }

        [Fact(DisplayName = "Handle() should create a customer without address")]
        [Trait("Application", "Customers / CreateCustomer")]
        public async void CreateCustomer_WithoutAddress()
        {
            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<CreateCustomerInput>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ValidationResult());

            var input = _fixture.GetValidCreateCustomerInputWithoutAddress();
            var customerToInsert = new Customer(input.Name, input.Document, isActive: true);
            var expectedCustomerInserted = new CustomerOutput
            (
                Guid.NewGuid(),
                input.Name,
                input.Document,
                null,
                true,
                DateTime.UtcNow, 
                "unit.testing",
                DateTime.UtcNow,
                "unit.testing"                
            );

            _documentServiceMock.Setup(s => s.IsDocumentUnique(It.IsAny<Customer>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(true));

            _customerRepositoryMock.Setup(s => s.InsertAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(customerToInsert));

            var useCase = new App.CreateCustomer(_documentServiceMock.Object, _unitOfWorkMock.Object, _customerRepositoryMock.Object, _validatorMock.Object);
            var output = await useCase.Handle(input, CancellationToken.None);

            _unitOfWorkMock.Verify(
                uow => uow.CommitAsync(It.IsAny<CancellationToken>()),
                Times.Once
            );

            output.Should().NotBeNull();
            output.Name.Should().Be(expectedCustomerInserted.Name);
            output.Document.Should().Be(expectedCustomerInserted.Document);
            output.IsActive.Should().BeTrue();
            output.Id.Should().NotBeEmpty();
        }

        [Fact(DisplayName = "Handle() should create a customer with address")]
        [Trait("Application", "Customers / CreateCustomer")]
        public async void CreateCustomer_WithAddress()
        {
            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<CreateCustomerInput>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ValidationResult());

            var input = _fixture.GetValidCreateCustomerInputWithAddress();
            var customerToInsert = new Customer(input.Name, input.Document, isActive: true);
            customerToInsert.ChangeAddress(input.Address!);
            
            var expectedCustomerInserted = new CustomerOutput
            (
                Guid.NewGuid(),
                input.Name,
                input.Document,
                input.Address,
                true,
                DateTime.UtcNow,
                "unit.testing",
                DateTime.UtcNow,
                "unit.testing"
            );

            _documentServiceMock.Setup(s => s.IsDocumentUnique(It.IsAny<Customer>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(true));

            _customerRepositoryMock.Setup(s => s.InsertAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(customerToInsert));

            var useCase = new App.CreateCustomer(_documentServiceMock.Object, _unitOfWorkMock.Object, _customerRepositoryMock.Object, _validatorMock.Object);
            var output = await useCase.Handle(input, CancellationToken.None);

            _unitOfWorkMock.Verify(
                uow => uow.CommitAsync(It.IsAny<CancellationToken>()),
                Times.Once
            );

            output.Should().NotBeNull();
            output.Name.Should().Be(expectedCustomerInserted.Name);
            output.Document.Should().Be(expectedCustomerInserted.Document);
            output.IsActive.Should().BeTrue();
            output.Id.Should().NotBeEmpty();

            output.Address.Should().NotBeNull();
            output.Address!.StreetName.Should().Be(input.Address!.StreetName);
            output.Address!.StreetNumber.Should().Be(input.Address!.StreetNumber);
            output.Address!.Complement.Should().Be(input.Address!.Complement);
            output.Address!.Neighborhood.Should().Be(input.Address!.Neighborhood);
            output.Address!.ZipCode.Should().Be(input.Address!.ZipCode);
            output.Address!.City.Should().Be(input.Address!.City);
            output.Address!.State.Should().Be(input.Address!.State);


        }

        [Fact(DisplayName = "Handle() should throw an error when validation failed")]
        [Trait("Application", "Customers / CreateCustomer")]
        public async void CreateCustomer_ValidationFailed()
        {
            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<CreateCustomerInput>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Property", "'Property' must not be empty") })));

            var input = _fixture.GetValidCreateCustomerInputWithoutAddress();
            var useCase = new App.CreateCustomer(_documentServiceMock.Object, _unitOfWorkMock.Object, _customerRepositoryMock.Object, _validatorMock.Object);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<EntityValidationException>();
        }

        [Theory(DisplayName = "Handle() should throw an error when data is invalid")]
        [Trait("Application", "Customers / CreateCustomer")]
        [MemberData(
            nameof(CreateCustomerTestDataGenerator.GetInvalidInputs),
            parameters: 12,
            MemberType = typeof(CreateCustomerTestDataGenerator)
        )]
        public async void ThrowWhenCantInstantiateCustomer(App.CreateCustomerInput input, string exceptionMessage)
        {
            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<CreateCustomerInput>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ValidationResult());

            var useCase = new App.CreateCustomer(_documentServiceMock.Object, _unitOfWorkMock.Object, _customerRepositoryMock.Object, _validatorMock.Object);

            Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should()
                .ThrowAsync<EntityValidationException>()
                .WithMessage(exceptionMessage);
        }


        [Fact(DisplayName = "Handle() should throw an error when document already exists")]
        [Trait("Application", "Customers / CreateCustomer")]
        public async void ThrowWhenDocumentAlreadyExists()
        {
            var input = _fixture.GetValidCreateCustomerInputWithoutAddress();

            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<CreateCustomerInput>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ValidationResult());

            _documentServiceMock.Setup(s => s.IsDocumentUnique(It.IsAny<Customer>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(false));

            var useCase = new App.CreateCustomer(_documentServiceMock.Object, _unitOfWorkMock.Object, _customerRepositoryMock.Object, _validatorMock.Object);

            Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should()
                .ThrowAsync<EntityValidationException>()
                .WithMessage("Document must be unique");
        }
    }
}
