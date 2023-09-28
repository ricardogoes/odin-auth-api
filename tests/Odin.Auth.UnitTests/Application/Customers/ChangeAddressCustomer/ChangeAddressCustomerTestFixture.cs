using Odin.Auth.Application.Customers.ChangeAddressCustomer;

namespace Odin.Auth.UnitTests.Application.Customers.ChangeAddressCustomer
{
    [CollectionDefinition(nameof(ChangeAddressCustomerTestFixtureCollection))]
    public class ChangeAddressCustomerTestFixtureCollection : ICollectionFixture<ChangeAddressCustomerTestFixture>
    { }

    public class ChangeAddressCustomerTestFixture : CustomerBaseFixture
    {
        public ChangeAddressCustomerTestFixture()
            : base() { }

        public ChangeAddressCustomerInput GetValidInputAddress()
        {
            return new ChangeAddressCustomerInput
            (
                customerId: Guid.NewGuid(),
                streetName: Faker.Address.StreetName(),
                streetNumber: int.Parse(Faker.Address.BuildingNumber()),
                complement: Faker.Address.SecondaryAddress(),
                neighborhood: Faker.Address.CardinalDirection(),
                zipCode: Faker.Address.ZipCode(),
                city: Faker.Address.City(),
                state: Faker.Address.StateAbbr(),
                loggedUsername: "unit.testing"
            );
        }

        public ChangeAddressCustomerInput GetInputAddressWithCustomerIdEmpty()
        {
            return new ChangeAddressCustomerInput
            (
                customerId: Guid.Empty,
                streetName: Faker.Address.StreetName(),
                streetNumber: int.Parse(Faker.Address.BuildingNumber()),
                complement: Faker.Address.SecondaryAddress(),
                neighborhood: Faker.Address.CardinalDirection(),
                zipCode: Faker.Address.ZipCode(),
                city: Faker.Address.City(),
                state: Faker.Address.StateAbbr(),
                loggedUsername: "unit.testing"
            );
        }

        public ChangeAddressCustomerInput GetInputAddressWithStreetNameEmpty()
        {
            return new ChangeAddressCustomerInput
            (
                customerId: Guid.NewGuid(),
                streetName: string.Empty,
                streetNumber: int.Parse(Faker.Address.BuildingNumber()),
                complement: Faker.Address.SecondaryAddress(),
                neighborhood: Faker.Address.CardinalDirection(),
                zipCode: Faker.Address.ZipCode(),
                city: Faker.Address.City(),
                state: Faker.Address.StateAbbr(),
                loggedUsername: "unit.testing"
            );
        }

        public ChangeAddressCustomerInput GetInputAddressWithStreetNumberEmpty()
        {
            return new ChangeAddressCustomerInput
            (
                customerId: Guid.NewGuid(),
                streetName: Faker.Address.StreetName(),
                streetNumber: 0,
                complement: Faker.Address.SecondaryAddress(),
                neighborhood: Faker.Address.CardinalDirection(),
                zipCode: Faker.Address.ZipCode(),
                city: Faker.Address.City(),
                state: Faker.Address.StateAbbr(),
                loggedUsername: "unit.testing"
            );
        }

        public ChangeAddressCustomerInput GetInputAddressWithNeighborhoodEmpty()
        {
            return new ChangeAddressCustomerInput
            (
                customerId: Guid.NewGuid(),
                streetName: Faker.Address.StreetName(),
                streetNumber: int.Parse(Faker.Address.BuildingNumber()),
                complement: Faker.Address.SecondaryAddress(),
                neighborhood: string.Empty,
                zipCode: Faker.Address.ZipCode(),
                city: Faker.Address.City(),
                state: Faker.Address.StateAbbr(),
                loggedUsername: "unit.testing"
            );
        }

        public ChangeAddressCustomerInput GetInputAddressWithZipCodeEmpty()
        {
            return new ChangeAddressCustomerInput
            (
                customerId: Guid.NewGuid(),
                streetName: Faker.Address.StreetName(),
                streetNumber: 196,
                complement: Faker.Address.SecondaryAddress(),
                neighborhood: Faker.Address.CardinalDirection(),
                zipCode: string.Empty,
                city: Faker.Address.City(),
                state: Faker.Address.StateAbbr(),
                loggedUsername: "unit.testing"
            );
        }

        public ChangeAddressCustomerInput GetInputAddressWithCityEmpty()
        {
            return new ChangeAddressCustomerInput
            (
                customerId: Guid.NewGuid(),
                streetName: Faker.Address.StreetName(),
                streetNumber: int.Parse(Faker.Address.BuildingNumber()),
                complement: Faker.Address.SecondaryAddress(),
                neighborhood: Faker.Address.CardinalDirection(),
                zipCode: Faker.Address.ZipCode(),
                city: string.Empty,
                state: Faker.Address.StateAbbr(),
                loggedUsername: "unit.testing"
            );
        }

        public ChangeAddressCustomerInput GetInputAddressWithStateEmpty()
        {
            return new ChangeAddressCustomerInput
            (
                customerId: Guid.NewGuid(),
                streetName: Faker.Address.StreetName(),
                streetNumber: int.Parse(Faker.Address.BuildingNumber()),
                complement: Faker.Address.SecondaryAddress(),
                neighborhood: Faker.Address.CardinalDirection(),
                zipCode: Faker.Address.ZipCode(),
                city: Faker.Address.City(),
                state: string.Empty,
                loggedUsername: "unit.testing"
            );
        }
    }
}
