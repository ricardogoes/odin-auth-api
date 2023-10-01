using Odin.Auth.Application.Customers.ChangeAddressCustomer;

namespace Odin.Auth.EndToEndTests.Controllers.Customers.ChangeAddressCustomer
{
    [CollectionDefinition(nameof(ChangeAddressCustomerApiTestCollection))]
    public class ChangeAddressCustomerApiTestCollection : ICollectionFixture<ChangeAddressCustomerApiTestFixture>
    { }

    public class ChangeAddressCustomerApiTestFixture : CustomerBaseFixture
    {
        public ChangeAddressCustomerApiTestFixture()
            : base()
        { }

        public ChangeAddressCustomerInput GetValidInput(Guid? id = null)
            => new
            (
                customerId: id ?? Guid.NewGuid(),
                streetName: Faker.Address.StreetName(),
                streetNumber: int.Parse(Faker.Address.BuildingNumber()),
                complement: Faker.Address.SecondaryAddress(),
                neighborhood: Faker.Address.CardinalDirection(),
                zipCode: Faker.Address.ZipCode(),
                city: Faker.Address.City(),
                state: Faker.Address.StateAbbr()
            );

        public ChangeAddressCustomerInput GetAddressInputWithoutStreetName(Guid? id = null)
            => new
            (
                customerId: id ?? Guid.NewGuid(),
                streetName: "",
                streetNumber: int.Parse(Faker.Address.BuildingNumber()),
                complement: Faker.Address.SecondaryAddress(),
                neighborhood: Faker.Address.CardinalDirection(),
                zipCode: Faker.Address.ZipCode(),
                city: Faker.Address.City(),
                state: Faker.Address.StateAbbr()
            );

        public ChangeAddressCustomerInput GetAddressInputWithoutStreetNumber(Guid? id = null)
            => new
            (
                customerId: id ?? Guid.NewGuid(),
                streetName: Faker.Address.StreetName(),
                streetNumber: -1,
                complement: Faker.Address.SecondaryAddress(),
                neighborhood: Faker.Address.CardinalDirection(),
                zipCode: Faker.Address.ZipCode(),
                city: Faker.Address.City(),
                state: Faker.Address.StateAbbr()
            );

        public ChangeAddressCustomerInput GetAddressInputWithoutNeighborhood(Guid? id = null)
            => new
            (
                customerId: id ?? Guid.NewGuid(),
                streetName: Faker.Address.StreetName(),
                streetNumber: int.Parse(Faker.Address.BuildingNumber()),
                complement: Faker.Address.SecondaryAddress(),
                neighborhood: "",
                zipCode: Faker.Address.ZipCode(),
                city: Faker.Address.City(),
                state: Faker.Address.StateAbbr()
            );

        public ChangeAddressCustomerInput GetAddressInputWithoutZipCode(Guid? id = null)
            => new
            (
                customerId: id ?? Guid.NewGuid(),
                streetName: Faker.Address.StreetName(),
                streetNumber: int.Parse(Faker.Address.BuildingNumber()),
                complement: Faker.Address.SecondaryAddress(),
                neighborhood: Faker.Address.CardinalDirection(),
                zipCode: "",
                city: Faker.Address.City(),
                state: Faker.Address.StateAbbr()
            );

        public ChangeAddressCustomerInput GetAddressInputWithoutCity(Guid? id = null)
            => new
            (
                customerId: id ?? Guid.NewGuid(),
                streetName: Faker.Address.StreetName(),
                streetNumber: int.Parse(Faker.Address.BuildingNumber()),
                complement: Faker.Address.SecondaryAddress(),
                neighborhood: Faker.Address.CardinalDirection(),
                zipCode: Faker.Address.ZipCode(),
                city: "",
                state: Faker.Address.StateAbbr()
            );

        public ChangeAddressCustomerInput GetAddressInputWithoutState(Guid? id = null)
            => new
            (
                customerId: id ?? Guid.NewGuid(),
                streetName: Faker.Address.StreetName(),
                streetNumber: int.Parse(Faker.Address.BuildingNumber()),
                complement: Faker.Address.SecondaryAddress(),
                neighborhood: Faker.Address.CardinalDirection(),
                zipCode: Faker.Address.ZipCode(),
                city: Faker.Address.City(),
                state: ""
            );
    }
}
