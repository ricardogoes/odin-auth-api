using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odin.Auth.Application.Customers;
using System.Net;

namespace Odin.Auth.EndToEndTests.Controllers.Customers.GetCustomerById
{

    [Collection(nameof(GetCustomerByIdApiTestCollection))]
    public class GetCustomerByIdApiTest
    {
        private readonly GetCustomerByIdApiTestFixture _fixture;

        public GetCustomerByIdApiTest(GetCustomerByIdApiTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Should get a customer by valid id")]
        [Trait("E2E/Controllers", "Customers / [v1]GetCustomerById")]
        public async Task GetCustomerById()
        {
            var customersList = _fixture.GetValidCustomersModelList(20);

            var dbContext = await _fixture.CreateDbContextAsync(preserveData: true);
            await dbContext.AddRangeAsync(customersList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var customerToQuery = customersList[10];

            var (response, output) = await _fixture.ApiClient.GetByIdAsync<CustomerOutput>($"/v1/customers/{customerToQuery.Id}");

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);

            output.Id.Should().Be(customerToQuery.Id);
            output.Name.Should().Be(customerToQuery.Name);
            output.Document.Should().Be(customerToQuery.Document);
            output.IsActive.Should().Be(customerToQuery.IsActive);
            output.CreatedAt.Should().Be(customerToQuery.CreatedAt);
        }

        [Fact(DisplayName = "Should throw an error when customer not found")]
        [Trait("E2E/Controllers", "Customers / [v1]GetCustomerById")]
        public async Task ErrorWhenNotFound()
        {
            var customersList = _fixture.GetValidCustomersModelList(20);

            var dbContext = await _fixture.CreateDbContextAsync(preserveData: true);
            await dbContext.AddRangeAsync(customersList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var idToQuery = Guid.NewGuid();

            var (response, output) = await _fixture.ApiClient.GetByIdAsync<ProblemDetails>($"/v1/customers/{idToQuery}");

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
            output.Should().NotBeNull();
            output!.Status.Should().Be(StatusCodes.Status404NotFound);
            output.Type.Should().Be("NotFound");
            output.Title.Should().Be("Not found");
            output.Detail.Should().Be($"Customer with Id '{idToQuery}' not found.");
        }
    }
}
