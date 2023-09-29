using Bogus.Extensions.Brazil;
using Odin.Auth.Infra.Data.EF.Models;

namespace Odin.Auth.UnitTests.Infra.Data.EF.Mappers
{
    [CollectionDefinition(nameof(ModelMapperTestFixtureCollection))]
    public class ModelMapperTestFixtureCollection : ICollectionFixture<ModelMapperTestFixture>
    { }

    public class ModelMapperTestFixture : BaseFixture
    {
        public ModelMapperTestFixture()
            : base() { }

        public CustomerModel GetValidCustomerModel(Guid? id = null)
        {
            return new CustomerModel
            (
                id: id ?? Guid.NewGuid(),
                name: Faker.Company.CompanyName(1),
                document: Faker.Company.Cnpj(),
                isActive: true,
                streetName: Faker.Address.StreetName(),
                streetNumber: int.Parse(Faker.Address.BuildingNumber()),
                complement: Faker.Address.SecondaryAddress(),
                neighborhood: Faker.Address.CardinalDirection(),
                zipCode: Faker.Address.ZipCode(),
                city: Faker.Address.City(),
                state: Faker.Address.StateAbbr(),
                createdAt: DateTime.UtcNow,
                createdBy: "unit.testing",
                lastUpdatedAt: DateTime.UtcNow,
                lastUpdatedBy: "unit.testing"
            );
        }

        public CustomerModel GetValidCustomerModelWithoutAddress()
        {
            return new CustomerModel
            (
                id: Guid.NewGuid(),
                name: Faker.Company.CompanyName(1),
                document: Faker.Company.Cnpj(),
                isActive: true,
                createdAt: DateTime.UtcNow,
                createdBy: "unit.testing",
                lastUpdatedAt: DateTime.UtcNow,
                lastUpdatedBy: "unit.testing"
            );
        }
    }
}
