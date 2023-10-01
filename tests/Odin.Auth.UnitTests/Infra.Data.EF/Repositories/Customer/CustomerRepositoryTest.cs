using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Odin.Auth.Application.Customers.GetCustomers;
using Odin.Auth.Domain.Exceptions;
using Odin.Auth.Infra.Data.EF.Mappers;
using Odin.Auth.Infra.Data.EF.Repositories;

namespace Odin.Auth.UnitTests.Infra.Data.EF.Repositories.Customer
{
    [Collection(nameof(CustomerRepositoryTestFixtureCollection))]
    public class CustomerRepositoryTest
    {
        private readonly CustomerRepositoryTestFixture _fixture;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;

        public CustomerRepositoryTest(CustomerRepositoryTestFixture fixture)
        {
            _fixture = fixture;

            _httpContextAccessorMock = new();
            _httpContextAccessorMock.Setup(s => s.HttpContext!.User.Identity!.Name).Returns("unit.testing");
        }

        [Fact(DisplayName = "InsertAsync() should insert a valid customer")]
        [Trait("Infra.Data.EF", "Repositories / CustomerRepository")]
        public async Task InsertValidCustomer()
        {
            var dbContext = _fixture.CreateDbContext();
            var exampleCustomer = _fixture.GetValidCustomer();

            var repository = new CustomerRepository(dbContext, _httpContextAccessorMock.Object);
            var unitOfWork = new UnitOfWork(dbContext);

            await repository.InsertAsync(exampleCustomer, CancellationToken.None);
            await unitOfWork.CommitAsync(CancellationToken.None);

            var customerInserted = await repository.FindByIdAsync(exampleCustomer.Id, CancellationToken.None);

            customerInserted.Should().NotBeNull();
            customerInserted.Id.Should().Be(exampleCustomer.Id);
            customerInserted.Name.Should().Be(exampleCustomer.Name);
            customerInserted.Document.Should().Be(exampleCustomer.Document);
            customerInserted.IsActive.Should().Be(exampleCustomer.IsActive);
        }



