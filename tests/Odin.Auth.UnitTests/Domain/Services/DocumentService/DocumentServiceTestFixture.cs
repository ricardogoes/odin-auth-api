using Bogus;
using Bogus.Extensions.Brazil;
using Moq;
using Odin.Auth.Domain.Interfaces;

namespace Odin.Auth.UnitTests.Domain.Services.DocumentService
{
    [CollectionDefinition(nameof(DocumentServiceTestFixtureCollection))]
    public class DocumentServiceTestFixtureCollection : ICollectionFixture<DocumentServiceTestFixture>
    { }

    public class DocumentServiceTestFixture : BaseFixture
    {
        public Mock<ICustomerRepository> GetCustomerRepository()
            => new();
    }
}
