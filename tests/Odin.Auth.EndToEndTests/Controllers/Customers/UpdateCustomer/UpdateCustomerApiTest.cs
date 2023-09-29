using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odin.Auth.Application.Customers;
using Odin.Auth.Application.Customers.UpdateCustomer;
using System.Net;
using System.Text.Json;

namespace Odin.Auth.EndToEndTests.Controllers.Customers.UpdateCustomer
{

    [Collection(nameof(UpdateCustomerApiTestCollection))]
    public class UpdateCustomerApiTest
    {
        private readonly UpdateCustomerApiTestFixture _fixture;

        public UpdateCustomerApiTest(UpdateCustomerApiTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Should update a valid customer")]
        [Trait("E2E/Controllers", "Customers / [v1]UpdateCustomer")]
        public async Task UpdateValidCustomer()
        {
            var customersList = _fixture.GetValidCustomersModelList(20);

            var dbContext = await _fixture.CreateDbContextAsync(preserveData: true);
            await dbContext.AddRangeAsync(customersList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var customerToUpdate = customersList[10];

            var input = _fixture.GetValidInput(customerToUpdate.Id);

            var (response, output) = await _fixture.ApiClient.PutAsync<CustomerOutput>($"/v1/customers/{customerToUpdate.Id}", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Document.Should().Be(input.Document);
            output.IsActive.Should().Be(customerToUpdate.IsActive);
            output.LastUpdatedAt.Should().NotBeSameDateAs(default);
        }

        [Fact(DisplayName = "Should throw an error when Id from route is different of Id from customer")]
        [Trait("E2E/Controllers", "Customers / [v1]UpdateCustomer")]
        public async Task ErrorWhenInvalidIds()
        {

            var customersList = _fixture.GetValidCustomersModelList(20);

            var dbContext = await _fixture.CreateDbContextAsync(preserveData: true);
            await dbContext.AddRangeAsync(customersList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var customerToUpdate = customersList[10];

            var input = _fixture.GetValidInput(customerToUpdate.Id);

            var (response, output) = await _fixture.ApiClient.PutAsync<ProblemDetails>($"/v1/customers/{Guid.NewGuid()}", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            output.Should().NotBeNull();
            output.Title.Should().Be("Bad request");
            output.Type.Should().Be("BadRequest");
            output.Status.Should().Be(StatusCodes.Status400BadRequest);
            output.Detail.Should().Be("Invalid request");
        }

        [Theory(DisplayName = "Should throw an error with invalid data")]
        [Trait("E2E/Controllers", "Customers / [v1]UpdateCustomer")]
        [MemberData(
            nameof(UpdateCustomerApiTestDataGenerator.GetInvalidInputs),
            MemberType = typeof(UpdateCustomerApiTestDataGenerator)
        )]
        public async Task ErrorWhenCantInstantiateCustomer(UpdateCustomerInput input, string property, string expectedDetail)
        {
            var customersList = _fixture.GetValidCustomersModelList(20);

            var dbContext = await _fixture.CreateDbContextAsync(preserveData: true);
            await dbContext.AddRangeAsync(customersList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var customerToUpdate = customersList[10];
            input.ChangeId(customerToUpdate.Id);

            var (response, output) = await _fixture.ApiClient.PutAsync<ProblemDetails>($"/v1/customers/{input.Id}", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

            output.Should().NotBeNull();
            output.Title.Should().Be("Unprocessable entity");
            output.Status.Should().Be(StatusCodes.Status422UnprocessableEntity);

            output.Extensions["errors"].Should().NotBeNull();
            var errors = JsonSerializer.Deserialize<Dictionary<string, string[]>>(JsonSerializer.Serialize(output.Extensions["errors"]))!;
            errors.ContainsKey(property).Should().BeTrue();
            errors[property].First().Should().Be(expectedDetail);
        }

        [Fact(DisplayName = "Should throw an error when customer not found")]
        [Trait("E2E/Controllers", "Customers / [v1]UpdateCustomer")]
        public async Task ErrorWhenNotFound()
        {
            var customersList = _fixture.GetValidCustomersModelList(20);

            var dbContext = await _fixture.CreateDbContextAsync(preserveData: true);
            await dbContext.AddRangeAsync(customersList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var idToQuery = Guid.NewGuid();
            var input = _fixture.GetValidInput(idToQuery);

            var (response, output) = await _fixture.ApiClient.PutAsync<ProblemDetails>($"/v1/customers/{idToQuery}", input);

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
            output.Should().NotBeNull();
            output!.Status.Should().Be(StatusCodes.Status404NotFound);
            output.Type.Should().Be("NotFound");
            output.Title.Should().Be("Not found");
            output.Detail.Should().Be($"Customer with Id '{idToQuery}' not found.");
        }

        [Fact(DisplayName = "Should throw an error when document is duplicated")]
        [Trait("E2E/Controllers", "Customers / [v1]UpdateCustomer")]
        public async Task ThrowErrorWithDuplicatedDocument()
        {
            var customersList = _fixture.GetValidCustomersModelList(20);

            var dbContext = await _fixture.CreateDbContextAsync(preserveData: true);
            await dbContext.AddRangeAsync(customersList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var customerToUpdate = customersList[10];
            var input = _fixture.GetValidInput(customerToUpdate.Id);
            input.ChangeDocument(customersList[2].Document);

            var (response, output) = await _fixture.ApiClient.PutAsync<ProblemDetails>($"/v1/customers/{input.Id}", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
            output.Should().NotBeNull();
            output.Title.Should().Be("Unprocessable entity");
            output.Type.Should().Be("UnprocessableEntity");
            output.Status.Should().Be(StatusCodes.Status422UnprocessableEntity);
            output.Detail.Should().Be("Document must be unique");
        }
    }
}