        [Fact(DisplayName = "FindByIdAsync() should get a customer by a valid Id")]
        [Trait("Infra.Data.EF", "Repositories / CustomerRepository")]
        public async Task Get()
        {
            var dbContext = _fixture.CreateDbContext();

            var exampleCustomer = _fixture.GetValidCustomerModel();
            var exampleCustomersList = _fixture.GetValidCustomersModelList(15);
            exampleCustomersList.Add(exampleCustomer);

            await dbContext.AddRangeAsync(exampleCustomersList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var customerRepository = new CustomerRepository(_fixture.CreateDbContext(true), _httpContextAccessorMock.Object);

            var dbCustomer = await customerRepository.FindByIdAsync(exampleCustomer.Id, CancellationToken.None);

            dbCustomer.Should().NotBeNull();
            dbCustomer!.Name.Should().Be(exampleCustomer.Name);
            dbCustomer.Id.Should().Be(exampleCustomer.Id);
            dbCustomer.Document.Should().Be(exampleCustomer.Document);
            dbCustomer.IsActive.Should().Be(exampleCustomer.IsActive);
            dbCustomer.CreatedAt.Should().Be(exampleCustomer.CreatedAt);
        }

        [Fact(DisplayName = "FindByIdAsync() should throw an error on FindByIdAsync when customer not found")]
        [Trait("Infra.Data.EF", "Repositories / CustomerRepository")]
        public async Task GetThrowIfNotFound()
        {
            var dbContext = _fixture.CreateDbContext();
            var exampleId = Guid.NewGuid();

            await dbContext.AddRangeAsync(_fixture.GetValidCustomersModelList(15));
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var customerRepository = new CustomerRepository(dbContext, _httpContextAccessorMock.Object);

            var task = async () => await customerRepository.FindByIdAsync(exampleId, CancellationToken.None);

            await task.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Customer with Id '{exampleId}' not found.");
        }

        [Fact(DisplayName = "UpdateAsync() should update a customer")]
        [Trait("Infra.Data.EF", "Repositories / CustomerRepository")]
        public async Task Update()
        {
            var dbContext = _fixture.CreateDbContext();
            var exampleCustomer = _fixture.GetValidCustomer();
            var newCustomerValues = _fixture.GetValidCustomer();

            var exampleCustomersList = _fixture.GetValidCustomersModelList(15);
            exampleCustomersList.Add(exampleCustomer.ToCustomerModel());

            await dbContext.AddRangeAsync(exampleCustomersList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            exampleCustomer.Update(newCustomerValues.Name, newCustomerValues.Document);

            dbContext = _fixture.CreateDbContext(true);
            var customerRepository = new CustomerRepository(dbContext, _httpContextAccessorMock.Object);
            var unitOfWork = new UnitOfWork(dbContext);

            await customerRepository.UpdateAsync(exampleCustomer, It.IsAny<CancellationToken>());
            await unitOfWork.CommitAsync(CancellationToken.None);

            var dbCustomer = await customerRepository.FindByIdAsync(exampleCustomer.Id, CancellationToken.None);

            dbCustomer.Should().NotBeNull();
            dbCustomer!.Name.Should().Be(newCustomerValues.Name);
            dbCustomer.Document.Should().Be(newCustomerValues.Document);
            dbCustomer.IsActive.Should().Be(newCustomerValues.IsActive);
        }

        [Fact(DisplayName = "DeleteAsync() should delete a customer")]
        [Trait("Infra.Data.EF", "Repositories / CustomerRepository")]
        public async Task Delete()
        {
            var dbContext = _fixture.CreateDbContext(true);
            var exampleCustomer = _fixture.GetValidCustomer();
            var exampleCustomersList = _fixture.GetValidCustomersModelList(15);
            exampleCustomersList.Add(exampleCustomer.ToCustomerModel());

            await dbContext.AddRangeAsync(exampleCustomersList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            dbContext = _fixture.CreateDbContext(true);
            var customerRepository = new CustomerRepository(dbContext, _httpContextAccessorMock.Object);
            var unitOfWork = new UnitOfWork(dbContext);

            await customerRepository.DeleteAsync(exampleCustomer);
            await unitOfWork.CommitAsync(CancellationToken.None);

            var action = async () => await customerRepository.FindByIdAsync(exampleCustomer.Id, CancellationToken.None);

            await action.Should().ThrowAsync<NotFoundException>().WithMessage($"Customer with Id '{exampleCustomer.Id}' not found.");
        }

        [Fact(DisplayName = "FindPaginatedListAsync() should get paginated list of customers with filtered data")]
        [Trait("Infra.Data.EF", "Repositories / CustomerRepository")]
        public async Task SearchRetursListAndTotalFiltered()
        {
            var dbContext = _fixture.CreateDbContext();
            var exampleCustomersList = _fixture.GetValidCustomersModelList(15);

            await dbContext.AddRangeAsync(exampleCustomersList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var searchInput = new GetCustomersInput(1, 20, name: "", document: "", isActive: true, sort: "name");
            var filters = new Dictionary<string, object?>
            {
                { "Name", searchInput.Name },
                { "Document", searchInput.Document },
                { "IsActive", searchInput.IsActive },
            };

            var customerRepository = new CustomerRepository(dbContext, _httpContextAccessorMock.Object);
            var output = await customerRepository.FindPaginatedListAsync(filters, searchInput.PageNumber, searchInput.PageSize, searchInput.Sort!, CancellationToken.None);

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.TotalItems.Should().Be(exampleCustomersList.Where(x => x.IsActive).Count());
            output.Items.Should().HaveCount(exampleCustomersList.Where(x => x.IsActive).Count());
            foreach (var outputItem in output.Items)
            {
                var exampleItem = exampleCustomersList.Find(
                    customer => customer.Id == outputItem.Id
                );
                exampleItem.Should().NotBeNull();
                outputItem.Name.Should().Be(exampleItem!.Name);
                outputItem.Document.Should().Be(exampleItem.Document);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
            }
        }

        [Fact(DisplayName = "FindPaginatedListAsync() should get paginated list of customers with no filtered data")]
        [Trait("Infra.Data.EF", "Repositories / CustomerRepository")]
        public async Task SearchRetursListAndTotal()
        {
            var dbContext = _fixture.CreateDbContext();
            var exampleCustomersList = _fixture.GetValidCustomersModelList(15);

            await dbContext.AddRangeAsync(exampleCustomersList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var searchInput = new GetCustomersInput(1, 20, name: "", document: "", isActive: null, sort: "name");
            var filters = new Dictionary<string, object?>
            {
                { "Name", searchInput.Name },
                { "Document", searchInput.Document },
                { "IsActive", searchInput.IsActive },
            };

            var customerRepository = new CustomerRepository(dbContext, _httpContextAccessorMock.Object);
            var output = await customerRepository.FindPaginatedListAsync(filters, searchInput.PageNumber, searchInput.PageSize, searchInput.Sort!, CancellationToken.None);

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.TotalItems.Should().Be(exampleCustomersList.Count());
            output.Items.Should().HaveCount(exampleCustomersList.Count());
            foreach (var outputItem in output.Items)
            {
                var exampleItem = exampleCustomersList.Find(
                    customer => customer.Id == outputItem.Id
                );
                exampleItem.Should().NotBeNull();
                outputItem.Name.Should().Be(exampleItem!.Name);
                outputItem.Document.Should().Be(exampleItem.Document);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
            }
        }

        [Fact(DisplayName = "FindPaginatedListAsync() should return a empty list when database is clean")]
        [Trait("Infra.Data.EF", "Repositories / CustomerRepository")]
        public async Task SearchRetursEmptyWhenPersistenceIsEmpty()
        {
            var dbContext = _fixture.CreateDbContext();
            var customerRepository = new CustomerRepository(dbContext, _httpContextAccessorMock.Object);
            var searchInput = new GetCustomersInput(1, 20, name: "", document: "", isActive: true, sort: "");
            var filters = new Dictionary<string, object?>
            {
                { "Name", searchInput.Name },
                { "Document", searchInput.Document },
                { "IsActive", searchInput.IsActive },
            };

            var output = await customerRepository.FindPaginatedListAsync(filters, searchInput.PageNumber, searchInput.PageSize, searchInput.Sort!, CancellationToken.None);

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.TotalItems.Should().Be(0);
            output.Items.Should().HaveCount(0);
        }

        [Fact(DisplayName = "FindByDocumentAsync() should get a customer by a valid document")]
        [Trait("Infra.Data.EF", "Repositories / CustomerRepository")]
        public async Task GetByDocument()
        {
            var dbContext = _fixture.CreateDbContext();

            var exampleCustomer = _fixture.GetValidCustomerModel();
            var exampleCustomersList = _fixture.GetValidCustomersModelList(15);
            exampleCustomersList.Add(exampleCustomer);

            await dbContext.AddRangeAsync(exampleCustomersList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var customerRepository = new CustomerRepository(_fixture.CreateDbContext(true), _httpContextAccessorMock.Object);

            var dbCustomer = await customerRepository.FindByDocumentAsync(exampleCustomer.Document, CancellationToken.None);

            dbCustomer.Should().NotBeNull();
            dbCustomer!.Name.Should().Be(exampleCustomer.Name);
            dbCustomer.Id.Should().Be(exampleCustomer.Id);
            dbCustomer.Document.Should().Be(exampleCustomer.Document);
            dbCustomer.IsActive.Should().Be(exampleCustomer.IsActive);
            dbCustomer.CreatedAt.Should().Be(exampleCustomer.CreatedAt);
        }

        [Fact(DisplayName = "FindByDocumentAsync() should throw an error when customer not found")]
        [Trait("Infra.Data.EF", "Repositories / CustomerRepository")]
        public async Task GetThrowIfDocumentNotFound()
        {
            var dbContext = _fixture.CreateDbContext();
            var document = _fixture.GetValidCustomerDocument();

            await dbContext.AddRangeAsync(_fixture.GetValidCustomersModelList(15));
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var customerRepository = new CustomerRepository(dbContext, _httpContextAccessorMock.Object);

            var task = async () => await customerRepository.FindByDocumentAsync(document, CancellationToken.None);

            await task.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Customer with Document '{document}' not found.");
        }
    }
}

