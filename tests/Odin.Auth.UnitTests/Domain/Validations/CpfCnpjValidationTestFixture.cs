using Bogus;

namespace Odin.Auth.UnitTests.Domain.Validations
{
    public class CpfCnpjValidationTestFixture : BaseFixture
    {
        public CpfCnpjValidationTestFixture()
            : base() { }

        public string GetValidCNPJ()
            => "38.656.085/0001-33";

        public string GetInvalidCNPJ()
            => "12.123.123/0002-12";

        public string GetValidCPF()
           => "345.388.868-58";

        public string GetInvalidCPF()
           => "123.123.123-21";
    }

    [CollectionDefinition(nameof(CpfCnpjValidationTestFixture))]
    public class CpfCnpjValidationTestFixtureCollection
        : ICollectionFixture<CpfCnpjValidationTestFixture>
    { }
}
