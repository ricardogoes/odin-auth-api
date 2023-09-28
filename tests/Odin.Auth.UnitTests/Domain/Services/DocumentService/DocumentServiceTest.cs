using FluentAssertions;
using Moq;
using Odin.Auth.Domain.Exceptions;
using Odin.Auth.Domain.Interfaces;
using Service = Odin.Auth.Domain.Services;

namespace Odin.Auth.UnitTests.Domain.Services.DocumentService
{
    [Collection(nameof(DocumentServiceTestFixtureCollection))]
    public class DocumentServiceTest
    {
        private readonly DocumentServiceTestFixture _fixture;

        private readonly Mock<ICustomerRepository> _customerRepositoryMock;

        public DocumentServiceTest(DocumentServiceTestFixture fixture)
        {
            _fixture = fixture;

            _customerRepositoryMock = _fixture.GetCustomerRepository();
        }

        [Fact(DisplayName = "IsDocumentUnique() should return true when customer document does not exist")]
        [Trait("Domain", "Services / DocumentService")]
        public async Task IsCustomerDocumentUnique()
        {
            var customer = _fixture.GetValidCustomer();
            
            _customerRepositoryMock.Setup(s => s.FindByDocumentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Throws(new NotFoundException());

            var documentService = new Service.DocumentService(_customerRepositoryMock.Object);
            var output = await documentService.IsDocumentUnique(customer, CancellationToken.None);

            output.Should().BeTrue();

            _customerRepositoryMock.Verify(x => x.FindByDocumentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "IsDocumentUnique() should return true when customer exist but with same id")]
        [Trait("Domain", "Services / DocumentService")]
        public async Task IsCustomerDocumentUniqueWithSameIds()
        {
            var customer = _fixture.GetValidCustomer();
            
            _customerRepositoryMock.Setup(s => s.FindByDocumentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(customer));

            var documentService = new Service.DocumentService(_customerRepositoryMock.Object);
            var output = await documentService.IsDocumentUnique(customer, CancellationToken.None);

            output.Should().BeTrue();

            _customerRepositoryMock.Verify(x => x.FindByDocumentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "IsDocumentUnique() should return false when customer already exists")]
        [Trait("Domain", "Services / DocumentService")]
        public async Task IsCustomerDocumentUniqueFalse()
        {
            var customer = _fixture.GetValidCustomer();
            var newCustomer = _fixture.GetValidCustomer();

            _customerRepositoryMock.Setup(s => s.FindByDocumentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(customer));

            var documentService = new Service.DocumentService(_customerRepositoryMock.Object);
            var output = await documentService.IsDocumentUnique(newCustomer, CancellationToken.None);

            output.Should().BeFalse();

            _customerRepositoryMock.Verify(x => x.FindByDocumentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
