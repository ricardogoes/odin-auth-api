using ValueObject = Odin.Auth.Domain.ValueObjects;

namespace Odin.Auth.UnitTests.Domain.ValuesObjects.Address
{
    [CollectionDefinition(nameof(AddressTestFixtureCollection))]
    public class AddressTestFixtureCollection : ICollectionFixture<AddressTestFixture>
    { }

    public class AddressTestFixture : BaseFixture
    {
        public AddressTestFixture()
            : base() { }

        public string GetValidStreetName()
            => Faker.Address.StreetName();

        public int GetValidStreetNumber()
             => int.Parse(Faker.Address.BuildingNumber());

        public string GetValidComplement()
            => Faker.Address.SecondaryAddress();

        public string GetValidNeighborhood()
            => Faker.Address.CardinalDirection();

        public string GetValidZipCode()
            => Faker.Address.ZipCode();

        public string GetValidCity()
            => Faker.Address.City();

        public string GetValidState()
            => Faker.Address.StateAbbr();
    }
}
