using FluentAssertions;
using Odin.Auth.Domain.Entities;
using Odin.Auth.Infra.Data.EF.Mappers;
using Odin.Auth.Infra.Data.EF.Models;

namespace Odin.Auth.UnitTests.Infra.Data.EF.Mappers
{
    [Collection(nameof(ModelMapperTestFixtureCollection))]
    public class CustomerModelMapperTest
    {
        private readonly ModelMapperTestFixture _fixture;

        public CustomerModelMapperTest(ModelMapperTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "ToCustomerModel() should map an Customer to CustomerModel without address")]
        [Trait("Infra.Data.EF", "Mappers / CustomerModelMapper")]
        public void MapCustomerToCustomerModel()
        {
            var customer = _fixture.GetValidCustomer();
            var model = customer.ToCustomerModel();

            model.Should().NotBeNull();
            model.Id.Should().Be(customer.Id);
            model.Name.Should().Be(customer.Name);
            model.Document.Should().Be(customer.Document);
            model.IsActive.Should().Be(customer.IsActive);

            model.StreetName.Should().BeNull();
            model.StreetNumber.Should().BeNull();
            model.Complement.Should().BeNull();
            model.Neighborhood.Should().BeNull();
            model.ZipCode.Should().BeNull();
            model.City.Should().BeNull();
            model.State.Should().BeNull();
        }

        [Fact(DisplayName = "ToCustomerModel() should map an Customer to CustomerModel with address")]
        [Trait("Infra.Data.EF", "Mappers / CustomerModelMapper")]
        public void MapCustomerToCustomerModelWithAddress()
        {
            var customer = _fixture.GetValidCustomer();
            var address = _fixture.GetValidAddress();
            customer.ChangeAddress(address);

            var model = customer.ToCustomerModel();

            model.Should().NotBeNull();
            model.Id.Should().Be(customer.Id);
            model.Name.Should().Be(customer.Name);
            model.Document.Should().Be(customer.Document);
            model.IsActive.Should().Be(customer.IsActive);

            model.StreetName.Should().Be(customer.Address!.StreetName);
            model.StreetNumber.Should().Be(customer.Address.StreetNumber);
            model.Complement.Should().Be(customer.Address.Complement);
            model.Neighborhood.Should().Be(customer.Address.Neighborhood);
            model.ZipCode.Should().Be(customer.Address.ZipCode);
            model.City.Should().Be(customer.Address.City);
            model.State.Should().Be(customer.Address.State);
        }

        [Fact(DisplayName = "ToCustomerModel() should map a list of customers to CustomerModel with address")]
        [Trait("Infra.Data.EF", "Mappers / CustomerModelMapper")]
        public void MapListCustomersToCustomerModelWithAddress()
        {
            var address = _fixture.GetValidAddress();

            var customer1 = _fixture.GetValidCustomer();
            customer1.ChangeAddress(address);

            var customer2 = _fixture.GetValidCustomer();
            customer2.ChangeAddress(address);

            var customers = new List<Customer> { customer1, customer2 };

            var model = customers.ToCustomerModel();

            model.Should().NotBeNull();
            foreach (var customer in model)
            {
                var customerToCompare = customers.FirstOrDefault(x => x.Id == customer.Id);
                customer.Id.Should().Be(customerToCompare!.Id);
                customer.Name.Should().Be(customerToCompare.Name);
                customer.Document.Should().Be(customerToCompare.Document);
                customer.IsActive.Should().Be(customerToCompare.IsActive);

                customer.StreetName.Should().Be(customerToCompare.Address!.StreetName);
                customer.StreetNumber.Should().Be(customerToCompare.Address.StreetNumber);
                customer.Complement.Should().Be(customerToCompare.Address.Complement);
                customer.Neighborhood.Should().Be(customerToCompare.Address.Neighborhood);
                customer.ZipCode.Should().Be(customerToCompare.Address.ZipCode);
                customer.City.Should().Be(customerToCompare.Address.City);
                customer.State.Should().Be(customerToCompare.Address.State);
            }
        }

        [Fact(DisplayName = "ToCustomer() should map an CustomerModel to Customer with address")]
        [Trait("Infra.Data.EF", "Mappers / CustomerModelMapper")]
        public void MapCustomerModelToCustomerWithAddress()
        {
            var model = _fixture.GetValidCustomerModel();
            var customer = model.ToCustomer();

            customer.Should().NotBeNull();
            customer.Id.Should().Be(model.Id);
            customer.Name.Should().Be(model.Name);
            customer.Document.Should().Be(model.Document);
            customer.IsActive.Should().Be(model.IsActive);

            customer.Address.Should().NotBeNull();
            customer.Address!.StreetName.Should().Be(customer.Address!.StreetName);
            customer.Address.StreetNumber.Should().Be(customer.Address.StreetNumber);
            customer.Address.Complement.Should().Be(customer.Address.Complement);
            customer.Address.Neighborhood.Should().Be(customer.Address.Neighborhood);
            customer.Address.ZipCode.Should().Be(customer.Address.ZipCode);
            customer.Address.City.Should().Be(customer.Address.City);
            customer.Address.State.Should().Be(customer.Address.State);
        }

        [Fact(DisplayName = "ToCustomer() should map an CustomerModel to Customer without address")]
        [Trait("Infra.Data.EF", "Mappers / CustomerModelMapper")]
        public void MapCustomerModelToCustomerWithoutAddress()
        {
            var model = _fixture.GetValidCustomerModelWithoutAddress();
            var customer = model.ToCustomer();

            customer.Should().NotBeNull();
            customer.Id.Should().Be(model.Id);
            customer.Name.Should().Be(model.Name);
            customer.Document.Should().Be(model.Document);
            customer.IsActive.Should().Be(model.IsActive);

            customer.Address.Should().BeNull();
        }

        [Fact(DisplayName = "ToCustomer() should map a list of customers models to Customer with address")]
        [Trait("Infra.Data.EF", "Mappers / CustomerModelMapper")]
        public void MapListCustomersModelToCustomerWithAddress()
        {
            var customerModel1 = _fixture.GetValidCustomerModel();
            var customerModel2 = _fixture.GetValidCustomerModel();

            var customersModel = new List<CustomerModel> { customerModel1, customerModel2 };

            var customers = customersModel.ToCustomer();

            customers.Should().NotBeNull();
            foreach (var customer in customers)
            {
                var customerToCompare = customersModel.FirstOrDefault(x => x.Id == customer.Id);
                customer.Should().NotBeNull();
                customer.Id.Should().Be(customerToCompare!.Id);
                customer.Name.Should().Be(customerToCompare.Name);
                customer.Document.Should().Be(customerToCompare.Document);
                customer.IsActive.Should().Be(customerToCompare.IsActive);

                customer.Address.Should().NotBeNull();
                customer.Address!.StreetName.Should().Be(customerToCompare.StreetName);
                customer.Address.StreetNumber.Should().Be(customerToCompare.StreetNumber);
                customer.Address.Complement.Should().Be(customerToCompare.Complement);
                customer.Address.Neighborhood.Should().Be(customerToCompare.Neighborhood);
                customer.Address.ZipCode.Should().Be(customerToCompare.ZipCode);
                customer.Address.City.Should().Be(customerToCompare.City);
                customer.Address.State.Should().Be(customerToCompare.State);
            }
        }
    }
}
