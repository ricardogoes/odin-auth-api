using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odin.Auth.Application.Users;
using Odin.Auth.Application.Users.UpdateProfile;
using System.Net;
using System.Text.Json;

namespace Odin.Auth.EndToEndTests.Controllers.Users.UpdateUser
{

    [Collection(nameof(UpdateUserApiTestCollection))]
    public class UpdateUserApiTest
    {
        private readonly UpdateUserApiTestFixture _fixture;

        public UpdateUserApiTest(UpdateUserApiTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Should update a valid user")]
        [Trait("E2E/Controllers", "Users / [v1]UpdateUser")]
        public async Task UpdateValidUser()
        {
            var context = await _fixture.CreateDbContextAsync();
            await _fixture.SeedCustomerDataAsync(context);

            var input = _fixture.GetValidInput(_fixture.CommonUserId);

            var (response, output) = await _fixture.ApiClient.PutAsync<UserOutput>($"/v1/users/{_fixture.CommonUserId}", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            output.Should().NotBeNull();
            output.FirstName.Should().Be(input.FirstName);
            output.LastName.Should().Be(input.LastName);
            output.Email.Should().Be(input.Email);
        }

        [Fact(DisplayName = "Should throw an error when Id from route is different of Id from user")]
        [Trait("E2E/Controllers", "Users / [v1]UpdateUser")]
        public async Task ErrorWhenInvalidIds()
        {

            var context = await _fixture.CreateDbContextAsync();
            await _fixture.SeedCustomerDataAsync(context);

            var input = _fixture.GetValidInput(_fixture.CommonUserId);

            var (response, output) = await _fixture.ApiClient.PutAsync<ProblemDetails>($"/v1/users/{Guid.NewGuid()}", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            output.Should().NotBeNull();
            output.Title.Should().Be("Bad request");
            output.Type.Should().Be("BadRequest");
            output.Status.Should().Be(StatusCodes.Status400BadRequest);
            output.Detail.Should().Be("Invalid request");
        }

        [Theory(DisplayName = "Should throw an error with invalid data")]
        [Trait("E2E/Controllers", "Users / [v1]UpdateUser")]
        [MemberData(
            nameof(UpdateUserApiTestDataGenerator.GetInvalidInputs),
            MemberType = typeof(UpdateUserApiTestDataGenerator)
        )]
        public async Task ErrorWhenCantInstantiateUser(UpdateProfileInput input, string property, string expectedDetail)
        {
            var context = await _fixture.CreateDbContextAsync();
            await _fixture.SeedCustomerDataAsync(context);

            input.ChangeUserId(_fixture.CommonUserId);

            var (response, output) = await _fixture.ApiClient.PutAsync<ProblemDetails>($"/v1/users/{_fixture.CommonUserId}", input);

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

        [Fact(DisplayName = "Should throw an error when user not found")]
        [Trait("E2E/Controllers", "Users / [v1]UpdateUser")]
        public async Task ErrorWhenNotFound()
        {
            var context = await _fixture.CreateDbContextAsync();
            await _fixture.SeedCustomerDataAsync(context);

            var idToQuery = Guid.NewGuid();
            var input = _fixture.GetValidInput(idToQuery);

            var (response, output) = await _fixture.ApiClient.PutAsync<ProblemDetails>($"/v1/users/{idToQuery}", input);

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
