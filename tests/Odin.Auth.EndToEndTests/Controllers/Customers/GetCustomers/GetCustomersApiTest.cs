using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Odin.Auth.Api.Models;
using Odin.Auth.Application.Customers;
using Odin.Auth.Application.Customers.GetCustomers;
using Odin.Auth.Infra.Data.EF.Models;
using System.Net;

namespace Odin.Auth.EndToEndTests.Controllers.Customers.GetCustomers
{

    [Collection(nameof(GetCustomersApiTestCollection))]
    public class GetCustomersApiTest
    {
        private readonly GetCustomersApiTestFixture _fixture;

        public GetCustomersApiTest(GetCustomersApiTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Should get customers with default data")]
        [Trait("E2E/Controllers", "Customers / [v1]GetCustomers")]
        public async Task GetCustomers()
        {
            var customersList = _fixture.GetValidCustomersModelList(20);

            var dbContext = await _fixture.CreateDbContextAsync(preserveData: false);
            await dbContext.AddRangeAsync(customersList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var (response, output) = await _fixture.ApiClient.GetAsync<PaginatedApiResponse<CustomerOutput>>("/v1/customers");

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);

            output.Should().NotBeNull();
            output.PageNumber.Should().Be(1);
            output.PageSize.Should().Be(10);
            output.TotalRecords.Should().Be(20);
            output.TotalPages.Should().Be(2);
            output.Items.Should().HaveCount(10);
        }

        [Fact(DisplayName = "Should return valid data")]
        [Trait("E2E/Controllers", "Customers / [v1]GetCustomers")]
        public async Task ListCategoriesAndTotal()
        {
            var customersList = _fixture.GetValidCustomersModelList(20);

            var dbContext = await _fixture.CreateDbContextAsync(preserveData: false);
            await dbContext.AddRangeAsync(customersList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var input = new GetCustomersInput(1, 5);

            var (response, output) = await _fixture.ApiClient.GetAsync<PaginatedApiResponse<CustomerOutput>>("/v1/customers", input);

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);

            output.Should().NotBeNull();
            output.PageNumber.Should().Be(input.PageNumber);
            output.PageSize.Should().Be(input.PageSize);
            output.TotalRecords.Should().Be(customersList.Count);
            output.Items.Should().HaveCount(input.PageSize);

            foreach (var outputItem in output.Items)
            {
                var customer = customersList.FirstOrDefault(x => x.Id == outputItem.Id);
                customer.Should().NotBeNull();
                outputItem.Name.Should().Be(customer!.Name);
                outputItem.Document.Should().Be(customer.Document);
                outputItem.IsActive.Should().Be(customer.IsActive);
            }
        }

        [Theory(DisplayName = "Should return paginated data")]
        [Trait("E2E/Controllers", "Customers / [v1]GetCustomers")]
        [InlineData(10, 1, 5, 5)]
        [InlineData(10, 2, 5, 5)]
        [InlineData(7, 2, 5, 2)]
        [InlineData(7, 3, 5, 0)]
        public async Task ListPaginated(int quantityToGenerate, int page, int pageSize, int expectedItems)
        {
            var customersList = _fixture.GetValidCustomersModelList(quantityToGenerate);

            var dbContext = await _fixture.CreateDbContextAsync(preserveData: false);
            await dbContext.AddRangeAsync(customersList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var input = new GetCustomersInput(page, pageSize);

            var (response, output) = await _fixture.ApiClient.GetAsync<PaginatedApiResponse<CustomerOutput>>("/v1/customers", input);

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();

            output.PageNumber.Should().Be(input.PageNumber);
            output.PageSize.Should().Be(input.PageSize);
            output.TotalRecords.Should().Be(quantityToGenerate);
            output.Items.Should().HaveCount(expectedItems);

            foreach (var outputItem in output.Items)
            {
                var customer = customersList.FirstOrDefault(x => x.Id == outputItem.Id);
                customer.Should().NotBeNull();
                outputItem.Name.Should().Be(customer!.Name);
                outputItem.Document.Should().Be(customer.Document);
                outputItem.IsActive.Should().Be(customer.IsActive);
            }
        }

        [Theory(DisplayName = "Should return filtered data by name")]
        [Trait("E2E/Controllers", "Customers / [v1]GetCustomers")]
        [InlineData("Customer", 1, 5, 5, 5)]
        [InlineData("Cliente", 1, 5, 3, 3)]
        [InlineData("Invalid", 1, 5, 0, 0)]
        public async Task SearchByName(string search, int page, int pageSize, int expectedQuantityItemsReturned, int expectedQuantityTotalItems)
        {
            var customersList = new List<CustomerModel>()
            {
                new CustomerModel(Guid.NewGuid(), "Customer 01", _fixture.GetValidDocument(), true,  DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing"),
                new CustomerModel(Guid.NewGuid(), "Customer 02", _fixture.GetValidDocument(), true,  DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing"),
                new CustomerModel(Guid.NewGuid(), "Customer 03", _fixture.GetValidDocument(), true,  DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing"),
                new CustomerModel(Guid.NewGuid(), "Customer 04", _fixture.GetValidDocument(), false, DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing"),
                new CustomerModel(Guid.NewGuid(), "Customer 05", _fixture.GetValidDocument(), false, DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing"),
                new CustomerModel(Guid.NewGuid(), "Cliente 01",  _fixture.GetValidDocument(), true,  DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing"),
                new CustomerModel(Guid.NewGuid(), "Cliente 02",  _fixture.GetValidDocument(), true,  DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing"),
                new CustomerModel(Guid.NewGuid(), "Cliente 03",  _fixture.GetValidDocument(), false, DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing")
            };

            var dbContext = await _fixture.CreateDbContextAsync(preserveData: false);
            await dbContext.AddRangeAsync(customersList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var input = new GetCustomersInput(page, pageSize, name: search);

            var (response, output) = await _fixture.ApiClient.GetAsync<PaginatedApiResponse<CustomerOutput>>("/v1/customers", input);

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();

            output.PageNumber.Should().Be(input.PageNumber);
            output.PageSize.Should().Be(input.PageSize);
            output.TotalRecords.Should().Be(expectedQuantityTotalItems);
            output.Items.Should().HaveCount(expectedQuantityItemsReturned);

            foreach (var outputItem in output.Items)
            {
                var customer = customersList.FirstOrDefault(x => x.Id == outputItem.Id);
                customer.Should().NotBeNull();
                outputItem.Name.Should().Be(customer!.Name);
                outputItem.Document.Should().Be(customer.Document);
                outputItem.IsActive.Should().Be(customer.IsActive);
            }
        }

        [Fact(DisplayName = "Should return filtered data by document")]
        [Trait("E2E/Controllers", "Customers / [v1]GetCustomers")]
        public async Task SearchByDocument()
        {
            var customersList = new List<CustomerModel>()
            {
                new CustomerModel(Guid.NewGuid(), "Customer 01", _fixture.GetValidDocument(), true,  DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing"),
                new CustomerModel(Guid.NewGuid(), "Customer 02", _fixture.GetValidDocument(), true,  DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing"),
                new CustomerModel(Guid.NewGuid(), "Customer 03", _fixture.GetValidDocument(), true,  DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing"),
                new CustomerModel(Guid.NewGuid(), "Customer 04", _fixture.GetValidDocument(), false, DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing"),
                new CustomerModel(Guid.NewGuid(), "Customer 05", _fixture.GetValidDocument(), false, DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing"),
                new CustomerModel(Guid.NewGuid(), "Cliente 01",  _fixture.GetValidDocument(), true,  DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing"),
                new CustomerModel(Guid.NewGuid(), "Cliente 02",  _fixture.GetValidDocument(), true,  DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing"),
                new CustomerModel(Guid.NewGuid(), "Cliente 03",  _fixture.GetValidDocument(), false, DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing")
            };

            var dbContext = await _fixture.CreateDbContextAsync(preserveData: false);
            await dbContext.AddRangeAsync(customersList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var input = new GetCustomersInput(1, 5, sort: null, name: null, document: customersList.First().Document, isActive: null);

            var (response, output) = await _fixture.ApiClient.GetAsync<PaginatedApiResponse<CustomerOutput>>("/v1/customers", input);

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();

            output.PageNumber.Should().Be(input.PageNumber);
            output.PageSize.Should().Be(input.PageSize);
            output.TotalRecords.Should().Be(1);
            output.Items.Should().HaveCount(1);

            foreach (var outputItem in output.Items)
            {
                var customer = customersList.FirstOrDefault(x => x.Id == outputItem.Id);
                customer.Should().NotBeNull();
                outputItem.Name.Should().Be(customer!.Name);
                outputItem.Document.Should().Be(customer.Document);
                outputItem.IsActive.Should().Be(customer.IsActive);
            }
        }

        [Theory(DisplayName = "Should return filtered data by status")]
        [Trait("E2E/Controllers", "Customers / [v1]GetCustomers")]
        [InlineData(true, 1, 5, 5, 5)]
        [InlineData(false, 1, 5, 3, 3)]
        public async Task SearchByStatus(bool search, int page, int pageSize, int expectedQuantityItemsReturned, int expectedQuantityTotalItems)
        {
            var customersList = new List<CustomerModel>()
            {
                new CustomerModel(Guid.NewGuid(), "Customer 01", _fixture.GetValidDocument(), true,  DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing"),
                new CustomerModel(Guid.NewGuid(), "Customer 02", _fixture.GetValidDocument(), true,  DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing"),
                new CustomerModel(Guid.NewGuid(), "Customer 03", _fixture.GetValidDocument(), true,  DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing"),
                new CustomerModel(Guid.NewGuid(), "Customer 04", _fixture.GetValidDocument(), false, DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing"),
                new CustomerModel(Guid.NewGuid(), "Customer 05", _fixture.GetValidDocument(), false, DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing"),
                new CustomerModel(Guid.NewGuid(), "Cliente 01",  _fixture.GetValidDocument(), true,  DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing"),
                new CustomerModel(Guid.NewGuid(), "Cliente 02",  _fixture.GetValidDocument(), true,  DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing"),
                new CustomerModel(Guid.NewGuid(), "Cliente 03",  _fixture.GetValidDocument(), false, DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing")
            };

            var dbContext = await _fixture.CreateDbContextAsync(preserveData: false);
            await dbContext.AddRangeAsync(customersList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var input = new GetCustomersInput(page, pageSize, isActive: search);

            var (response, output) = await _fixture.ApiClient.GetAsync<PaginatedApiResponse<CustomerOutput>>("/v1/customers", input);

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();

            output.PageNumber.Should().Be(input.PageNumber);
            output.PageSize.Should().Be(input.PageSize);
            output.TotalRecords.Should().Be(expectedQuantityTotalItems);
            output.Items.Should().HaveCount(expectedQuantityItemsReturned);

            foreach (var outputItem in output.Items)
            {
                var customer = customersList.FirstOrDefault(x => x.Id == outputItem.Id);
                customer.Should().NotBeNull();
                outputItem.Name.Should().Be(customer!.Name);
                outputItem.Document.Should().Be(customer.Document);
                outputItem.IsActive.Should().Be(customer.IsActive);
            }
        }

        [Theory(DisplayName = "Should return customers ordered by field")]
        [Trait("E2E/Controllers", "Customers / [v1]GetCustomers")]
        [InlineData("name")]
        [InlineData("name desc")]
        [InlineData("id")]
        [InlineData("id desc")]
        public async Task ListOrdered(string orderBy)
        {
            var customersList = new List<CustomerModel>()
            {
                new CustomerModel(Guid.NewGuid(), "Customer 01", _fixture.GetValidDocument(), true,  DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing"),
                new CustomerModel(Guid.NewGuid(), "Customer 02", _fixture.GetValidDocument(), true,  DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing"),
                new CustomerModel(Guid.NewGuid(), "Customer 03", _fixture.GetValidDocument(), true,  DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing"),
                new CustomerModel(Guid.NewGuid(), "Customer 04", _fixture.GetValidDocument(), false, DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing"),
                new CustomerModel(Guid.NewGuid(), "Customer 05", _fixture.GetValidDocument(), false, DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing"),
                new CustomerModel(Guid.NewGuid(), "Cliente 01",  _fixture.GetValidDocument(), true,  DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing"),
                new CustomerModel(Guid.NewGuid(), "Cliente 02",  _fixture.GetValidDocument(), true,  DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing"),
                new CustomerModel(Guid.NewGuid(), "Cliente 03",  _fixture.GetValidDocument(), false, DateTime.Now, "unit.Testing", DateTime.Now, "unit.testing")
            };

            var dbContext = await _fixture.CreateDbContextAsync(preserveData: false);
            await dbContext.AddRangeAsync(customersList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var input = new GetCustomersInput(1, 5, sort: orderBy);

            var (response, output) = await _fixture.ApiClient.GetAsync<PaginatedApiResponse<CustomerOutput>>("/v1/customers", input);

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();

            output.PageNumber.Should().Be(input.PageNumber);
            output.PageSize.Should().Be(input.PageSize);
            output.TotalRecords.Should().Be(customersList.Count);
            output.Items.Should().HaveCount(5);
        }
    }
}
