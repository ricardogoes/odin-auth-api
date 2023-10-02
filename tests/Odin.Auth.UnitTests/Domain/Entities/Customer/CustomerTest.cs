using FluentAssertions;
using Odin.Auth.Domain.Exceptions;
using DomainEntity = Odin.Auth.Domain.Entities;

namespace Odin.Auth.UnitTests.Domain.Entities.Customer
{
    [Collection(nameof(CustomerTestFixture))]
    public class CustomerTest
    {
        private readonly CustomerTestFixture _fixture;

        public CustomerTest(CustomerTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "ctor() should instantiate a new customer")]
        [Trait("Domain", "Entities / Customer")]
        public void Instantiate()
        {
            var validCustomer = _fixture.GetValidCustomer();
            var customer = new DomainEntity.Customer(validCustomer.Name, validCustomer.Document);

            customer.Should().NotBeNull();
            customer.Name.Should().Be(validCustomer.Name);
            customer.Document.Should().Be(validCustomer.Document);
            customer.Id.Should().NotBeEmpty();
            customer.IsActive.Should().BeTrue();
        }

        [Theory(DisplayName = "ctor() should instantiate an customer with IsActive status")]
        [Trait("Domain", "Entities / Customer")]
        [InlineData(true)]
        [InlineData(false)]
        public void InstantiateWithIsActive(bool isActive)
        {
            var validCustomer = _fixture.GetValidCustomer();
            var customer = new DomainEntity.Customer(validCustomer.Name, validCustomer.Document, isActive: isActive);

            customer.Should().NotBeNull();
            customer.Name.Should().Be(validCustomer.Name);
            customer.Document.Should().Be(validCustomer.Document);
            customer.Id.Should().NotBeEmpty();
            customer.IsActive.Should().Be(isActive);
        }

        [Theory(DisplayName = "Ctor() should throw an error when Name is empty")]
        [Trait("Domain", "Entities / Customer")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void InstantiateErrorWhenNameIsEmpty(string? name)
        {
            var validCustomer = _fixture.GetValidCustomer();

            Action action = () => new DomainEntity.Customer(name!, validCustomer.Document);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("Name should not be empty or null");
        }

        [Theory(DisplayName = "Ctor() should throw an error when Document is invalid")]
        [Trait("Domain", "Entities / Customer")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("12.123.123/0002-12")]
        public void InstantiateErrorWhenDocumentIsInvalid(string? document)
        {
            var validCustomer = _fixture.GetValidCustomer();

            Action action =
                () => new DomainEntity.Customer(validCustomer.Name, document!);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("Document should be a valid CPF or CNPJ");
        }

        [Fact(DisplayName = "Activate() should activate a customer")]
        [Trait("Domain", "Entities / Customer")]
        public void Activate()
        {
            var validCustomer = _fixture.GetValidCustomer();

            var customer = new DomainEntity.Customer(validCustomer.Name, validCustomer.Document, isActive: false);
            customer.Activate();

            customer.IsActive.Should().BeTrue();
        }

        [Fact(DisplayName = "Deactivate() should deactivate a customer")]
        [Trait("Domain", "Entities / Customer")]
        public void Deactivate()
        {
            var validCustomer = _fixture.GetValidCustomer();

            var customer = new DomainEntity.Customer(validCustomer.Name, validCustomer.Document, isActive: true);
            customer.Deactivate();

            customer.IsActive.Should().BeFalse();
        }

        [Fact(DisplayName = "ChangeAddress() should change address")]
        [Trait("Domain", "Entities / Customer")]
        public void ChangeAddress()
        {
            var customer = _fixture.GetValidCustomer();
            var address = _fixture.GetValidAddress();

            customer.ChangeAddress(address);

            customer.Address.Should().NotBeNull();
            customer.Address!.StreetName.Should().Be(address.StreetName);
            customer.Address.StreetNumber.Should().Be(address.StreetNumber);
            customer.Address.Complement.Should().Be(address.Complement);
            customer.Address.Neighborhood.Should().Be(address.Neighborhood);
            customer.Address.ZipCode.Should().Be(address.ZipCode);
            customer.Address.City.Should().Be(address.City);
            customer.Address.State.Should().Be(address.State);
        }

        [Fact(DisplayName = "Update() should update a customer")]
        [Trait("Domain", "Entities / Customer")]
        public void Update()
        {
            var customer = _fixture.GetValidCustomer();
            var customerWithNewValues = _fixture.GetValidCustomer();

            customer.Update(customerWithNewValues.Name, customerWithNewValues.Document);

            customer.Name.Should().Be(customerWithNewValues.Name);
            customer.Document.Should().Be(customerWithNewValues.Document);
        }

        [Fact(DisplayName = "Update() should update only a name of a customer")]
        [Trait("Domain", "Entities / Customer")]
        public void UpdateOnlyName()
        {
            var customer = _fixture.GetValidCustomer();
            var newName = _fixture.GetValidCustomerName();
            var currentDocument = customer.Document;

            customer.Update(newName, null);

            customer.Name.Should().Be(newName);
            customer.Document.Should().Be(currentDocument);
        }

        [Theory(DisplayName = "Update() should throw an error when name is empty")]
        [Trait("Domain", "Entities / Customer")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void UpdateErrorWhenNameIsEmpty(string? name)
        {
            var customer = _fixture.GetValidCustomer();
            Action action =
                () => customer.Update(name!, null);

            action.Should().Throw<EntityValidationException>()
                .WithMessage("Name should not be empty or null");
        }

        [Fact(DisplayName = "SetAuditLog() should set auditLog")]
        [Trait("Domain", "Entities / Customer")]
        public void AuditLog()
        {
            var customer = _fixture.GetValidCustomer();

            var createdAt = DateTime.Now;
            var createdBy = "unit.testing";
            var lastUpdatedAt = DateTime.Now;
            var lastUpdatedBy = "unit.testing";

            customer.SetAuditLog(createdAt, createdBy, lastUpdatedAt, lastUpdatedBy);

            customer.CreatedAt.Should().Be(createdAt);
            customer.CreatedBy.Should().Be(createdBy);
            customer.LastUpdatedAt.Should().Be(lastUpdatedAt);
            customer.LastUpdatedBy.Should().Be(lastUpdatedBy);
        }
    }
}
