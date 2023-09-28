using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odin.Auth.Application.Customers;
using Odin.Auth.Application.Customers.ChangeAddressCustomer;
using System.Net;
using System.Text.Json;

namespace Odin.Auth.EndToEndTests.Controllers.Customers.ChangeAddressCustomer
{

    [Collection(nameof(ChangeAddressCustomerApiTestCollection))]
    public class ChangeAddressCustomerApiTest
    {
        private readonly ChangeAddressCustomerApiTestFixture _fixture;

        public ChangeAddressCustomerApiTest(ChangeAddressCustomerApiTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Should change a customer address")]
        [Trait("E2E/Controllers", "Customers / [v1]ChangeAddressCustomer")]
        public async Task ActivateCustomer()
        {
            var customersList = _fixture.GetValidCustomersModelList(20);

            var dbContext = await _fixture.CreateDbContextAsync(preserveData: true);
            await dbContext.AddRangeAsync(customersList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var customerToChangeAddress = customersList.Where(x => x.IsActive).FirstOrDefault()!;

            var input = _fixture.GetValidInput(customerToChangeAddress.Id);

            var (response, output) = await _fixture.ApiClient.PutAsync<CustomerOutput>($"/v1/customers/{customerToChangeAddress.Id}/addresses", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            output.Should().NotBeNull();
            output.Name.Should().Be(customerToChangeAddress.Name);
            output.Document.Should().Be(customerToChangeAddress.Document);
            output.IsActive.Should().BeTrue();
            output.LastUpdatedAt.Should().NotBeSameDateAs(default);

            output.Address.Should().NotBeNull();
            output.Address!.StreetName.Should().Be(input.StreetName);
            output.Address.StreetNumber.Should().Be(input.StreetNumber);
            output.Address.Complement.Should().Be(input.Complement);
            output.Address.Neighborhood.Should().Be(input.Neighborhood);
            output.Address.ZipCode.Should().Be(input.ZipCode);
            output.Address.City.Should().Be(input.City);
            output.Address.State.Should().Be(input.State);
        }

        [Fact(DisplayName = "Should throw an error when Id is empty")]
        [Trait("E2E/Controllers", "Customers / [v1]ChangeAddressCustomer")]
        public async Task ErrorWhenInvalidIds()
        {

            var input = _fixture.GetValidInput();
            var (response, output) = await _fixture.ApiClient.PutAsync<ProblemDetails>($"/v1/customers/{Guid.Empty}/addresses", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            output.Should().NotBeNull();
            output.Title.Should().Be("Bad request");
            output.Type.Should().Be("BadRequest");
            output.Status.Should().Be(StatusCodes.Status400BadRequest);
            output.Detail.Should().Be("Invalid request");
        }

        [Fact(DisplayName = "Should throw an error when customer not found")]
        [Trait("E2E/Controllers", "Customers / [v1]ChangeAddressCustomer")]
        public async Task ErrorWhenNotFound()
        {
            var customersList = _fixture.GetValidCustomersModelList(20);

            var dbContext = await _fixture.CreateDbContextAsync(preserveData: true);
            await dbContext.AddRangeAsync(customersList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var idToQuery = Guid.NewGuid();
            var input = _fixture.GetValidInput(idToQuery);

            var (response, output) = await _fixture.ApiClient.PutAsync<ProblemDetails>($"/v1/customers/{idToQuery}/addresses", input);

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
            output.Should().NotBeNull();
            output!.Status.Should().Be(StatusCodes.Status404NotFound);
            output.Type.Should().Be("NotFound");
            output.Title.Should().Be("Not found");
            output.Detail.Should().Be($"Customer with Id '{idToQuery}' not found.");
        }

        [Theory(DisplayName = "Should throw an error with invalid data")]
        [Trait("E2E/Controllers", "Customers / [v1]ChangeAddressCustomer")]
        [MemberData(
            nameof(ChangeAddressCustomerApiTestDataGenerator.GetInvalidInputs),
            MemberType = typeof(ChangeAddressCustomerApiTestDataGenerator)
        )]
        public async Task ErrorWhenCantInstantiateAddress(ChangeAddressCustomerInput input, string property, string expectedDetail)
        {
            var customersList = _fixture.GetValidCustomersModelList(20);

            var dbContext = await _fixture.CreateDbContextAsync(preserveData: true);
            await dbContext.AddRangeAsync(customersList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var idToQuery = customersList.Where(x => x.IsActive).Select(x => x.Id).FirstOrDefault();
            input.ChangeCustomerId(idToQuery);

            var (response, output) = await _fixture.ApiClient.PutAsync<ProblemDetails>($"/v1/customers/{idToQuery}/addresses", input);

            output.Should().NotBeNull();
            output.Title.Should().Be("Unprocessable entity");
            output.Status.Should().Be(StatusCodes.Status422UnprocessableEntity);

            output.Extensions["errors"].Should().NotBeNull();
            var errors = JsonSerializer.Deserialize<Dictionary<string, string[]>>(JsonSerializer.Serialize(output.Extensions["errors"]))!;
            errors.ContainsKey(property).Should().BeTrue();
            errors[property].First().Should().Be(expectedDetail);
        }
    }
}
