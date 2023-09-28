using FluentAssertions;
using Moq;
using Odin.Auth.Domain.Entities;
using Odin.Auth.Domain.Interfaces;
using Odin.Auth.Domain.Models;
using App = Odin.Auth.Application.Customers.GetCustomers;

namespace Odin.Auth.UnitTests.Application.Customers.GetCustomers
{
    [Collection(nameof(GetCustomerByIdTestFixtureCollection))]
    public class GetCustomersTest
    {
        private readonly GetCustomersTestFixture _fixture;

        private readonly Mock<ICustomerRepository> _repositoryMock;

        public GetCustomersTest(GetCustomersTestFixture fixture)
        {
            _fixture = fixture;
            _repositoryMock = _fixture.GetCustomerRepositoryMock();
        }

        [Fact(DisplayName = "Handle() should return data filtered")]
        [Trait("Application", "Customers / GetCustomers")]
        public async Task GetCustomers()
        {
            var expectedCustomers = new PaginatedListOutput<Customer>
            (
                totalItems: 15,
                items: new List<Customer>
                {
                    new Customer(_fixture.GetValidCustomerName(), _fixture.GetValidCustomerDocument(), isActive: true),
                    new Customer(_fixture.GetValidCustomerName(), _fixture.GetValidCustomerDocument(), isActive: true),
                    new Customer(_fixture.GetValidCustomerName(), _fixture.GetValidCustomerDocument(), isActive: true),
                    new Customer(_fixture.GetValidCustomerName(), _fixture.GetValidCustomerDocument(), isActive: true),
                    new Customer(_fixture.GetValidCustomerName(), _fixture.GetValidCustomerDocument(), isActive: true),
                    new Customer(_fixture.GetValidCustomerName(), _fixture.GetValidCustomerDocument(), isActive: true),
                    new Customer(_fixture.GetValidCustomerName(), _fixture.GetValidCustomerDocument(), isActive: true),
                    new Customer(_fixture.GetValidCustomerName(), _fixture.GetValidCustomerDocument(), isActive: true),
                    new Customer(_fixture.GetValidCustomerName(), _fixture.GetValidCustomerDocument(), isActive: true),
                    new Customer(_fixture.GetValidCustomerName(), _fixture.GetValidCustomerDocument(), isActive: true),
                    new Customer(_fixture.GetValidCustomerName(), _fixture.GetValidCustomerDocument(), isActive: true),
                    new Customer(_fixture.GetValidCustomerName(), _fixture.GetValidCustomerDocument(), isActive: true),
                    new Customer(_fixture.GetValidCustomerName(), _fixture.GetValidCustomerDocument(), isActive: true),
                    new Customer(_fixture.GetValidCustomerName(), _fixture.GetValidCustomerDocument(), isActive: true),
                    new Customer(_fixture.GetValidCustomerName(), _fixture.GetValidCustomerDocument(), isActive: true),
                }
            );

            _repositoryMock.Setup(s => s.FindPaginatedListAsync(It.IsAny<Dictionary<string, object?>>(), 1, 10, "name", new CancellationToken()))
                .Returns(() => Task.FromResult(expectedCustomers));

            var useCase = new App.GetCustomers(_repositoryMock.Object);
            var customers = await useCase.Handle(new App.GetCustomersInput(1, 10, "name", "", "", true), new CancellationToken());

            customers.Should().NotBeNull();
            customers.TotalItems.Should().Be(15);
            customers.TotalPages.Should().Be(2);
            customers.PageNumber.Should().Be(1);
            customers.PageSize.Should().Be(10);
            customers.Items.Should().HaveCount(15);
        }

        [Fact(DisplayName = "Handle() should return 1 page when total items is less than page size")]
        [Trait("Application", "Customers / GetCustomers")]
        public async Task GetOnePageWhenTotalItemsLessPageSize()
        {
            var expectedCustomers = new PaginatedListOutput<Customer>
            (
                totalItems: 4,
                items: new List<Customer>
                {
                    new Customer(_fixture.GetValidCustomerName(), _fixture.GetValidCustomerDocument(), isActive: true),
                    new Customer(_fixture.GetValidCustomerName(), _fixture.GetValidCustomerDocument(), isActive: true),
                    new Customer(_fixture.GetValidCustomerName(), _fixture.GetValidCustomerDocument(), isActive: true),
                    new Customer(_fixture.GetValidCustomerName(), _fixture.GetValidCustomerDocument(), isActive: true),
                }
            );

            _repositoryMock.Setup(s => s.FindPaginatedListAsync(It.IsAny<Dictionary<string, object?>>(), 1, 10, "name", new CancellationToken()))
                .Returns(() => Task.FromResult(expectedCustomers));

            var useCase = new App.GetCustomers(_repositoryMock.Object);
            var customers = await useCase.Handle(new App.GetCustomersInput(1, 10, "name", "Unit Testing", "123.123.123-12", true), new CancellationToken());

            customers.Should().NotBeNull();
            customers.TotalItems.Should().Be(4);
            customers.TotalPages.Should().Be(1);
            customers.PageNumber.Should().Be(1);
            customers.PageSize.Should().Be(10);
            customers.Items.Should().HaveCount(4);
        }
    }
}
