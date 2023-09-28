using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odin.Auth.Application.Customers;
using System.Net;

namespace Odin.Auth.EndToEndTests.Controllers.Customers.ChangeStatusCustomer
{

    [Collection(nameof(ChangeStatusCustomerApiTestCollection))]
    public class ChangeStatusCustomerApiTest
    {
        private readonly ChangeStatusCustomerApiTestFixture _fixture;

        public ChangeStatusCustomerApiTest(ChangeStatusCustomerApiTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Should activate a customer")]
        [Trait("E2E/Controllers", "Customers / [v1]ChangeStatusCustomer")]
        public async Task ActivateCustomer()
        {
            var customersList = _fixture.GetValidCustomersModelList(20);

            var dbContext = await _fixture.CreateDbContextAsync(preserveData: true);
            await dbContext.AddRangeAsync(customersList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var customerToChangeStatus = customersList.FirstOrDefault()!;
            customerToChangeStatus.ChangeIsActive(false);

            var input = _fixture.GetValidInputToActivate(customerToChangeStatus.Id);


            var (response, output) = await _fixture.ApiClient.PutAsync<CustomerOutput>($"/v1/customers/{customerToChangeStatus.Id}/status?action=ACTIVATE", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            output.Should().NotBeNull();
            output.Name.Should().Be(customerToChangeStatus.Name);
            output.Document.Should().Be(customerToChangeStatus.Document);
            output.IsActive.Should().BeTrue();
            output.LastUpdatedAt.Should().NotBeSameDateAs(default);
        }

        [Fact(DisplayName = "Should deactivate a customer")]
        [Trait("E2E/Controllers", "Customers / [v1]ChangeStatusCustomer")]
        public async Task DeactivateCustomer()
        {
            var customersList = _fixture.GetValidCustomersModelList(20);

            var dbContext = await _fixture.CreateDbContextAsync(preserveData: true);
            await dbContext.AddRangeAsync(customersList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var customerToChangeStatus = customersList.Where(x => x.IsActive).FirstOrDefault()!;

            var input = _fixture.GetValidInputToDeactivate(customerToChangeStatus.Id);

            var (response, output) = await _fixture.ApiClient.PutAsync<CustomerOutput>($"/v1/customers/{customerToChangeStatus.Id}/status?action=DEACTIVATE", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            output.Should().NotBeNull();
            output.Name.Should().Be(customerToChangeStatus.Name);
            output.Document.Should().Be(customerToChangeStatus.Document);
            output.IsActive.Should().BeFalse();
            output.LastUpdatedAt.Should().NotBeSameDateAs(default);
        }

        [Fact(DisplayName = "Should throw an error when Id is empty")]
        [Trait("E2E/Controllers", "Customers / [v1]ChangeStatusCustomer")]
        public async Task ErrorWhenInvalidIds()
        {
            var (response, output) = await _fixture.ApiClient.PutAsync<ProblemDetails>($"/v1/customers/{Guid.Empty}/status?action=ACTIVATE", null);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            output.Should().NotBeNull();
            output.Title.Should().Be("Bad request");
            output.Type.Should().Be("BadRequest");
            output.Status.Should().Be(StatusCodes.Status400BadRequest);
            output.Detail.Should().Be("Invalid request");
        }

        [Fact(DisplayName = "Should throw an error when action is invalid")]
        [Trait("E2E/Controllers", "Customers / [v1]ChangeStatusCustomer")]
        public async Task ErrorWhenInvalidAction()
        {

            var customerToChangeStatus = _fixture.GetValidCustomer();

            var input = _fixture.GetInputWithInvalidAction(customerToChangeStatus.Id);

            var (response, output) = await _fixture.ApiClient.PutAsync<ProblemDetails>($"/v1/customers/{Guid.NewGuid()}/status?action=INVALID", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            output.Should().NotBeNull();
            output.Title.Should().Be("Bad request");
            output.Type.Should().Be("BadRequest");
            output.Status.Should().Be(StatusCodes.Status400BadRequest);
            output.Detail.Should().Be("Invalid action. Only ACTIVATE or DEACTIVATE values are allowed");
        }


        [Fact(DisplayName = "Should throw an error when customer not found")]
        [Trait("E2E/Controllers", "Customers / [v1]ChangeStatusCustomer")]
        public async Task ErrorWhenNotFound()
        {
            var customersList = _fixture.GetValidCustomersModelList(20);

            var dbContext = await _fixture.CreateDbContextAsync(preserveData: true);
            await dbContext.AddRangeAsync(customersList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var idToQuery = Guid.NewGuid();
            var input = _fixture.GetValidInputToActivate(idToQuery);

            var (response, output) = await _fixture.ApiClient.PutAsync<ProblemDetails>($"/v1/customers/{idToQuery}/status?action=ACTIVATE", input);

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
