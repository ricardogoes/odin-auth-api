using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odin.Auth.Domain.Models;
using System.Net;

namespace Odin.Auth.EndToEndTests.Controllers.Users.ChangeStatusUser
{

    [Collection(nameof(ChangeStatusUserApiTestCollection))]
    public class ChangeStatusUserApiTest
    {
        private readonly ChangeStatusUserApiTestFixture _fixture;

        public ChangeStatusUserApiTest(ChangeStatusUserApiTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Should activate a user")]
        [Trait("E2E/Controllers", "Users / [v1]ChangeStatusUser")]
        public async Task ActivateUser()
        {
            var input = _fixture.GetValidInputToActivate(_fixture.CommonUserId);

            var (response, output) = await _fixture.ApiClient.PutAsync<UserOutput>($"/v1/users/{input.UserId}/status?action=ACTIVATE", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            output.Id.Should().Be(_fixture.CommonUserId);
            output.Username.Should().Be("common_user");
            output.FirstName.Should().Be("Common");
            output.LastName.Should().Be("User");
            output.Enabled.Should().BeTrue();
        }

        [Fact(DisplayName = "Should deactivate a user")]
        [Trait("E2E/Controllers", "Users / [v1]ChangeStatusUser")]
        public async Task DeactivateUser()
        {
            var input = _fixture.GetValidInputToDeactivate(_fixture.CommonUserId);

            try
            {
                var (response, output) = await _fixture.ApiClient.PutAsync<UserOutput>($"/v1/users/{input.UserId}/status?action=DEACTIVATE", input);

                response.Should().NotBeNull();
                response.StatusCode.Should().Be(HttpStatusCode.OK);

                output.Id.Should().Be(_fixture.CommonUserId);
                output.Username.Should().Be("common_user");
                output.FirstName.Should().Be("Common");
                output.LastName.Should().Be("User");
                output.Enabled.Should().BeFalse();
            }
            finally
            {
                await _fixture.ApiClient.PutAsync<UserOutput>($"/v1/users/{input.UserId}/status?action=ACTIVATE", input);
            }            
        }

        [Fact(DisplayName = "Should throw an error when Id is empty")]
        [Trait("E2E/Controllers", "Users / [v1]ChangeStatusUser")]
        public async Task ErrorWhenInvalidIds()
        {
            var (response, output) = await _fixture.ApiClient.PutAsync<ProblemDetails>($"/v1/users/{Guid.Empty}/status?action=ACTIVATE", null);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            output.Should().NotBeNull();
            output.Title.Should().Be("Bad request");
            output.Type.Should().Be("BadRequest");
            output.Status.Should().Be(StatusCodes.Status400BadRequest);
            output.Detail.Should().Be("Invalid request");
        }

        [Fact(DisplayName = "Should throw an error when action is invalid")]
        [Trait("E2E/Controllers", "Users / [v1]ChangeStatusUser")]
        public async Task ErrorWhenInvalidAction()
        {

            var userToChangeStatus = _fixture.GetValidUser();

            var input = _fixture.GetInputWithInvalidAction(userToChangeStatus.Id);

            var (response, output) = await _fixture.ApiClient.PutAsync<ProblemDetails>($"/v1/users/{Guid.NewGuid()}/status?action=INVALID", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            output.Should().NotBeNull();
            output.Title.Should().Be("Bad request");
            output.Type.Should().Be("BadRequest");
            output.Status.Should().Be(StatusCodes.Status400BadRequest);
            output.Detail.Should().Be("Invalid action. Only ACTIVATE or DEACTIVATE values are allowed");
        }


        [Fact(DisplayName = "Should throw an error when user not found")]
        [Trait("E2E/Controllers", "Users / [v1]ChangeStatusUser")]
        public async Task ErrorWhenNotFound()
        {
            var idToQuery = Guid.NewGuid();
            var input = _fixture.GetInputWithInvalidAction(_fixture.CommonUserId);

            var (response, output) = await _fixture.ApiClient.PutAsync<ProblemDetails>($"/v1/users/{idToQuery}/status?action=ACTIVATE", input);

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
            output.Should().NotBeNull();
            output!.Status.Should().Be(StatusCodes.Status404NotFound);
            output.Type.Should().Be("NotFound");
            output.Title.Should().Be("Not found");
            output.Detail.Should().Be($"User with ID '{idToQuery}' not found");
        }
    }
}
