using Odin.Auth.Domain.Entities;
using Odin.Auth.Domain.ValueObjects;
using Odin.Auth.Infra.Data.EF.Models;

namespace Odin.Auth.EndToEndTests.Controllers.Customers
{
    public class CustomerBaseFixture : BaseFixture
    {
        public Address GetValidAddress()
        {
            var address = new Address(
                Faker.Address.StreetName(),
                int.Parse(Faker.Address.BuildingNumber()),
                Faker.Address.SecondaryAddress(),
                Faker.Address.CardinalDirection(),
                Faker.Address.ZipCode(),
                Faker.Address.City(),
                Faker.Address.StateAbbr()
            );
            return address;
        }
        public List<Customer> GetValidCustomersList(int length = 10)
            => Enumerable.Range(1, length)
                .Select(_ => GetValidCustomer()).ToList();

        public List<CustomerModel> GetValidCustomersModelList(int length = 10)
            => Enumerable.Range(1, length)
                .Select(_ => GetValidCustomerModel()).ToList();
    }
}
