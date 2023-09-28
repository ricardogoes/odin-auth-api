using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odin.Auth.Api.Models.Users;
using Odin.Auth.Application.Users;
using Odin.Auth.Domain.Models;
using System.Net;
using System.Text.Json;

namespace Odin.Auth.EndToEndTests.Controllers.Users.CreateUser
{

    [Collection(nameof(CreateUserApiTestCollection))]
    public class CreateUserApiTest
    {
        private readonly CreateUserApiTestFixture _fixture;

        public CreateUserApiTest(CreateUserApiTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Should insert a valid user")]
        [Trait("E2E/Controllers", "Users / [v1]CreateUser")]
        public async Task InsertValidUser()
        {
            var context = await _fixture.CreateDbContextAsync();
            await _fixture.SeedCustomerDataAsync(context);

            var input = _fixture.GetValidInput();

            var (response, output) = await _fixture.ApiClient.PostAsync<UserOutput>("/v1/users", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            output.Username.Should().Be(input.Username);
            output.FirstName.Should().Be(input.FirstName);
            output.LastName.Should().Be(input.LastName);
            output.Email.Should().Be(input.Email);
            output.IsActive.Should().BeTrue();

            output.Groups.Should().NotBeNull();
            output.Groups.Should().HaveCount(1);
            output.Groups.First().Name.Should().Be("odin.baseline");
        }

        [Theory(DisplayName = "Should throw an error with invalid data")]
        [Trait("E2E/Controllers", "Users / [v1]CreateUser")]
        [MemberData(
            nameof(CreateUserApiTestDataGenerator.GetInvalidInputs),
            MemberType = typeof(CreateUserApiTestDataGenerator)
        )]
        public async Task ErrorWhenCantInstantiateUser(CreateUserApiRequest input, string property, string expectedDetail)
        {
            var context = await _fixture.CreateDbContextAsync();
            await _fixture.SeedCustomerDataAsync(context);

            var (response, output) = await _fixture.ApiClient.PostAsync<ProblemDetails>("/v1/users", input);

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
    }
}
