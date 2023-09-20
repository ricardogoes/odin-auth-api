using Bogus;

namespace Odin.Auth.UnitTests.Domain.Validations
{

    [CollectionDefinition(nameof(DomainValidationTestFixtureCollection))]
    public class DomainValidationTestFixtureCollection : ICollectionFixture<DomainValidationTestFixture>
    { }

    public class DomainValidationTestFixture : BaseFixture
    {
        public DomainValidationTestFixture()
            : base() { }

        public string GetValidFieldName()
            => Faker.Commerce.ProductName().Replace(" ", "");

        public string GetValidValue()
           => Faker.Commerce.ProductName();

    }
}
