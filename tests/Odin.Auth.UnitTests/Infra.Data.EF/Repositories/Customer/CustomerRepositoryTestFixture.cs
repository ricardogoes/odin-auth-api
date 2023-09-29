using Odin.Auth.Infra.Data.EF.Models;
using DomainEntity = Odin.Auth.Domain.Entities;

namespace Odin.Auth.UnitTests.Infra.Data.EF.Repositories.Customer
{
    [CollectionDefinition(nameof(CustomerRepositoryTestFixtureCollection))]
    public class CustomerRepositoryTestFixtureCollection : ICollectionFixture<CustomerRepositoryTestFixture>
    { }

    public class CustomerRepositoryTestFixture : BaseFixture
    {
        public CustomerRepositoryTestFixture()
            : base()
        { }

        public List<CustomerModel> GetValidCustomersModelList(int length = 10)
            => Enumerable.Range(1, length)
                .Select(_ => GetValidCustomerModel()).ToList();
    }
}
